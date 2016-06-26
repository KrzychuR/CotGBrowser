using CotGBrowser.Common;
using GotGLib;
using GotGLib.NH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Practices.Unity;
using GotGLib.DTO;
using System.Collections.ObjectModel;
using OxyPlot;
using GotGLib.DB;

namespace CotGBrowser.Views
{
    public class RankingsWindowMV: BaseModelView
    {
        public RankingsWindowMV()
        {
            RefreshRankingsCmd = new SimpleCommand(this, (p) => DoRefreshRankingsCmd(), (p) => !IsBusy);
            RefreshPlotCmd = new SimpleCommand(this, (p) => DoRefreshPlotCmd(), (p) => !IsBusy);
            RefreshAlianceRankingsCmd = new SimpleCommand(this, (p) => DoRefreshAlianceRankingsCmd(), (p) => !IsBusy);
            RefreshAliancePlotCmd = new SimpleCommand(this, (p) => DoRefreshAliancePlotCmd(), (p) => !IsBusy);

            EmpireRankings = new ObservableCollection<CurrentEmpireRanking>();
            EmpireHistRanking = new ObservableCollection<EmpireScoreHistory>();
            AlianceRankingHistory = new ObservableCollection<AlianceScoreHistory>();
            AlianceRankings = new ObservableCollection<CurrentAlianceRanking>();

            if (IoCHelper.IsInitialized)
            {
                Db = IoCHelper.GetIoC().Resolve<Database>();
            }
        }

        #region Polecenia

        public ICommand RefreshRankingsCmd { get; set; }
        public ICommand RefreshAlianceRankingsCmd { get; set; }
        public ICommand RefreshAliancePlotCmd { get; set; }
        public ICommand RefreshPlotCmd { get; set; }

        #endregion

        #region Cechy

        private CurrentAlianceRanking m_SelectedAlianceRanking;

        /// <summary>
        /// Aktualnie wybrany ranking sojuszu
        /// </summary>
        public CurrentAlianceRanking SelectedAlianceRanking
        {
            get { return m_SelectedAlianceRanking; }
            set
            {
                if (SetProperty(ref m_SelectedAlianceRanking, value))
                {
                    DoRefreshAlianceHistory();
                }
            }
        }

        private ObservableCollection<CurrentAlianceRanking> m_AlianceRankings;

        /// <summary>
        /// Wszystkie (odfiltrowane) rankingi sojuszu
        /// </summary>
        public ObservableCollection<CurrentAlianceRanking> AlianceRankings
        {
            get { return m_AlianceRankings; }
            set { m_AlianceRankings = value; }
        }

        private List<CurrentAlianceRanking> m_AlianceRankingsPlotData;

        /// <summary>
        /// Wszystkie (odfiltrowane) rankingi sojuszu
        /// </summary>
        public List<CurrentAlianceRanking> AlianceRankingsPlotData
        {
            get { return m_AlianceRankingsPlotData; }
            set { SetProperty(ref m_AlianceRankingsPlotData, value); }
        }


        private ObservableCollection<AlianceScoreHistory> m_AlianceRankingHistory;

        /// <summary>
        /// Historia wybranego sojuszu
        /// </summary>
        public ObservableCollection<AlianceScoreHistory> AlianceRankingHistory
        {
            get { return m_AlianceRankingHistory; }
            set { SetProperty(ref m_AlianceRankingHistory, value); }
        }


        private PlotModel m_PlotM;

        public PlotModel PlotM
        {
            get { return m_PlotM; }
            set { SetProperty(ref m_PlotM, value); }
        }

        private PlotModel m_AliancePlotM;

        public PlotModel AliancePlotM
        {
            get { return m_AliancePlotM; }
            set { SetProperty(ref m_AliancePlotM, value); }
        }

        private string m_AlianceFilterText;

        /// <summary>
        /// Tekst szukany w sojuszach
        /// </summary>
        public string AlianceFilterText
        {
            get { return m_AlianceFilterText; }
            set
            {
                if (SetProperty(ref m_AlianceFilterText, value))
                {
                    DoAlianceFilter();
                }
            }
        }


        private ObservableCollection<CurrentEmpireRanking> m_EmpireRankings;

        /// <summary>
        /// Rankingi graczy
        /// </summary>
        public ObservableCollection<CurrentEmpireRanking> EmpireRankings
        {
            get { return m_EmpireRankings; }
            set { SetProperty(ref m_EmpireRankings, value); }
        }

        private ObservableCollection<EmpireScoreHistory> m_EmpireHistRanking;

        /// <summary>
        /// Historia rankingu gracza na kontynencie
        /// </summary>
        public ObservableCollection<EmpireScoreHistory> EmpireHistRanking
        {
            get { return m_EmpireHistRanking; }
            set { SetProperty(ref m_EmpireHistRanking, value); }
        }

        private CurrentEmpireRanking m_SelectedRanking;

