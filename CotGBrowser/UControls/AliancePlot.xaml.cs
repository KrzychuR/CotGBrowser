using GotGLib.DTO;
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

namespace CotGBrowser.UControls
{
    /// <summary>
    /// Interaction logic for AliancePlot.xaml
    /// </summary>
    public partial class AliancePlot : UserControl
    {
        public AliancePlot()
        {
            InitializeComponent();
            this.grid.DataContext = new AliancePlotMV();
        }

        private AliancePlotMV ModelView { get { return this.grid.DataContext as AliancePlotMV; } }

        public Dictionary<CurrentAlianceRanking, List<AlianceScoreHistory>> Aliances
        {
            get { return (Dictionary<CurrentAlianceRanking, List<AlianceScoreHistory>>)GetValue(AliancesProperty); }
            set { SetValue(AliancesProperty, value); }
        }

        public static readonly DependencyProperty AliancesProperty =
            DependencyProperty.Register("Aliances", typeof(Dictionary<CurrentAlianceRanking, List<AlianceScoreHistory>>),
                typeof(AliancePlot), new FrameworkPropertyMetadata(DPAliances) { BindsTwoWayByDefault = true });

        private static void DPAliances(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var uc = d as AliancePlot;
            var val = e.NewValue as Dictionary<CurrentAlianceRanking, List<AlianceScoreHistory>>;

            if (uc != null && uc.ModelView != null)
            {
                uc.ModelView.Aliances = val;
            }
        }
    }
}
