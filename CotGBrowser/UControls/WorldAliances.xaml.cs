using GotGLib.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for WorldAliances.xaml
    /// </summary>
    public partial class WorldAliances : UserControl
    {
        public WorldAliances()
        {
            InitializeComponent();
            this.grid.DataContext = new WorldAliancesMV();
        }

        private WorldAliancesMV ModelView { get { return this.grid.DataContext as WorldAliancesMV; } }

        public List<CurrentAlianceRanking> Aliances
        {
            get { return (List<CurrentAlianceRanking>)GetValue(AliancesProperty); }
            set { SetValue(AliancesProperty, value); }
        }

        public static readonly DependencyProperty AliancesProperty =
            DependencyProperty.Register("Aliances", typeof(List<CurrentAlianceRanking>),
                typeof(WorldAliances), new FrameworkPropertyMetadata(DPAliances) { BindsTwoWayByDefault = true });

        private static void DPAliances(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var uc = d as WorldAliances;
            var val = e.NewValue as List<CurrentAlianceRanking>;

            if (uc != null && uc.ModelView != null)
            {
                uc.ModelView.AlianceRankings = val;
            }
        }
    }
}
