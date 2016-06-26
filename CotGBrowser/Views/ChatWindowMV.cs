using CotGBrowser.Common;
using GotGLib;
using GotGLib.JS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using System.Windows.Documents;
using System.Windows.Media;
using System.IO;
using System.Windows.Input;
using System.Windows;
using System.Text.RegularExpressions;

namespace CotGBrowser.Views
{
    public class ChatWindowMV: BaseModelView
    {
        public ChatWindowMV()
        {
            m_IsWorldVisible = true;
            m_IsAllyVisible = true;
            m_IsOfficerVisible = true;
            m_IsWhisperVisible = true;
            m_Out2Ally = true;
            m_Out2Officers = false;
            m_Out2World = false;
            m_AutoScroll = true;
            m_IsWhisperMode = false;
            m_IsOfficer = true;
            m_WndState = WindowState.Normal;    

            if (IoCHelper.IsInitialized)
            {
                Notify = IoCHelper.GetIoC().Resolve<CotGNotifyIcon>();

                ChatDoc = new FlowDocument();
                ChatDoc.PagePadding = new Thickness(1);
                
                m_ChatP = new Paragraph();
                m_ChatP.FontFamily = new FontFamily("Segoe UI");
                m_ChatP.FontSize = 13.333;
                ChatDoc.Blocks.Add(m_ChatP);

                JSInterface = IoCHelper.GetIoC().Resolve<JScriptInterface>();
                JSInterface.InitChat();
                JSInterface.ChatMessage += JSInterface_ChatMessage;
                JSInterface.ChatInputChanged += JSInterface_ChatInputChanged;

                IsOfficer = JSInterface.IsOfficer();

                ReloadMessages();
            }
        }

        private void JSInterface_ChatInputChanged(object sender, string e)
        {
            OutMsg += e;
        }

        private bool? m_IsOfficer;

        public bool? IsOfficer
        {
            get { return m_IsOfficer; }
            set { SetProperty(ref m_IsOfficer, value); }
        }

        private WindowState m_WndState;

        public WindowState WndState
        {
            get { return m_WndState; }
            set { SetProperty(ref m_WndState, value); }
        }


        private bool? m_AutoScroll;

        public bool? AutoScroll
        {
            get { return m_AutoScroll; }
            set { SetProperty(ref m_AutoScroll, value); }
        }

        private FlowDocument m_ChatDoc;

        public FlowDocument ChatDoc
        {
            get { return m_ChatDoc; }
            set { SetProperty(ref m_ChatDoc, value); }
        }

        private bool? m_IsWorldVisible;

        public bool? IsWorldVisible
        {
            get { return m_IsWorldVisible; }
            set
            {
                if (SetProperty(ref m_IsWorldVisible, value))
                    ReloadMessages();
            }
        }

        private bool? m_Out2World;

        public bool? Out2World
        {
            get { return m_Out2World; }
            set { SetProperty(ref m_Out2World, value); }
        }

        private bool? m_IsAllyVisible;

        public bool? IsAllyVisible
        {
            get { return m_IsAllyVisible; }
            set
            {
                if (SetProperty(ref m_IsAllyVisible, value))
                    ReloadMessages();
            }
        }

        private bool? m_Out2Ally;

        public bool? Out2Ally
        {
            get { return m_Out2Ally; }
            set
            {
                if( SetProperty(ref m_Out2Ally, value) )
                {
                    if (m_Out2Ally.HasValue && m_Out2Ally.Value)
                        IsAllyVisible = true;
                }
            }
        }

        private bool? m_IsOfficerVisible;

        public bool? IsOfficerVisible
        {
            get { return m_IsOfficerVisible; }
            set
            {
                if(SetProperty(ref m_IsOfficerVisible, value))
                    ReloadMessages();
            }
        }

        private bool? m_Out2Officers;

        public bool? Out2Officers
        {
            get { return m_Out2Officers; }
            set
            {
                if( SetProperty(ref m_Out2Officers, value) )
                {
                    if (m_Out2Officers.HasValue && m_Out2Officers.Value)
                        IsOfficerVisible = true;
                }
            }
        }

        private bool? m_IsWhisperVisible;

        public bool? IsWhisperVisible
        {
            get { return m_IsWhisperVisible; }
            set
            {
                if(SetProperty(ref m_IsWhisperVisible, value))
                    ReloadMessages();
            }
        }

        private string m_OutMsg;

        public string OutMsg
        {
            get { return m_OutMsg; }
            set { SetProperty(ref m_OutMsg, value); }
        }

        private bool? m_IsWhisperMode;

