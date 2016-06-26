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
    /// Interaction logic for EmpirePlot.xaml
    /// </summary>
    public partial class EmpirePlot : UserControl
    {
        public EmpirePlot()
        {
            InitializeComponent();
            this.grid.DataContext = new EmpirePlotMV();
        }

        private EmpirePlotMV ModelView { get { return this.grid.DataContext as EmpirePlotMV; } }

        #region Empires

        public Dictionary<CurrentEmpireRanking, List<EmpireScoreHistory>> Empires
        {
            get { return (Dictionary<CurrentEmpireRanking, List<EmpireScoreHistory>>)GetValue(EmpiresProperty); }
            set { SetValue(EmpiresProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Empires.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EmpiresProperty =
            DependencyProperty.Register("Empires", typeof(Dictionary<CurrentEmpireRanking, List<EmpireScoreHistory>>), 
                typeof(EmpirePlot), new FrameworkPropertyMetadata(DPEmpires) { BindsTwoWayByDefault = true });

        private static void DPEmpires(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var uc = d as EmpirePlot;
            var val = e.NewValue as Dictionary<CurrentEmpireRanking, List<EmpireScoreHistory>>;

            if (uc != null && uc.ModelView != null)
            {
                uc.ModelView.Empires = val;
            }
        }

        #endregion

        #region EmpireUnitKills

        public Dictionary<CurrentEmpireRanking, List<UnitsKillsHistory>> EmpireUnitKills
        {
            get { return (Dictionary<CurrentEmpireRanking, List<UnitsKillsHistory>>)GetValue(EmpireUnitKillsProperty); }
            set { SetValue(EmpireUnitKillsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EmpireUnitKills.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EmpireUnitKillsProperty =
            DependencyProperty.Register("EmpireUnitKills", typeof(Dictionary<CurrentEmpireRanking, List<UnitsKillsHistory>>), 
                typeof(EmpirePlot), new FrameworkPropertyMetadata(DPEmpireUnitKills) { BindsTwoWayByDefault = true });

        private static void DPEmpireUnitKills(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var uc = d as EmpirePlot;
            var val = e.NewValue as Dictionary<CurrentEmpireRanking, List<UnitsKillsHistory>>;

            if (uc != null && uc.ModelView != null)
            {
                uc.ModelView.EmpiresUnitKills = val;
            }
        }

        #endregion

        #region DefRep

        public Dictionary<CurrentEmpireRanking, List<DefReputationHistory>> DefReputations
        {
            get { return (Dictionary<CurrentEmpireRanking, List<DefReputationHistory>>)GetValue(DefReputationsProperty); }
            set { SetValue(DefReputationsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Empires.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DefReputationsProperty =
            DependencyProperty.Register("DefReputations", typeof(Dictionary<CurrentEmpireRanking, List<DefReputationHistory>>),
                typeof(EmpirePlot), new FrameworkPropertyMetadata(DPDefReputations) { BindsTwoWayByDefault = true });

        private static void DPDefReputations(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var uc = d as EmpirePlot;
            var val = e.NewValue as Dictionary<CurrentEmpireRanking, List<DefReputationHistory>>;

            if (uc != null && uc.ModelView != null)
            {
                uc.ModelView.DefReputations = val;
            }
        }

        #endregion

        #region OffRep

        public Dictionary<CurrentEmpireRanking, List<OffReputationHistory>> OffReputations
        {
            get { return (Dictionary<CurrentEmpireRanking, List<OffReputationHistory>>)GetValue(OffReputationsProperty); }
            set { SetValue(OffReputationsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Empires.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OffReputationsProperty =
            DependencyProperty.Register("OffReputations", typeof(Dictionary<CurrentEmpireRanking, List<OffReputationHistory>>),
                typeof(EmpirePlot), new FrameworkPropertyMetadata(DPOffReputations) { BindsTwoWayByDefault = true });

        private static void DPOffReputations(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var uc = d as EmpirePlot;
            var val = e.NewValue as Dictionary<CurrentEmpireRanking, List<OffReputationHistory>>;

            if (uc != null && uc.ModelView != null)
            {
                uc.ModelView.OffReputations = val;
            }
        }

        #endregion
    }
}
