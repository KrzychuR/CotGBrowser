using CefSharp;
using GotGLib.JS;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace GotGLib
{
    /*
    public class LifeSpanHandler : ILifeSpanHandler
    {
        private ILog log;
        private JScriptInterface JSInt;
        DispatcherTimer m_Timer;

        public LifeSpanHandler()
        {
            log = log4net.LogManager.GetLogger(GetType());

            JSInt = new JScriptInterface(); //TODO Do bani bo i tak są zależności unity...
        }

        public bool DoClose(IWebBrowser browserControl, IBrowser browser)
        {
            return false;
        }

        public void OnAfterCreated(IWebBrowser browserControl, IBrowser browser)
        {
            if (first)
            {
                first = false;
                return; //TODO tandeta do testów
            }

            log.Debug("OnAfterCreated");
            browserControl.FrameLoadEnd += BrowserControl_FrameLoadEnd;
            m_BrowserControl = browserControl;
            m_Timer = new DispatcherTimer();
            m_Timer.Interval = new TimeSpan(0, 0, 15);
            m_Timer.Tick += M_Timer_Tick;
            m_Timer.IsEnabled = true;
        }

        async private void M_Timer_Tick(object sender, EventArgs e)
        {
            m_Timer.IsEnabled = false;

            var resp = await m_Frame.EvaluateScriptAsync(JSRes.AJAX);

            if (!resp.Success)
            {
                log.Error(resp.Message);
            }
            else
            {
                m_Frame.ExecuteJavaScriptAsync("InitAjaxHandlers()");
            }
        }

        public void OnBeforeClose(IWebBrowser browserControl, IBrowser browser)
        {
        }

        public bool OnBeforePopup(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IWindowInfo windowInfo, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
        {

            m_Frame = frame;
            log.Debug("OnBeforePopup");
            newBrowser = null;
            return false;
        }

        IFrame m_Frame;
        IWebBrowser m_BrowserControl;
        bool first = true;

        private void BrowserControl_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {

        }
    }
    */
}
