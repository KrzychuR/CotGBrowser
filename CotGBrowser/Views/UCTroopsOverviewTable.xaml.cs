using GotGLib;
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
using Microsoft.Practices.Unity;

namespace CotGBrowser.Views
{
    /// <summary>
    /// Interaction logic for UCTroopsOverview.xaml
    /// </summary>
    public partial class UCTroopsOverviewTable : UserControl
    {
        public UCTroopsOverviewTable()
        {
            InitializeComponent();

            if (IoCHelper.IsInitialized)
            {
                this.grid.DataContext = IoCHelper.GetIoC().Resolve<UCTroopsOverviewTableMV>();
            }
        }

        public UCTroopsOverviewTableMV ModelView { get { return this.grid.DataContext as UCTroopsOverviewTableMV; } }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                ModelView.FilterCmd.Execute(null);
            }
        }
    }
}