        /// <summary>
        /// Wybrany ranking gracza
        /// </summary>
        public CurrentEmpireRanking SelectedRanking
        {
            get { return m_SelectedRanking; }
            set
            {
                if (SetProperty(ref m_SelectedRanking, value))
                {
                    DoRefreshHistory();
                }
            }
        }

        private string m_FilterText;

        /// <summary>
        /// Szukany tekst w rankingach gracza
        /// </summary>
        public string FilterText
        {
            get { return m_FilterText; }
            set
            {
                if (SetProperty(ref m_FilterText, value))
                {
                    DoFilter();
                }
            }
        }

        private int m_EmpireContinentFilter;

        public int EmpireContinentFilter
        {
            get { return m_EmpireContinentFilter; }
            set { SetProperty(ref m_EmpireContinentFilter, value); }
        }

        private int m_AlianceContinentFilter;

        public int AlianceContinentFilter
        {
            get { return m_AlianceContinentFilter; }
            set { SetProperty(ref m_AlianceContinentFilter, value); }
        }

        private Dictionary<CurrentEmpireRanking, List<OffReputationHistory>> m_EmpireOffRepHistPlotData;

        public Dictionary<CurrentEmpireRanking, List<OffReputationHistory>> EmpireOffRepHistPlotData
        {
            get { return m_EmpireOffRepHistPlotData; }
            set { SetProperty(ref m_EmpireOffRepHistPlotData, value); }
        }

        private Dictionary<CurrentEmpireRanking, List<DefReputationHistory>> m_EmpireDefRepHistPlotData;

        public Dictionary<CurrentEmpireRanking, List<DefReputationHistory>> EmpireDefRepHistPlotData
        {
            get { return m_EmpireDefRepHistPlotData; }
            set { SetProperty(ref m_EmpireDefRepHistPlotData, value); }
        }

        private Dictionary<CurrentEmpireRanking, List<UnitsKillsHistory>> m_EmpireUnitKillsPlotData;

        public Dictionary<CurrentEmpireRanking, List<UnitsKillsHistory>> EmpireUnitKillsPlotData
        {
            get { return m_EmpireUnitKillsPlotData; }
            set { SetProperty(ref m_EmpireUnitKillsPlotData, value); }
        }

        private Dictionary<CurrentEmpireRanking, List<EmpireScoreHistory>> m_EmpirePlotData;

        public Dictionary<CurrentEmpireRanking, List<EmpireScoreHistory>> EmpirePlotData
        {
            get { return m_EmpirePlotData; }
            set
            {
                SetProperty(ref m_EmpirePlotData, value);
            }
        }

        private Dictionary<CurrentAlianceRanking, List<AlianceScoreHistory>> m_AliancePlotData;

        public Dictionary<CurrentAlianceRanking, List<AlianceScoreHistory>> AliancePlotData
        {
            get { return m_AliancePlotData; }
            set
            {
                SetProperty(ref m_AliancePlotData, value);
            }
        }

        #endregion

        private Database Db { get; set; }

        private List<EmpireScoreHistory> GetPlayerHistory(string player, int continent)
        {
            var get = IoCHelper.GetIoC().Resolve<GetRankingHistory>();
            get.PlayerName = player;
            get.Continent = continent;
            Db.ExecuteWork(get);
            return get.Result;
        }

        private List<UnitsKillsHistory> GetPlayerKillsHistory(string player)
        {
            var get = IoCHelper.GetIoC().Resolve<GetKillsHistory>();
            get.PlayerName = player;
            Db.ExecuteWork(get);
            return get.Result;
        }

        private List<DefReputationHistory> GetDefRepHistory(string player)
        {
            var get = IoCHelper.GetIoC().Resolve<GetDefRepHistory>();
            get.PlayerName = player;
            Db.ExecuteWork(get);
            return get.Result;
        }

        private List<OffReputationHistory> GetOffRepHistory(string player)
        {
            var get = IoCHelper.GetIoC().Resolve<GetOffRepHistory>();
            get.PlayerName = player;
            Db.ExecuteWork(get);
            return get.Result;
        }

        async private void DoRefreshPlotCmd()
        {
            IsBusy = true;

            try
            {
                //lista zaznaczonych rankingów do obejrzenia historii
                var playersHistory = new Dictionary<CurrentEmpireRanking, List<EmpireScoreHistory>>();
                var killsHistory = new Dictionary<CurrentEmpireRanking, List<UnitsKillsHistory>>();
                var defHistory = new Dictionary<CurrentEmpireRanking, List<DefReputationHistory>>();
                var offHistory = new Dictionary<CurrentEmpireRanking, List<OffReputationHistory>>();

                await Task.Run(() =>
                {
                    foreach (var rank in EmpireRankings.Where(x => x.UISelected))
                    {
                        playersHistory.Add(rank, GetPlayerHistory(rank.PlayerName, rank.Continent));
                        killsHistory.Add(rank, GetPlayerKillsHistory(rank.PlayerName));
                        defHistory.Add(rank, GetDefRepHistory(rank.PlayerName));
                        offHistory.Add(rank, GetOffRepHistory(rank.PlayerName));
                    }
                });

                EmpirePlotData = playersHistory;
                EmpireUnitKillsPlotData = killsHistory;
                EmpireDefRepHistPlotData = defHistory;
                EmpireOffRepHistPlotData = offHistory;
            }
            finally
            {
                IsBusy = false;
            }
        }

