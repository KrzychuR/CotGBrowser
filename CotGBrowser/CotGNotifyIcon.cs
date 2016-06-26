using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CotGBrowser
{
    public class CotGNotifyIcon
    {
        private System.Windows.Forms.NotifyIcon notifyIcon = null;

        public CotGNotifyIcon()
        {
            notifyIcon = new System.Windows.Forms.NotifyIcon();
            notifyIcon.Icon = Images.ImagesRes.app_icon;
            notifyIcon.Visible = true;
        }

        public void AppStarted()
        {
            notifyIcon.ShowBalloonTip(3000, "CotGBrowser", "Started....", System.Windows.Forms.ToolTipIcon.Info);
        }

        public void Info(string msg)
        {
            notifyIcon.ShowBalloonTip(3000, "CotGBrowser", msg, System.Windows.Forms.ToolTipIcon.Info);
        }

        public void Warning(string msg)
        {
            notifyIcon.ShowBalloonTip(3000, "CotGBrowser", msg, System.Windows.Forms.ToolTipIcon.Warning);
        }

        public void Alarm(string msg)
        {
            notifyIcon.ShowBalloonTip(3000, "CotGBrowser", msg, System.Windows.Forms.ToolTipIcon.Error);
        }
    }
}