        public bool? IsWhisperMode
        {
            get { return m_IsWhisperMode; }
            set { SetProperty(ref m_IsWhisperMode, value); }
        }

        private string m_PlayerFilter;

        public string PlayerFilter
        {
            get { return m_PlayerFilter; }
            set
            {
                if(SetProperty( ref m_PlayerFilter, value))
                {
                    ReloadMessages();
                }
            }
        }

        private string m_WhisperTo;

        public string WhisperTo
        {
            get { return m_WhisperTo; }
            set { SetProperty(ref m_WhisperTo, value); }
        }

        public void SendMessage()
        {
            m_OutHistory.Add(OutMsg);
            m_histPos = m_OutHistory.Count - 1;

            List<string> replaced;

            string outMsg = OutMsg+" "; //dodaj zeby wyr. regularne zadziałało

            var //@@Aliance ->  <alliance>Aliance</alliance> 
            r = new Regex("(@@[a-zA-Z0-9_]+)+");
            replaced = new List<string>();

            foreach (Match m in r.Matches(outMsg))
            {
                if (!replaced.Contains(m.Groups[1].Value))
                {
                    outMsg = outMsg.Replace(m.Groups[1].Value, string.Format("<alliance>{0}</alliance>", m.Groups[1].Value.Substring(1).Trim()));
                    replaced.Add(m.Groups[1].Value);
                }
            }

            //@Player -> <player>Player</player>
            r = new Regex("(@[a-zA-Z0-9_]+)+");
            replaced = new List<string>();

            foreach (Match m in r.Matches(outMsg))
            {
                if (!replaced.Contains(m.Groups[1].Value))
                {
                    outMsg = outMsg.Replace(m.Groups[1].Value, string.Format("<player>{0}</player>", m.Groups[1].Value.Substring(1).Trim()));
                    replaced.Add(m.Groups[1].Value);
                }
            }

            r = new Regex("#([0-9]{1,3}:[0-9]{1,3})+");
            replaced = new List<string>();

            foreach (Match m in r.Matches(outMsg))
            { 
                if (!replaced.Contains(m.Groups[1].Value))
                {
                    outMsg = outMsg.Replace('#'+m.Groups[1].Value, string.Format("<coords>{0}</coords>", m.Groups[1].Value.Trim()));
                    replaced.Add(m.Groups[1].Value);
                }
            }


            outMsg = outMsg.Trim();

            if (IsWhisperMode.HasValue ? IsWhisperMode.Value : false && !string.IsNullOrWhiteSpace(WhisperTo))
            {
                var users = WhisperTo.Split(';');

                foreach (var u in users)
                {
                    var msgs = new List<string>();

                    if (!string.IsNullOrWhiteSpace(u))
                    {
                        msgs.Add(string.Format("/w {0} {1}", u, outMsg));
                    }

                    JSInterface.SendAllyChatMessage(msgs);
                }
            }
            else
            {
                if (Out2Ally.HasValue ? Out2Ally.Value : false)
                    JSInterface.SendAllyChatMessage(outMsg);
                else if (Out2World.HasValue ? Out2World.Value : false)
                    JSInterface.SendWorldChatMessage(outMsg);
                else if (Out2Officers.HasValue ? Out2Officers.Value : false)
                    JSInterface.SendOfficerChatMessage(outMsg);
            }

            OutMsg = "";
        }

        private int m_histPos;

        public void ShowPrevMsg()
        {
            if (m_OutHistory.Count > 0)
            {
                OutMsg = m_OutHistory[m_histPos];
                m_histPos--;

                if (m_histPos < 0)
                    m_histPos = 0;
            }
        }

        public void ShowNextMsg()
        {
            if (m_OutHistory.Count > 0)
            {
                if (m_histPos < m_OutHistory.Count - 1)
                {
                    m_histPos++;
                }

                OutMsg = m_OutHistory[m_histPos];
            }
        }

        private List<String> m_OutHistory = new List<string>();