        private List<CurrentAlianceRanking> m_AlianceRankingsCache;

        async private void DoRefreshAlianceRankingsCmd()
        {
            var get = IoCHelper.GetIoC().Resolve<GetAlianceRankings>();
            get.Continent = AlianceContinentFilter;

            IsBusy = true;

            try
            {
                await Task.Run(() => Db.ExecuteWork(get));

                if (get.Result != null)
                {
                    m_AlianceRankingsCache = get.Result;
                    DoAlianceFilter();
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void DoAlianceFilter()
        {
            AlianceRankings.Clear();

            if (m_AlianceRankingsCache != null)
            {
                foreach (var r in m_AlianceRankingsCache)
                {
                    if (r.ContainsSearchExp(AlianceFilterText))
                        AlianceRankings.Add(r);
                }
            }
        }

        async private void DoRefreshAlianceHistory()
        {
            if (SelectedAlianceRanking != null)
            {
                var get = IoCHelper.GetIoC().Resolve<GetAlianceRankingHistory>();

                IsBusy = true;

                try
                {
                    get.AlianceName = SelectedAlianceRanking.AlianceName;
                    get.Continent = SelectedAlianceRanking.Continent;

                    await Task.Run(() => Db.ExecuteWork(get));

                    if (get.Result != null)
                    {
                        AlianceRankingHistory.Clear();

                        foreach (var r in get.Result)
                        {
                            AlianceRankingHistory.Add(r);
                        }
                    }
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        private List<AlianceScoreHistory> GetAlianceHistory(string aliance, int continent)
        {
            var get = IoCHelper.GetIoC().Resolve<GetAlianceRankingHistory>();
            get.AlianceName = aliance;
            get.Continent = continent;
            Db.ExecuteWork(get);
            return get.Result;
        }

        async private void DoRefreshAliancePlotCmd()
        {
            IsBusy = true;

            try
            {
                //lista zaznaczonych rankingów sojuszy do obejrzenia historii
                var aliancesHistory = new Dictionary<CurrentAlianceRanking, List<AlianceScoreHistory>>();

                await Task.Run(() =>
                {
                    foreach (var rank in AlianceRankings.Where(x => x.UISelected))
                    {
                        aliancesHistory.Add(rank, GetAlianceHistory(rank.AlianceName, rank.Continent));
                    }
                });

                AliancePlotData = aliancesHistory;

                //wybieram te sojusze które są zaznaczone na chociaż jednym kontynencie
                var selectedAlianceNames = AlianceRankings.Where(x => x.UISelected).GroupBy(x => x.AlianceName);

                var aliances2Plot = new List<CurrentAlianceRanking>();

                foreach (var alianceName in selectedAlianceNames)
                {
                    aliances2Plot.AddRange(m_AlianceRankingsCache.Where(x => x.AlianceName == alianceName.Key));
                }

                AlianceRankingsPlotData = aliances2Plot;
            }
            finally
            {
                IsBusy = false;
            }
        }


        async private void DoRefreshRankingsCmd()
        {
            var get = IoCHelper.GetIoC().Resolve<GetEmpireRanking>();
            get.Continent = EmpireContinentFilter;

            IsBusy = true;

            try
            {
                await Task.Run(() => Db.ExecuteWork(get));

                if (get.Result != null)
                {
                    m_RankingsCache = get.Result;
                    DoFilter();
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void DoFilter()
        {
            EmpireRankings.Clear();

            if (m_RankingsCache != null)
            {
                foreach (var r in m_RankingsCache)
                {
                    if (r.ContainsSearchExp(FilterText))
                        EmpireRankings.Add(r);
                }
            }
        }

        private List<CurrentEmpireRanking> m_RankingsCache;

        async private void DoRefreshHistory()
        {
            if (SelectedRanking != null)
            {
                var get = IoCHelper.GetIoC().Resolve<GetRankingHistory>();

                IsBusy = true;

                try
                {
                    get.PlayerName = SelectedRanking.PlayerName;
                    get.Continent = SelectedRanking.Continent;

                    await Task.Run(() => Db.ExecuteWork(get));

                    if (get.Result != null)
                    {
                        EmpireHistRanking.Clear();

                        foreach (var r in get.Result)
                        {
                            EmpireHistRanking.Add(r);
                        }
                    }
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }
    }
}
