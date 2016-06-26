using AutoMapper;
using CefSharp;
using CotGBrowser.Views;
using GotGLib;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using Microsoft.Practices.Unity;

namespace CotGBrowser
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected ILog m_Log;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            log4net.Config.XmlConfigurator.Configure();
            m_Log = LogManager.GetLogger(this.GetType());

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            var tmpFolder = System.IO.Path.GetTempPath() + "CotgBrowser_Cache";

            if (CotGBrowser.Properties.Settings.Default.ClearCache)
            {
                var dirInfo = new System.IO.DirectoryInfo(tmpFolder);

                foreach (System.IO.FileInfo file in dirInfo.GetFiles())
                    file.Delete();

                CotGBrowser.Properties.Settings.Default.ClearCache = false;
                CotGBrowser.Properties.Settings.Default.Save();
            }

            var settings = new CefSettings();
            settings.CachePath = tmpFolder;
            //settings.CefCommandLineArgs.Clear();
            settings.RemoteDebuggingPort = 8088;
            settings.CefCommandLineArgs.Add("enable-crash-reporter", "1");
            settings.CefCommandLineArgs.Add("proxy-auto-detect", "1");

            //single_process 
            //settings.CefCommandLineArgs.Add("single_process", "1");
            //settings.CefCommandLineArgs.Add("renderer-process-limit", "1");

            //WPF opt
            settings.WindowlessRenderingEnabled = true;
            settings.EnableInternalPdfViewerOffScreen();
            //settings.CefCommandLineArgs.Add("disable-gpu", "1");
            settings.CefCommandLineArgs.Add("disable-gpu-compositing", "1");
            //settings.CefCommandLineArgs.Add("enable-begin-frame-scheduling", "1");

            //settings.CefCommandLineArgs.Add("enable-logging", "v=1");

            //settings.CefCommandLineArgs.Add("js-flags", "--max-old-space-size=2000");

            //settings.CefCommandLineArgs.Add("renderer-startup-dialog", "1");
            //settings.CefCommandLineArgs.Add("disable-gpu-vsync", "1");
            //settings.CefCommandLineArgs.Add("disable-canvas-aa", "1");
            //settings.CefCommandLineArgs.Add("disable-gpu-rasterization", "1");
            
            settings.MultiThreadedMessageLoop = true;
            settings.FocusedNodeChangedEnabled = true;
            settings.LogSeverity = LogSeverity.Error;

            Cef.OnContextInitialized = delegate
            {
                var cookieManager = Cef.GetGlobalCookieManager();
                cookieManager.SetStoragePath("cookies", true);

                //Dispose of context when finished - preferable not to keep a reference if possible.
                using (var context = Cef.GetGlobalRequestContext())
                {
                    string errorMessage;
                    //You can set most preferences using a `.` notation rather than having to create a complex set of dictionaries.
                    //The default is true, you can change to false to disable
                    context.SetPreference("webkit.webprefs.plugins_enabled", true, out errorMessage);
                }
            };

            if (!Cef.Initialize(settings, shutdownOnProcessExit: true, performDependencyCheck: false))
            {
                throw new Exception("Unable to Initialize Cef");
            }

            /*
            Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        //Application.Current.Dispatcher.Invoke(new Action(
                        //  () => Cef.DoMessageLoopWork()));
                        Cef.DoMessageLoopWork();
                        System.Threading.Thread.Sleep(100);
                    }
                    catch (Exception eml)
                    {
                        m_Log.Error(eml.Message, eml);
                    }
                }
            });
            */

            //konf. fabryki
            IoCHelper.GetIoC();

            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<GotGLib.NH.Schema.MapperProfile>();
            });
            Mapper.AssertConfigurationIsValid();

            m_Log.Info("Application started.");

            Application.Current.MainWindow = IoCHelper.GetIoC().Resolve<Views.MainWindow>();
            Application.Current.MainWindow.Show();
            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;

            CotGNotifyIcon notify = new CotGNotifyIcon();
            IoCHelper.GetIoC().RegisterInstance<CotGNotifyIcon>(notify, new ContainerControlledLifetimeManager());
            notify.AppStarted();
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception exc = e.ExceptionObject as Exception;
            m_Log.Error("Unhandled exception", exc);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Cef.Shutdown();
            base.OnExit(e);
        }
    }
}