        async private void ReloadMessages()
        {
            IsBusy = true;

            try
            {
                m_ChatP.Inlines.Clear();

                Paragraph histP = new Paragraph();
                histP.FontFamily = new FontFamily("Segoe UI");
                histP.FontSize = 13.333;

                StringBuilder sb = new StringBuilder();
                string xaml = "";

                await Task.Run(() =>
                {
                    foreach (var cm in JSInterface.ChatMessages)
                    {
                        if (cm.Players != null && cm.Players.Count > 0)
                        {
                            if (!IsAcceptedPlayer(cm.Players[0]))
                                continue;
                        }

                        xaml = GetXamlMsg(cm);

                        if (xaml != null)
                            sb.Append(xaml);
                    }

                    xaml = string.Format("<Span xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" >{0}</Span>",
                        sb.ToString());
                });

                AddXamlMsg2Doc(xaml, histP, "err :(");
                ChatDoc.Blocks.Clear();
                ChatDoc.Blocks.Add(histP);
                ChatDoc.Blocks.Add(m_ChatP);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool IsAcceptedPlayer(string pn)
        {
            if (string.IsNullOrWhiteSpace(pn) || string.IsNullOrWhiteSpace(PlayerFilter))
                return true;

            if (PlayerFilter[0] == '~')
            {
                if (pn.ToUpper() == PlayerFilter.Substring(1).ToUpper())
                    return false;
            }
            else
            {
                var users = PlayerFilter.ToUpper().Split(';');
                return users.Contains(pn.ToUpper());

                //if (pn.ToUpper() != PlayerFilter.ToUpper())
                  //  return false;
            }

            return true;
        }

        private void JSInterface_ChatMessage(object sender, ChatMessage e)
        {
            AddMessage(e);
        }

        private string AddXamlLinks(string msg, ChatMessage cm)
        {
            var res = msg;

            string foreground = null;

            switch (cm.ChatType)
            {
                case ChatType.World:
                    foreground = "#F3D298";
                    break;
                case ChatType.Alliance:
                    foreground = "#78B042";
                    break;
                case ChatType.Officers:
                    foreground = "#B6CE9A";
                    break;
                case ChatType.Whisper:
                    foreground = "#FF88FF";
                    break;
                default:
                    foreground = "White";
                    break;
            }

            foreach (string player in cm.Players)
            {
                res = res.Replace(player, string.Format("<Hyperlink Foreground=\"{3}\" TextDecorations=\"{0}\" NavigateUri=\"{1}\">{2}</Hyperlink>",
                    "{x:Null}", "p" + player, player, foreground));
            }

            foreach (string rep in cm.Reports)
            {
                res = res.Replace(rep, string.Format("<Hyperlink Foreground=\"{3}\" TextDecorations=\"{0}\" NavigateUri=\"{1}\">{2}</Hyperlink>",
                    "{x:Null}", "r" + rep, rep, "#FF0066CC"));
            }

            foreach (var coord in cm.Coords)
            {
                res = res.Replace(coord.Coords, string.Format("<Hyperlink Foreground=\"{3}\" TextDecorations=\"{0}\" NavigateUri=\"{1}\">{2}</Hyperlink>",
                    "{x:Null}", "c" + coord.CityId, coord.Coords, "#FF0066CC"));
            }

            foreach (string aliance in cm.Aliances)
            {
                res = res.Replace(aliance, string.Format("<Hyperlink Foreground=\"{3}\" TextDecorations=\"{0}\" NavigateUri=\"{1}\">{2}</Hyperlink>",
                    "{x:Null}", "a" + aliance, aliance, foreground));
            }

            //emotki

            var imgTemplate = "<InlineUIContainer BaselineAlignment=\"Center\">" +
                                "<Image Height=\"15\" Margin=\"2 0 2 0\" Source=\"pack://application:,,,/emotions/{0}\"></Image>" +
                               "</InlineUIContainer>";

            res = res.Replace(" :)", string.Format(imgTemplate, "icon_smile.gif"));
            res = res.Replace(" :-)", string.Format(imgTemplate, "icon_smile.gif"));
            res = res.Replace(" ;)", string.Format(imgTemplate, "icon_wink.gif"));
            res = res.Replace(" ;-)", string.Format(imgTemplate, "icon_wink.gif"));
            res = res.Replace(" :D", string.Format(imgTemplate, "icon_biggrin.gif"));
            res = res.Replace(" :-D", string.Format(imgTemplate, "icon_biggrin.gif"));
            res = res.Replace(" :|", string.Format(imgTemplate, "icon_neutral.gif"));
            res = res.Replace(" :-|", string.Format(imgTemplate, "icon_neutral.gif"));
            res = res.Replace(" :/", string.Format(imgTemplate, "icon_confused.gif"));
            res = res.Replace(" :-/", string.Format(imgTemplate, "icon_confused.gif"));
            res = res.Replace(" :(", string.Format(imgTemplate, "icon_sad.gif"));
            res = res.Replace(" :-(", string.Format(imgTemplate, "icon_sad.gif"));
            res = res.Replace(" :o", string.Format(imgTemplate, "icon_surprised.gif"));
            res = res.Replace(" :-o", string.Format(imgTemplate, "icon_surprised.gif"));
            res = res.Replace(" :P", string.Format(imgTemplate, "icon_razz.gif"));
            res = res.Replace(" :-P", string.Format(imgTemplate, "icon_razz.gif"));
            res = res.Replace(" :[", string.Format(imgTemplate, "icon_mad.gif"));
            res = res.Replace(" :-[", string.Format(imgTemplate, "icon_mad.gif"));

            return res;
        }

        private string GetXamlMsg(ChatMessage cm)
        {
            //<Span xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"><Hyperlink NavigateUri="http://www.google.com/">Google Home Page</Hyperlink></Span>

            string xaml = null;
            var spanTempl = string.Format("<Span Foreground=\"{0}\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" >{1}<LineBreak /></Span>",
                "__FOREGROUND__", System.Web.HttpUtility.HtmlEncode(cm.Message));

            if (cm.ChatType == ChatType.World && (IsWorldVisible.HasValue ? IsWorldVisible.Value : false))
            {
                xaml = spanTempl.Replace("__FOREGROUND__", "#F3D298");
            }
            else if (cm.ChatType == ChatType.Alliance && (IsAllyVisible.HasValue ? IsAllyVisible.Value : false))
            {
                xaml = spanTempl.Replace("__FOREGROUND__", "#78B042");
            }
            else if (cm.ChatType == ChatType.Officers && (IsOfficerVisible.HasValue ? IsOfficerVisible.Value : false))
            {
                xaml = spanTempl.Replace("__FOREGROUND__", "#B6CE9A");
            }
            else if (cm.ChatType == ChatType.Whisper && (IsWhisperVisible.HasValue ? IsWhisperVisible.Value : false))
            {
                xaml = spanTempl.Replace("__FOREGROUND__", "#FF88FF");
            }
            else if(cm.ChatType == ChatType.Other)
            {
                xaml = spanTempl.Replace("__FOREGROUND__", "White");
            }

            if (xaml != null)
                xaml = AddXamlLinks(xaml, cm);

            return xaml;
        }

        private void AddXamlMsg2Doc(string xamlMsg, Paragraph p, string plainMsg)
        {
            TextRange tr = new TextRange(p.ContentEnd, p.ContentEnd);
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(xamlMsg));

            bool err = false;
            try
            {
                tr.Load(stream, "Xaml");
            }
            catch(Exception e)
            {
                Log.Error("Invalid XAML: " + xamlMsg);
                Log.Error(e.Message, e);
                err = true;
            }

            if (err)
                p.Inlines.Add(new Run(plainMsg));
        }

        private void AddMessage(ChatMessage cm)
        {
            //fitrujemy po graczu
            if(cm.Players.Count > 0)
            {
                //pierwsz to ten kto pisze do nas
                if (!IsAcceptedPlayer(cm.Players[0]))
                    return;
            }

            if(cm.ChatType == ChatType.Whisper && WndState == WindowState.Minimized)
            {
                Notify.Info(cm.Message);
            }

            string xaml = GetXamlMsg(cm);

            if (xaml != null)
            {
                AddXamlMsg2Doc(xaml, m_ChatP, cm.Message);

                if(AutoScroll.HasValue ? AutoScroll.Value : false)
                    DoScrollDocumentToBottom();
            }
        }

        public event EventHandler ScrollDocumentToBottom;

        private void DoScrollDocumentToBottom()
        {
            var h = ScrollDocumentToBottom;

            if (h != null)
                h(this, new EventArgs());
        }

        public void HyperLinkClicked(object sender, RoutedEventArgs e)
        {
            var hl = e.Source as Hyperlink;

            if(hl != null && hl.NavigateUri != null)
            {
                if(hl.NavigateUri.OriginalString.StartsWith("r"))
                {
                    JSInterface.ShowReport(hl.NavigateUri.OriginalString.Substring(1));
                }
                else if(hl.NavigateUri.OriginalString.StartsWith("p"))
                {
                    JSInterface.ShowPlayer(hl.NavigateUri.OriginalString.Substring(1));
                }
                else if (hl.NavigateUri.OriginalString.StartsWith("c"))
                {
                    JSInterface.ShowCityCoords(hl.NavigateUri.OriginalString.Substring(1));
                }
                else if (hl.NavigateUri.OriginalString.StartsWith("a"))
                {
                    JSInterface.ShowAlliance(hl.NavigateUri.OriginalString.Substring(1));
                }
            }
        }

        private Paragraph m_ChatP;

        private JScriptInterface JSInterface { get; set; }

        private CotGNotifyIcon Notify { get; set; }
    }
}
