using GotGLib;
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
using System.Windows.Shapes;

namespace CotGBrowser.Views
{
    /// <summary>
    /// Interaction logic for DataColoectWindow.xaml
    /// </summary>
    public partial class DataColectWindow : MetroWindow
    {
        public DataColectWindow()
        {
            InitializeComponent();

            if (IoCHelper.IsInitialized)
            {
                this.DataContext = new DataColectWindowMV();
            }
        }

        public DataColectWindowMV ModelView { get { return this.DataContext as DataColectWindowMV; } }
    }
}
