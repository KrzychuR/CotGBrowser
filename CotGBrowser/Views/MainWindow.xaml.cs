using Microsoft.Practices.Unity;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OxyPlot.Wpf;
using GotGLib;
using GotGLib.JS;
using CefSharp;
using CefSharp.WinForms;

namespace CotGBrowser.Views
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            AllowsTransparency = false;

            if (IoCHelper.IsInitialized)
            {
                //webBrowser.RequestHandler = new RequestHandler();
                var mv = IoCHelper.GetIoC().Resolve<MainWindowMV>();
                DataContext = mv;

                var jsInt = IoCHelper.GetIoC().Resolve<JScriptInterface>();
                var browser = new ChromiumWebBrowser(mv.Url);
                jsInt.InitBrowser(browser);
                mv.Browser = browser;
                wfHost.Child = browser;
            }
        }
    }
}
