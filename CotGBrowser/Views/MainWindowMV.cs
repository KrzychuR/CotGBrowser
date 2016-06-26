using GotGLib;
using GotGLib.JS;
using Microsoft.Practices.Unity;
using System;
using System.Windows.Input;
using CefSharp.WinForms;
using System.Threading;
using System.Reflection;
using System.Windows.Threading;
using CotGBrowser.Common;
using System.Windows;
using GotGLib.DB;
using GotGLib.NH;
using System.Threading.Tasks;

namespace CotGBrowser.Views
{
    public class MainWindowMV: BaseModelView
    {
        public MainWindowMV()
        {
            //TestCmd = new SimpleCommand(this, (p) => DoRefreshPlayerName(), (p) => !IsBusy);

            if (IoCHelper.IsInitialized)
            {
                Url = @"https://www.crownofthegods.com";
                m_SyncCtx = SynchronizationContext.Current;

                string path = (AppDomain.CurrentDomain.GetData("DataDirectory") as string ?? "");

                if (!string.IsNullOrWhiteSpace(path))
                    path += @"\";

                StatusInfo = "DB file: " + path + @"data\db.sqlite";
                MainWindowTitle = "CotGBrowser, " + Assembly.GetExecutingAssembly().GetName().Version.ToString();

                JSInterface = IoCHelper.GetIoC().Resolve<JScriptInterface>();

                m_Timer = new DispatcherTimer();
                m_Timer.Tick += M_Timer_Tick;
                m_Timer.Interval = new TimeSpan(0, 0, 5);
                m_Timer.Start();

                NavigateCmd = new SimpleCommand(this, (p) => DoNavigateCmd(), (p) => !IsBusy);
                ShowRankingsCmd = new SimpleCommand(this, (p) => DoShowRankingsCmd(), (p) => !IsBusy && HasAccess2Reports);
                ShowMenuCmd = new SimpleCommand(this, (p) => DoShowMenuCmd());

                ShowDataCollectCmd = new SimpleCommand(this, (p) => DoShowDataCollectCmd(), (p) => !IsBusy && !string.IsNullOrWhiteSpace(PlayerName));

                ShowChatCmd = new SimpleCommand(this, (p) => DoShowChatCmd(), (p) => !IsBusy && !string.IsNullOrWhiteSpace(PlayerName));
                ClearCacheCmd = new SimpleCommand(this, (p) => DoClearCacheCmd());
                ShowOverviewsCmd = new SimpleCommand(this, (p) => DoShowOverviewsCmd(), (p) => !IsBusy && !string.IsNullOrWhiteSpace(PlayerName));
            }


        }

        private void DoShowOverviewsCmd()
        {
            IoCHelper.GetIoC().Resolve<OverviewsWindow>().Show();
        }

        private void DoClearCacheCmd()
        {
            CotGBrowser.Properties.Settings.Default.ClearCache = true;
            CotGBrowser.Properties.Settings.Default.Save();

            MessageBox.Show("Restart application please.");
        }

        private void DoShowChatCmd()
        {
            var wnd = IoCHelper.GetIoC().Resolve<ChatWindow>();
            //wnd.ModelView.Browser = Browser;
            wnd.Show();
        }

        private void DoShowDataCollectCmd()
        {
            var wnd = IoCHelper.GetIoC().Resolve<DataColectWindow>();
            wnd.ModelView.Browser = Browser;
            wnd.Show();
        }

        private void DoShowMenuCmd()
        {
            FlyoutVisible = true;
        }

        private void DoShowRankingsCmd()
        {
            var wnd = IoCHelper.GetIoC().Resolve<RankingsWindow>();
            wnd.Show();
        }

        private void M_Timer_Tick(object sender, EventArgs e)
        {
            DoRefreshPlayerName();
        }

        #region Polecenia

        public ICommand NavigateCmd { get; set; }

        public ICommand TestCmd { get; set; }

        public ICommand ShowRankingsCmd { get; set; }
        public ICommand ShowMenuCmd { get; set; }
        public ICommand ShowDataCollectCmd { get; set; }
        public ICommand ClearCacheCmd { get; set; }

        public ICommand ShowChatCmd { get; set; }
        public ICommand ShowOverviewsCmd { get; set; }
        #endregion

        #region Cechy

        private bool m_FlyoutVisible;

        public bool FlyoutVisible
        {
            get { return m_FlyoutVisible; }
            set { SetProperty(ref m_FlyoutVisible, value); }
        }

        private string m_PlayerName;

        public string PlayerName
        {
            get { return m_PlayerName; }
            set { SetProperty(ref m_PlayerName, value); }
        }

        private string m_BrowserStatusMsg;

        public string BrowserStatusMsg
        {
            get { return m_BrowserStatusMsg; }
            set { SetProperty(ref m_BrowserStatusMsg,value); }
        }

        private string m_MainWindowTitle;

        public string MainWindowTitle
        {
            get { return m_MainWindowTitle; }
            set { SetProperty(ref m_MainWindowTitle, value); }
        }

        private ChromiumWebBrowser m_Browser;

        public ChromiumWebBrowser Browser
        {
            get { return m_Browser; }
            set
            {
                if (SetProperty(ref m_Browser, value) && value != null)
                {
                    m_Browser.StatusMessage += M_Browser_StatusMessage;
                    m_Browser.LoadingStateChanged += M_Browser_LoadingStateChanged;
                }
            }
        }

        private void M_Browser_LoadingStateChanged(object sender, CefSharp.LoadingStateChangedEventArgs e)
        {
            m_SyncCtx.Post(new SendOrPostCallback(isLoading => {
                IsBusy = (bool)isLoading;
            }), e.IsLoading);
        }

        SynchronizationContext m_SyncCtx;

        private void M_Browser_StatusMessage(object sender, CefSharp.StatusMessageEventArgs e)
        {
            m_SyncCtx.Post(new SendOrPostCallback(msg => {
                BrowserStatusMsg = msg as string;
            }), e.Value);
        }

        private string  m_Url;

        public string Url
        {
            get { return m_Url; }
            set { SetProperty(ref m_Url, value); }
        }


        #endregion

        private DispatcherTimer m_Timer;

        private void DoNavigateCmd()
        {
            //Browser.Address = Url;
        }

        private JScriptInterface JSInterface { get; set; }

        [Dependency]
        protected Database Db { get; set; }

        private bool m_HasAccess2Reports;

        public bool HasAccess2Reports
        {
            get { return m_HasAccess2Reports; }
            set { SetProperty(ref m_HasAccess2Reports, value); }
        }


        private void DoRefreshPlayerName()
        {
            PlayerName = JSInterface.GetPlayerName();
            MainWindowTitle = string.Format("CotGBrowser, {0} ({1})",
                Assembly.GetExecutingAssembly().GetName().Version.ToString(),
                PlayerName);

            if (!string.IsNullOrWhiteSpace(PlayerName))
            {
                m_Timer.IsEnabled = false;
                HasAccess2Reports = true;
            }
        }
    }
}
