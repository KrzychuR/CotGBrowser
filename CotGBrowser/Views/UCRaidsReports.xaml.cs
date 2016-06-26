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
using GotGLib.DTO;

namespace CotGBrowser.Views
{
    /// <summary>
    /// Interaction logic for UCRaidsReports.xaml
    /// </summary>
    public partial class UCRaidsReports : UserControl
    {
        public UCRaidsReports()
        {
            InitializeComponent();

            if(IoCHelper.IsInitialized)
            {
                this.grid.DataContext = IoCHelper.GetIoC().Resolve<UCRaidsReportsMV>();
                ModelView.PropertyChanged += ModelView_PropertyChanged;
            }
        }

        private void ModelView_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == BaseDTO.PropName(() => ModelView.SelectedReport))
            {
                this.SelectedReport = ModelView.SelectedReport;
            }
        }

        public UCRaidsReportsMV ModelView { get { return this.grid.DataContext as UCRaidsReportsMV; } }

        #region SelectedReport

        public RaidReport SelectedReport
        {
            get { return (RaidReport)GetValue(SelectedReportProperty); }
            set { SetValue(SelectedReportProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedReport.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedReportProperty =
            DependencyProperty.Register("SelectedReport", typeof(RaidReport), typeof(UCRaidsReports),
                new FrameworkPropertyMetadata(DPSelectedReportChanged) { BindsTwoWayByDefault = true });

        private static void DPSelectedReportChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var uc = d as UCRaidsReports;
            var val = e.NewValue as RaidReport;

            if (uc != null && uc.ModelView != null)
            {
                uc.ModelView.SelectedReport = val;
            }
        }

        #endregion
    }
}
