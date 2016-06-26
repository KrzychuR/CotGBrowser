using CefSharp;
using CefSharp.WinForms;
using GotGLib.DB;
using GotGLib.DTO;
using GotGLib.NH;
using log4net;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace GotGLib.JS
{
    public class ProgressMessage
    {
        public int Total { get; set; }
        public int Step { get; set; }
        public string Message { get; set; }
    }

    public enum ChatType { Other, World, Alliance, Officers, Whisper }

    public class ChatMessage
    {
        public ChatMessage()
        {
            Players = new List<string>();
            Reports = new List<string>();
            Aliances = new List<string>();
            Coords = new List<Coordinate>();
        }

        public ChatType ChatType { get; set; }
        public string Time { get; set; }
        public List<string> Players { get; set; }
        public List<string> Reports { get; set; }
        public List<string> Aliances { get; set; }
        public List<Coordinate> Coords { get; set; }
        public string Message { get; set; }
    }

    public class Coordinate
    {
        public string CityId { get; set; }
        public string Coords { get; set; }
    }

    public class JScriptInterface
    {
        public JScriptInterface()
        {
            log = log4net.LogManager.GetLogger(GetType());
            log.Info("Interface created...");
            m_SyncCtx = DispatcherSynchronizationContext.Current;

            m_Timer = new DispatcherTimer();
            m_Timer.Tick += M_Timer_Tick;
            m_Timer.Interval = new TimeSpan(0, 0, 2);
        }

        SynchronizationContext m_SyncCtx;

        #region API do JS

        public void failed(string status, string type, string continent)
        {
            try
            {
                log.ErrorFormat("Error: {0}, type:{0}, continent: {1}", status, type, continent);
                DoProgress(string.Format("Error: {0}, type:{1}, continent: {2}", status, type, continent));

                //m_Continents.Enqueue(type+":"+continent);

                Random rand = new Random();
                int sleepTime = rand.Next(1000, 10000);
                DoProgress("Sleep: " + sleepTime.ToString());

                //await Task.Run(() => System.Threading.Thread.Sleep(sleepTime));

                //pobieram następny z listy
                if (m_Continents.Count > 0)
                {
                    var paramStr = m_Continents.Dequeue();
                    var b = paramStr.Split(':')[1];
                    var a = paramStr.Split(':')[0];

                    log.InfoFormat("Invoking CotGBrowser_GetRanks({0}, {1}), ...", a, b);
                    DoProgress(string.Format("Invoking CotGBrowser_GetRanks({0}, {1}), ...", a, b));

                    m_Browser.ExecuteScriptAsync("CotGBrowser_GetRanks", a, b);
                }
                else
                {
                    log.Info("All jobs finished.");
                    DoProgress("All jobs finished.");
                    m_CurrentStep = m_TotalSteps;
                    DoJobFinished();
                }
            }
            catch(Exception e)
            {
                log.Error(e.Message, e);
            }
        }

        async public void rankings(string jsonData, string type, string continent)
        {
            try
            {
                log.InfoFormat("New rankings, type: {0}, continent: {1}, parsing...", type, continent);
                DoProgress(string.Format("New rankings, type:{0}, continent: {1}, parsing...", type, continent));

                var conv = new GameDataConverter();

                if (type == "0")
                {
                    var ranks = conv.GetRankings(jsonData);
                    log.InfoFormat("New empire rankings parsed, rows:{0}, updating database...", ranks.Count());
                    DoProgress(string.Format("New empire rankings parsed, rows:{0}, updating database...", ranks.Count()));

                    SaveCurrentEmpireRanking save = new SaveCurrentEmpireRanking();
                    save.Progress += (s, e) => DoProgress(e.Message);

                    await Task.Run(() =>
                    {
                        int c = int.Parse(continent);

                        foreach (var r in ranks)
                        {
                            r.Continent = c;
                        }

                        save.CurrentRankings = ranks;

                        Db.ExecuteWork(save);
                    });
                }
                else if(type == "5")
                {
                    var ranks = conv.GetUnitsKills(jsonData);
                    log.InfoFormat("New units kills rankings parsed, rows:{0}, updating database...", ranks.Count());
                    DoProgress(string.Format("New units kills rankings parsed, rows:{0}, updating database...", ranks.Count()));

                    SaveCurrentEmpireRanking save = new SaveCurrentEmpireRanking();
                    save.Progress += (s, e) => DoProgress(e.Message);

                    await Task.Run(() =>
                    {
                        foreach (var r in ranks)
                            r.Continent = 56;

                        save.CurrentRankings = ranks;
                        Db.ExecuteWork(save);
                    });
                }
                else if (type == "1")
                {
                    var ranks = conv.GetAliancesRanking(jsonData);
                    log.InfoFormat("New aliances rankings parsed, rows:{0}, updating database...", ranks.Count());
                    DoProgress(string.Format("New aliances rankings parsed, rows:{0}, updating database...", ranks.Count()));

                    SaveCurrentAlianceRanking save = new SaveCurrentAlianceRanking();
                    int total = ranks.Count;
                    int step = 0;

                    await Task.Run(() =>
                    {
                        foreach (var r in ranks)
                        {
                            r.Continent = int.Parse(continent);
                            save.AlianceScore = r;
                            Db.ExecuteWork(save);
                            step++;
                            DoProgress(string.Format("~Db updating.... {0}/{1}", step, total));
                        }
                    });
                }
                else if (type == "7")
                {
                    var ranks = conv.GetCaverns(jsonData);
                    log.InfoFormat("New caverns rankings parsed, rows:{0}, updating database...", ranks.Count());
                    DoProgress(string.Format("New caverns rankings parsed, rows:{0}, updating database...", ranks.Count()));

                    SaveCurrentEmpireRanking save = new SaveCurrentEmpireRanking();
                    save.Progress += (s, e) => DoProgress(e.Message);

                    await Task.Run(() =>
                    {
                        foreach (var r in ranks)
                            r.Continent = 56;

                        save.CurrentRankings = ranks;
                        Db.ExecuteWork(save);
                    });
                }
                else if (type == "4")
                {
                    //def rep
                    var ranks = conv.GetDefReputation(jsonData);
                    log.InfoFormat("New def reputation rankings parsed, rows:{0}, updating database...", ranks.Count());
                    DoProgress(string.Format("New def reputation rankings parsed, rows:{0}, updating database...", ranks.Count()));

                    SaveCurrentEmpireRanking save = new SaveCurrentEmpireRanking();
                    save.Progress += (s, e) => DoProgress(e.Message);

                    await Task.Run(() =>
                    {
                        foreach (var r in ranks)
                            r.Continent = 56;

                        save.CurrentRankings = ranks;
                        Db.ExecuteWork(save);
                    });
                }
                else if (type == "3")
                {
                    //off rep
                    var ranks = conv.GetOffReputation(jsonData);
                    log.InfoFormat("New off reputation rankings parsed, rows:{0}, updating database...", ranks.Count());
                    DoProgress(string.Format("New off reputation rankings parsed, rows:{0}, updating database...", ranks.Count()));

                    SaveCurrentEmpireRanking save = new SaveCurrentEmpireRanking();
                    save.Progress += (s, e) => DoProgress(e.Message);

                    await Task.Run(() =>
                    {
                        foreach (var r in ranks)
                            r.Continent = 56;

                        save.CurrentRankings = ranks;
                        Db.ExecuteWork(save);
                    });
                }

                log.Info("Database updated.");
                m_CurrentStep++;
                DoProgress("Database updated.");

                //pobieram następny z listy
                if (m_Continents.Count > 0)
                {
                    var paramStr = m_Continents.Dequeue();
                    var b = paramStr.Split(':')[1];
                    var a = paramStr.Split(':')[0];

                    log.InfoFormat("Invoking CotGBrowser_GetRanks({0}, {1}), ...", a, b);
                    DoProgress(string.Format("Invoking CotGBrowser_GetRanks({0}, {1}), ...", a, b));
                    
                    m_Browser.ExecuteScriptAsync("CotGBrowser_GetRanks", a, b);
                }
                else
                {
                    log.Info("All jobs finished.");
                    DoProgress("All jobs finished.");
                    m_CurrentStep = m_TotalSteps;
                    DoJobFinished();
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message, e);
                DoProgress(e.Message);
            }
        }

        public void world_chat_message(string txt, string html)
        {
            //log.Debug(txt);
            try
            {

                ChatType ct = ChatType.Other;

                if (txt.Contains("[World]"))
                {
                    ct = ChatType.World;
                }
                else if (txt.Contains("[Alliance]"))
                {
                    ct = ChatType.Alliance;
                }
                else if (txt.Contains("[Officer]"))
                {
                    ct = ChatType.Officers;
                }
                else if (txt.Contains("whisper"))
                {
                    ct = ChatType.Whisper;
                }

                var cm = new JS.ChatMessage { ChatType = ct, Message = txt };
                AnalyzeMsg(html, cm);

                //muszę tak bo jestem w innym wątku... i może boleć

                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    try
                    {
                        ChatMessages.Add(cm);
                    }
                    catch (Exception e)
                    {
                        log.Error(e.Message, e);
                    }
                }), DispatcherPriority.Normal);

                DoChatMessage(cm);
            }
            catch(Exception e)
            {
                log.Error(e.Message, e);
                log.Error("Message: "+html);
            }
        }

        public void ajax_success(string settings, string data)
        {
            log.DebugFormat("JS_settings: {0}", settings);
            log.DebugFormat("JS_data: {0}", data);
        }

        public void raids_repors(string data)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(data))
                {
                    var conv = new GameDataConverter();
                    var reports = conv.GetRaidReports(data);
                    DoCurrentRaidReports(reports);
                }
            }
            catch(Exception e)
            {
                log.Error(e.Message, e);
            }
        }

        public void raid_report(string data)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(data))
                {
                    log.Debug("raid_report: " + data);
                    var conv = new GameDataConverter();
                    var report = conv.GetRaidReport(data);
                    DoCurrentRaidReport(report);
                }
            }
            catch(Exception e)
            {
                log.Error(e.Message, e);
            }
        }

        public void troops_overview(string data)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(data))
                {
                    log.Debug("troops_overview: " + data);
                    var conv = new GameDataConverter();
                    var troops = conv.GetTroopsOverview(data);
                    DoTroopsOverview(troops);
                }
            }
            catch(Exception e)
            {
                log.Error(e.Message, e);
            }
        }

        #endregion

        async public void InitAjaxHandlers(IWebBrowser wb = null)
        {
            

            JavascriptResponse resp;

            if(wb == null)
                resp = await m_Browser.EvaluateScriptAsync(JSRes.AJAX);
            else
                resp = await wb.EvaluateScriptAsync(JSRes.AJAX);

            if (!resp.Success)
            {
                log.Error(resp.Message);
            }
            else
            {
                if(wb == null)
                    m_Browser.ExecuteScriptAsync("InitAjaxHandlers", "");
                else
                    wb.ExecuteScriptAsync("InitAjaxHandlers", "");
            }
        }

        private List<JS.ChatMessage> m_ChatMessages = new List<JS.ChatMessage>();

        public List<JS.ChatMessage> ChatMessages
        {
            get { return m_ChatMessages; }
            set { m_ChatMessages = value; }
        }

        private void AnalyzeMsg(string html, ChatMessage cm)
        {
            //gracze występujący w komunikacie

            //<span class="playerlink" data="Akoma">
            Regex r = new Regex("<span class=\"playerlink\" data=\"([a-zA-Z0-9_]+)\">");

            foreach (Match m in r.Matches(html))
            {
                if(!cm.Players.Contains(m.Groups[1].Value))
                    cm.Players.Add(m.Groups[1].Value);
            }

            //<span style="color:#7979FF !important" class="playerblink">DarkMoon</span>
            r = new Regex("<span style=\"color:....... !important\" class=\"playerblink\">([a-zA-Z0-9_]+)</span>");

            foreach (Match m in r.Matches(html))
            {
                if (!cm.Players.Contains(m.Groups[1].Value))
                    cm.Players.Add(m.Groups[1].Value);
            }

            //a teraz wszystkie raporty
            //<span class="replink gFrep" style="color:#7979FF !important" data="7584555197">Share Report:7584555197</span>
            r = new Regex("<span class=\"replink gFrep\" style=\"color:....... !important\" data=\"([0-9]*)\">");

            foreach (Match m in r.Matches(html))
            {
                if (!cm.Reports.Contains(m.Groups[1].Value))
                    cm.Reports.Add(m.Groups[1].Value);
            }

            //<span class="cityblink shcitt" style="color:#7979FF !important" data="3014888">232:46</span>
            r = new Regex("<span class=\"cityblink shcitt\" style=\"color:....... !important\" data=\"([0-9]*)\">([0-9]+:[0-9]+)</span>");

            foreach (Match m in r.Matches(html))
            {
                if(cm.Coords.FirstOrDefault(x => x.Coords == m.Groups[2].Value) == null)
                    cm.Coords.Add(new Coordinate() { CityId = m.Groups[1].Value, Coords = m.Groups[2].Value });
            }

            //<span class="allyblink chatblink " style="color:#7979FF !important">TheEvilHusaria</span>
            r = new Regex("<span class=\"allyblink chatblink \" style=\"color:....... !important\">([a-zA-Z0-9_\\s]+)</span>");

            foreach (Match m in r.Matches(html))
            {
                if (!cm.Aliances.Contains(m.Groups[1].Value))
                    cm.Aliances.Add(m.Groups[1].Value);
            }

        }


        public void InitBrowser(ChromiumWebBrowser wb)
        {
            m_Browser = wb;
            //m_Browser.LifeSpanHandler = new LifeSpanHandler();
            m_Browser.RegisterJsObject("hobbita", this);

            m_Browser.FrameLoadEnd += M_Browser_FrameLoadEnd;
        }

        public void InitBrowser(IWebBrowser wb)
        {
            if (m_Browser == null)
            {
                wb.RegisterJsObject("hobbita", this);
                wb.FrameLoadEnd += M_Browser_FrameLoadEnd;
            }
        }

        private void M_Browser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            log.Debug("Frame loaded" + e.Frame.Url);
        }

        private bool m_ChatInitialized = false;

        async public void InitChat()
        {
            if (!m_ChatInitialized)
            {
                m_ChatInitialized = true;
                var resp = await m_Browser.EvaluateScriptAsync(JSRes.ChatListener);

                if(!resp.Success)
                {
                    log.Error(resp.Message);
                }
                else
                {
                    m_Browser.ExecuteScriptAsync("CotGBrowser_RegisterChatListeners", "");
                }

                m_Timer.Start();
            }
        }

        private bool m_OverviewsInitialized = false;

        async public void InitOverviews()
        {
            if (!m_OverviewsInitialized)
            {
                m_OverviewsInitialized = true;
                var resp = await m_Browser.EvaluateScriptAsync(JSRes.Overviews);

                if (!resp.Success)
                {
                    log.Error(resp.Message);
                }
            }
        }

        public void RefreshRaidsReports()
        {
            InitOverviews();
            m_Browser.ExecuteScriptAsync("CotGBrowser_RefreshRaidReports", "");
        }

        public void RefreshRaidReport(string reportId)
        {
            InitOverviews();
            m_Browser.ExecuteScriptAsync("CotGBrowser_GetRaidReport", reportId);
        }

        public void RefreshTroopsOverview()
        {
            InitOverviews();
            m_Browser.ExecuteScriptAsync("CotGBrowser_RefreshTroopsOverview", "");
        }

        #region Zdarzenia 

        //Event odpalany gdy skończy się obsługa metody odpalonej przez JavaScript
        public event EventHandler JobFinished;

        private void DoJobFinished()
        {
            var h = JobFinished;

            if (h != null)
                h(this, new EventArgs());
        }

        public event EventHandler<ProgressMessage> Progress;

        private void DoProgress(string msg)
        {
            var h = Progress;

            if (h != null)
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    try
                    {
                        h(this, new ProgressMessage() { Message = msg as string, Total = m_TotalSteps, Step = m_CurrentStep });
                    }
                    catch(Exception e)
                    {
                        log.Error(e.Message, e);
                    }
                }), DispatcherPriority.Background);

                
            }
        }

        public event EventHandler<ChatMessage> ChatMessage;

        private void DoChatMessage(ChatMessage m)
        {
            var h = ChatMessage;

            if(h != null)
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    try
                    {
                        h(this, m);
                    }
                    catch(Exception e)
                    {
                        log.Error(e.Message, e);
                    }
                }), DispatcherPriority.Normal);
            }
        }

        public event EventHandler<string> ChatInputChanged;

        private void DoChatInputChanged(string txt)
        {
            var h = ChatInputChanged;

            if(h != null)
            {
                h(this, txt);
            }
        }

        public event EventHandler<List<RaidReport>> CurrentRaidReports;

        private void DoCurrentRaidReports(List<RaidReport> list)
        {
            var h = CurrentRaidReports;

            if(h != null)
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    try
                    {
                        h(this, list);
                    }
                    catch (Exception e)
                    {
                        log.Error(e.Message, e);
                    }
                }), DispatcherPriority.Normal);
            }
        }

        public event EventHandler<RaidReport> CurrentRaidReport;

        private void DoCurrentRaidReport(RaidReport rep)
        {
            var h = CurrentRaidReport;

            if (h != null)
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    try
                    {
                        h(this, rep);
                    }
                    catch (Exception e)
                    {
                        log.Error(e.Message, e);
                    }
                }), DispatcherPriority.Normal);
            }
        }

        public event EventHandler<List<CityOverview>> TroopsOverview;

        private void DoTroopsOverview(List<CityOverview> troops)
        {
            var h = TroopsOverview;

            if (h != null)
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    try
                    {
                        h(this, troops);
                    }
                    catch (Exception e)
                    {
                        log.Error(e.Message, e);
                    }
                }), DispatcherPriority.Normal);
            }
        }

        #endregion

        private string PrepareMessage(string msg)
        {
            //nie może być '
            if (msg == null)
                msg = "";

            var m = msg.Replace(@"'", @"\'");

            return m;
        }

        public void SendAllyChatMessage(string msg)
        {
            var l = new List<string>();
            l.Add(msg);
            SendAllyChatMessage(l);
        }

        public void SendAllyChatMessage(List<string> msg)
        {
            m_ChatInputCheckingEnabled = false;
            Task task;

            foreach (var m in msg)
            {
                task = m_Browser.EvaluateScriptAsync(string.Format("$('#achatMsg').val('{0}')", PrepareMessage(m)));

                if (!task.Wait(1000)) return;

                task = m_Browser.EvaluateScriptAsync("$('#asendChat').click()");

                if (!task.Wait(1000)) return;

                task = m_Browser.EvaluateScriptAsync(string.Format("$('#achatMsg').val('{0}')", ""));

                if (!task.Wait(1000)) return;
            }

            m_ChatInputCheckingEnabled = true;
        }

        async public void SendWorldChatMessage(string msg)
        {
            m_ChatInputCheckingEnabled = false;
            await m_Browser.EvaluateScriptAsync(string.Format("$('#chatMsg').val('{0}')", PrepareMessage(msg)));
            await m_Browser.EvaluateScriptAsync("$('#sendChat').click()");
            await m_Browser.EvaluateScriptAsync(string.Format("$('#chatMsg').val('{0}')", ""));
            m_ChatInputCheckingEnabled = true;
        }

        async public void SendOfficerChatMessage(string msg)
        {
            m_ChatInputCheckingEnabled = false;
            await m_Browser.EvaluateScriptAsync(string.Format("$('#ochatMsg').val('{0}')", PrepareMessage(msg)));
            await m_Browser.EvaluateScriptAsync("$('#osendChat').click()");
            await m_Browser.EvaluateScriptAsync(string.Format("$('#ochatMsg').val('{0}')", ""));
            m_ChatInputCheckingEnabled = true;
        }

        public void ShowReport(string repNo)
        {
            //< span class="replink gFrep" style="color:#7979FF !important" data="204932426">
            m_Browser.ExecuteScriptAsync(string.Format("$(\"span[class='replink gFrep'][data='{0}']\")[0].click()", repNo));
        }

        public void ShowPlayer(string player)
        {
            //< <span class="playerlink" data="Lalwande">
            m_Browser.ExecuteScriptAsync(string.Format("$(\"span[class='playerblink']:contains('{0}')\")[0].click()", player));
        }

        public void ShowCityCoords(string coords)
        {
            //<span class="cityblink shcitt" style="color:#7979FF !important" data="12189933">237:186</span>
            m_Browser.ExecuteScriptAsync(string.Format("$(\"span[class='cityblink shcitt'][data='{0}']\")[0].click()", coords));
        }

        public void ShowAlliance(string aliance)
        {
            //<span class="allyblink chatblink " style="color:#7979FF !important">TheEvilHusaria</span>
            //$("span[class='allyblink chatblink ']:contains('TheEvilHusaria')")[0].click()
            m_Browser.ExecuteScriptAsync(string.Format("$(\"span[class='allyblink chatblink ']:contains('{0}')\")[0].click()", aliance));
        }

        public bool IsOfficer()
        {
            var task = m_Browser.EvaluateScriptAsync("$('#officerChat').length", new TimeSpan(0, 0, 2));

            try
            {
                task.Wait();
            }
            catch (Exception aExc)
            {
                log.Error(aExc.Message, aExc);
                return false;
            }

            if (task.Result.Success)
            {
                return task.Result.Result.ToString() == "1";
            }
            else
                return false;
        }

        async public void DownloadAllRankings()
        {
            await m_Browser.EvaluateScriptAsync(JSRes.GetRanks);

            /// --punktacja ogólna
            m_Continents.Enqueue("0:56");

            ///-------- Punktacja na kontynentach
            m_Continents.Enqueue("0:21");
            m_Continents.Enqueue("0:00");
            m_Continents.Enqueue("0:01");
            m_Continents.Enqueue("0:02");
            m_Continents.Enqueue("0:03");
            m_Continents.Enqueue("0:04");
            m_Continents.Enqueue("0:05");

            m_Continents.Enqueue("0:10");
            m_Continents.Enqueue("0:11");
            m_Continents.Enqueue("0:12");
            m_Continents.Enqueue("0:13");
            m_Continents.Enqueue("0:14");
            m_Continents.Enqueue("0:15");

            m_Continents.Enqueue("0:20");
            m_Continents.Enqueue("0:22");
            m_Continents.Enqueue("0:23");
            m_Continents.Enqueue("0:24");
            m_Continents.Enqueue("0:25");

            m_Continents.Enqueue("0:30");
            m_Continents.Enqueue("0:31");
            m_Continents.Enqueue("0:32");
            m_Continents.Enqueue("0:33");
            m_Continents.Enqueue("0:34");
            m_Continents.Enqueue("0:35");

            m_Continents.Enqueue("0:40");
            m_Continents.Enqueue("0:41");
            m_Continents.Enqueue("0:42");
            m_Continents.Enqueue("0:43");
            m_Continents.Enqueue("0:44");
            m_Continents.Enqueue("0:45");

            m_Continents.Enqueue("0:50");
            m_Continents.Enqueue("0:51");
            m_Continents.Enqueue("0:52");
            m_Continents.Enqueue("0:53");
            m_Continents.Enqueue("0:54");
            m_Continents.Enqueue("0:55");

            //--- sojusze na kontynentach
            m_Continents.Enqueue("1:00");
            m_Continents.Enqueue("1:01");
            m_Continents.Enqueue("1:02");
            m_Continents.Enqueue("1:03");
            m_Continents.Enqueue("1:04");
            m_Continents.Enqueue("1:05");

            m_Continents.Enqueue("1:10");
            m_Continents.Enqueue("1:11");
            m_Continents.Enqueue("1:12");
            m_Continents.Enqueue("1:13");
            m_Continents.Enqueue("1:14");
            m_Continents.Enqueue("1:15");

            m_Continents.Enqueue("1:20");
            m_Continents.Enqueue("1:21");
            m_Continents.Enqueue("1:22");
            m_Continents.Enqueue("1:23");
            m_Continents.Enqueue("1:24");
            m_Continents.Enqueue("1:25");

            m_Continents.Enqueue("1:30");
            m_Continents.Enqueue("1:31");
            m_Continents.Enqueue("1:32");
            m_Continents.Enqueue("1:33");
            m_Continents.Enqueue("1:34");
            m_Continents.Enqueue("1:35");

            m_Continents.Enqueue("1:40");
            m_Continents.Enqueue("1:41");
            m_Continents.Enqueue("1:42");
            m_Continents.Enqueue("1:43");
            m_Continents.Enqueue("1:44");
            m_Continents.Enqueue("1:45");

            m_Continents.Enqueue("1:50");
            m_Continents.Enqueue("1:51");
            m_Continents.Enqueue("1:52");
            m_Continents.Enqueue("1:53");
            m_Continents.Enqueue("1:54");
            m_Continents.Enqueue("1:55");

            //--- units kills
            m_Continents.Enqueue("5:x");
    
            //--- def rep and off rep
            m_Continents.Enqueue("4:x");
            m_Continents.Enqueue("3:x");

            //--- caverns
            m_Continents.Enqueue("7:x");

            m_TotalSteps = m_Continents.Count;
            m_CurrentStep = 0;

            if (m_Browser != null && m_Continents.Count > 0)
            {
                var paramStr = m_Continents.Dequeue();
                var b = paramStr.Split(':')[1];
                var a = paramStr.Split(':')[0];

                log.InfoFormat("Invoking CotGBrowser_GetRanks({0}, {1}), ...", a, b);
                DoProgress(string.Format("Invoking CotGBrowser_GetRanks({0}, {1}), ...", a, b));

                m_Browser.ExecuteScriptAsync("CotGBrowser_GetRanks", a, b);
                //m_Browser.InvokeScript("CotGBrowser_GetRanks", a, b);
            }
        }

        //lista numerów kontynentów do pobrania statystyk
        protected Queue<string> m_Continents = new Queue<string>();

        protected ILog log;

        protected ChromiumWebBrowser m_Browser;

        protected int m_TotalSteps;

        protected int m_CurrentStep;
        //private string res;

        [Dependency]
        protected Database Db { get; set; }

        [Dependency]
        protected IUnityContainer IoC { get; set; }

        public string GetPlayerName()
        {
            if (m_Browser.IsBrowserInitialized)
            {
                var task = m_Browser.EvaluateScriptAsync("$('#playerName').html()", new TimeSpan(0, 0, 0, 2));

                try
                {
                    task.Wait();
                }
                catch (Exception aExc)
                {
                    log.Error(aExc.Message, aExc);
                    return null;
                }

                if (task.Result.Success)
                {
                    return task.Result.Result as string;
                }
                else
                    return null;
            }
            else
                return null;
        }

        public string GetWorldNum()
        {
            if (m_Browser.IsBrowserInitialized)
            {
                var task = m_Browser.EvaluateScriptAsync("$('#worldnum').text()", new TimeSpan(0, 0, 0, 2));

                try
                {
                    task.Wait();
                }
                catch (Exception aExc)
                {
                    log.Error(aExc.Message, aExc);
                    return null;
                }

                if (task.Result.Success)
                {
                    return task.Result.Result as string;
                }
                else
                    return null;
            }
            else
                return null;
        }

        public string GetAllyChatInput()
        {
            if (m_Browser != null && m_Browser.IsBrowserInitialized)
            {
                var task = m_Browser.EvaluateScriptAsync("$('#achatMsg').val()", new TimeSpan(0, 0, 10));

                try
                {
                    task.Wait();
                }
                catch (Exception aExc)
                {
                    log.Error(aExc.Message, aExc);
                    return null;
                }

                if (task.Result.Success)
                {
                    return task.Result.Result as string;
                }
                else
                    return null;
            }
            else
                return null;
        }

        public string GetWorldChatInput()
        {
            if (m_Browser.IsBrowserInitialized)
            {
                var task = m_Browser.EvaluateScriptAsync("$('#chatMsg').val()", new TimeSpan(0, 0, 2));

                try
                {
                    task.Wait();
                }
                catch (Exception aExc)
                {
                    log.Error(aExc.Message, aExc);
                    return null;
                }

                if (task.Result.Success)
                {
                    return task.Result.Result as string;
                }
                else
                    return null;
            }
            else
                return null;
        }

        public string GetOfficerChatInput()
        {
            if (m_Browser.IsBrowserInitialized)
            {
                var task = m_Browser.EvaluateScriptAsync("$('#ochatMsg').val()", new TimeSpan(0, 0, 2));

                try
                {
                    task.Wait();
                }
                catch (Exception aExc)
                {
                    log.Error(aExc.Message, aExc);
                    return null;
                }

                if (task.Result.Success)
                {
                    return task.Result.Result as string;
                }
                else
                    return null;
            }
            else
                return null;
        }

        private string m_LastAllyChatInput = "";
        private string m_LastWorldChatInput = "";
        private string m_LastOfficerChatInput = "";
        private bool m_ChatInputCheckingEnabled = true;

        private DispatcherTimer m_Timer;

        private string RemoveTagsFromInput(string input)
        {
            if(input != null)
            {
                input = input.Replace("<coords>", "").Replace("</coords>", "");
            }

            return input;
        }

        private void M_Timer_Tick(object sender, EventArgs e)
        {
            if (m_Browser == null) return;

            m_Timer.IsEnabled = false;

            try
            {
                if(m_ChatInputCheckingEnabled)
                {
                    var input = RemoveTagsFromInput(GetAllyChatInput());
                    

                    if (m_LastAllyChatInput != input && input != null)
                    {
                        DoChatInputChanged(input);
                        m_LastAllyChatInput = input;
                        m_Browser.EvaluateScriptAsync(string.Format("$('#achatMsg').val('{0}')", ""));
                    }
                    else
                    {
                        input = RemoveTagsFromInput(GetOfficerChatInput());

                        if(m_LastOfficerChatInput != input && input != null)
                        {
                            DoChatInputChanged(input);
                            m_LastOfficerChatInput = input;
                            m_Browser.EvaluateScriptAsync(string.Format("$('#ochatMsg').val('{0}')", ""));
                        }
                        else
                        {
                            input = RemoveTagsFromInput(GetWorldChatInput());

                            if (m_LastWorldChatInput != input && input != null)
                            {
                                DoChatInputChanged(input);
                                m_LastWorldChatInput = input;
                                m_Browser.EvaluateScriptAsync(string.Format("$('#chatMsg').val('{0}')", ""));
                            }
                        }
                    }
                }
            }
            finally
            {
                m_Timer.IsEnabled = true;
            }
        }
    }
}
