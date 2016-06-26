using CefSharp.WinForms;
using CotGBrowser.Common;
using GotGLib;
using GotGLib.JS;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Practices.Unity;

namespace CotGBrowser.Views
{
    public class DataColectWindowMV: BaseModelView
    {
        public DataColectWindowMV()
        {
            IsBrowserInitialized = false;
            InjectScriptsCmd = new SimpleCommand(this, (p) => DoInjectScriptsCmd(), (p) => !IsBusy && Browser != null);
            DownloadEmpireRankingsCmd = new SimpleCommand(this, (p) => DoDownloadEmpireRankingsCmd(), (p) => !IsBusy);
            TestCmd = new SimpleCommand(this, (p) => DoTestCmd(), (p) => !IsBusy);

            if (IoCHelper.IsInitialized)
            {
                JSInterface = IoCHelper.GetIoC().Resolve<JScriptInterface>();
                JSInterface.Progress += JSInterface_Progress;
            }
        }

        private void DoTestCmd()
        {
            JSInterface.InitAjaxHandlers();
        }

        #region Polecenia

        public ICommand DownloadEmpireRankingsCmd { get; set; }
        public ICommand InjectScriptsCmd { get; set; }

        public ICommand TestCmd { get; set; }

        #endregion

        #region Cechy

        private ChromiumWebBrowser m_Browser;

        public ChromiumWebBrowser Browser
        {
            get { return m_Browser; }
            set
            {
                if (SetProperty(ref m_Browser, value) && value != null)
                {
                }
            }
        }

        private bool m_IsBrowserInitialized;

        public bool IsBrowserInitialized
        {
            get { return m_IsBrowserInitialized; }
            set { SetProperty(ref m_IsBrowserInitialized, value); }
        }

        private ObservableCollection<string> m_Messages  = new ObservableCollection<string>();

        /// <summary>
        /// Komunikaty
        /// </summary>
        public ObservableCollection<string> Messages
        {
            get { return m_Messages; }
            set { m_Messages = value; }
        }

        private string m_LastMessage;

        public string LastMessage
        {
            get { return m_LastMessage; }
            set { SetProperty(ref m_LastMessage, value); }
        }

        private int m_TotalSteps;

        public int TotalSteps
        {
            get { return m_TotalSteps; }
            set { SetProperty(ref m_TotalSteps, value); }
        }

        private int m_CurrentStep;

        public int CurrentStep
        {
            get { return m_CurrentStep; }
            set { SetProperty(ref m_CurrentStep, value); }
        }

        private void DoInjectScriptsCmd()
        {
            if (Browser != null)
            {
                IsBusy = true;
                //JSInterface.InitBrowser(Browser);
                IsBrowserInitialized = true;
                IsBusy = false;
            }
        }

        private void DoDownloadEmpireRankingsCmd()
        {
            JSInterface.DownloadAllRankings();
        }

        private JScriptInterface JSInterface { get; set; }

        private void JSInterface_Progress(object sender, ProgressMessage e)
        {
            TotalSteps = e.Total;
            CurrentStep = e.Step;
            LastMessage = e.Message;

            if (!e.Message.StartsWith("~"))
                Messages.Add(e.Message);
        }

        #endregion
    }
}
