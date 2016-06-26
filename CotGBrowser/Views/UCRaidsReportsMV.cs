using CotGBrowser.Common;
using GotGLib;
using GotGLib.DTO;
using GotGLib.JS;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace CotGBrowser.Views
{
    public class UCRaidsReportsMV : BaseModelView
    {
        public UCRaidsReportsMV()
        {
            m_InsufficientCapacity = false;
            m_LastNReports = 15;
            m_NewOnly = false;
            Cities = new ObservableCollection<string>();

            if(IoCHelper.IsInitialized)
            {
                RefreshCmd = new SimpleCommand(this, (p) => DoRefreshCmd(), (p) => !IsBusy);
                JSInterface = IoCHelper.GetIoC().Resolve<JScriptInterface>();
                JSInterface.CurrentRaidReports += JSInterface_CurrentRaidReports;
            }

            DataView = (CollectionView)CollectionViewSource.GetDefaultView(Reports);
        }

        private CollectionView m_DataView;

        public CollectionView DataView
        {
            get { return m_DataView; }
            set { SetProperty(ref m_DataView, value); }
        }

        private ObservableCollection<RaidReport> m_Reports = new ObservableCollection<RaidReport>();

        public ObservableCollection<RaidReport> Reports
        {
            get { return m_Reports; }
            set { SetProperty(ref m_Reports, value); }
        }

        public ObservableCollection<string> Cities { get; set; }

        private RaidReport m_SelectedReport;

        public RaidReport SelectedReport
        {
            get { return m_SelectedReport; }
            set { SetProperty(ref m_SelectedReport, value); }
        }

        private bool? m_InsufficientCapacity;

        public bool? InsufficientCapacity
        {
            get { return m_InsufficientCapacity; }
            set
            {
                if (SetProperty(ref m_InsufficientCapacity, value))
                    Filter();
            }
        }

        private bool? m_NewOnly;

        public bool? NewOnly
        {
            get { return m_NewOnly; }
            set
            {
                if (SetProperty(ref m_NewOnly, value))
                    Filter();
            }
        }

        private int m_LastNReports;

        public int LastNReports
        {
            get { return m_LastNReports; }
            set
            {
                if (SetProperty(ref m_LastNReports, value))
                    Filter();
            }
        }

        private string m_SearchText;

        public string SearchText
        {
            get { return m_SearchText; }
            set
            {
                if (SetProperty(ref m_SearchText, value))
                    Filter();
            }
        }

        private string m_SelectedCity;

        public string SelectedCity
        {
            get { return m_SelectedCity; }
            set
            {
                if (SetProperty(ref m_SelectedCity, value))
                    Filter();
            }
        }


        #region Polecenia ---------------------------------

        public ICommand RefreshCmd { get; set; }

        private void DoRefreshCmd()
        {
            JSInterface.RefreshRaidsReports();
        }

        #endregion

        private List<RaidReport> m_Cache = new List<RaidReport>();

        protected JScriptInterface JSInterface { get; set; }

        private void JSInterface_CurrentRaidReports(object sender, List<RaidReport> e)
        {
            m_Cache = e;

            Filter();
        }

        const string ALL_CITIES = "<ALL>";

        private void Filter()
        {
            if (IsBusy) return;

            IsBusy = true;

            var selCity = SelectedCity == ALL_CITIES ? "" : SelectedCity;

            SelectedReport = null;
            Reports.Clear();
            bool repVisible = true;
            bool cityVisible = true;

            Cities.Clear();
            Cities.Add(ALL_CITIES);

            foreach (var city in m_Cache.GroupBy(x => x.AttackCity))
            {
                var commonCityData = city.First();
                var lastReports = city.OrderByDescending(x => x.ReportDT).Take(LastNReports);

                cityVisible = commonCityData.ContainsSearchExp(SearchText);

                if(cityVisible && (NewOnly ?? false))
                {
                    //czy miasto ma jakiś nowy raport?
                    cityVisible = lastReports.Any(x => x.IsNew);
                }

                if(cityVisible && (InsufficientCapacity ?? false))
                {
                    //czy w pierwszych najświeższych (nowych) 15 - jest za mała pojemność?
                    cityVisible = lastReports.Where(x => x.IsNew).Any(x => x.CarryCapacity < 100);
                }

                if (cityVisible)
                {
                    if (string.IsNullOrWhiteSpace(selCity) ? true : city.Key == selCity)
                    {
                        foreach (var r in lastReports)
                            Reports.Add(r);
                    }

                    Cities.Add(city.Key);
                }


//                    && (DateTime.Now - r.ReportDT).TotalDays <= LastNDays
  ///                  && ((NewOnly ?? false) ? r.IsNew : true)
                    ;

                /*


                if(InsufficientCapacity ?? false)
                {
                    //czy najświeższy raport ma za małą pojemność ? jak tak... to pokazuje to miasto w całości
                }
                   // && ( ? r.CarryCapacity < 100 : true)


                repVisible =
                    cityVisible
                    && ( string.IsNullOrWhiteSpace(selCity) ? true : r.AttackCity == selCity )
                    ;

                if(repVisible)
                    Reports.Add(r);

                if (cityVisible && !Cities.Contains(r.AttackCity))
                    Cities.Add(r.AttackCity);
                    */
            }

            SelectedCity = selCity;
            IsBusy = false;
        }
    }
}
