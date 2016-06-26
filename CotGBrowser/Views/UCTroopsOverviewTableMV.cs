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
    public class UCTroopsOverviewTableMV: BaseModelView
    {
        public UCTroopsOverviewTableMV()
        {
            if (IoCHelper.IsInitialized)
            {
                m_CitiesWithoutCommandsOnly = false;
                RefreshCmd = new SimpleCommand(this, (p) => DoRefreshCmd(), (p) => !IsBusy);
                FilterCmd = new SimpleCommand(this, (p) => Filter(), (p) => !IsBusy);
                JSInterface = IoCHelper.GetIoC().Resolve<JScriptInterface>();
                JSInterface.TroopsOverview += JSInterface_TroopsOverview;
                //DataView = (CollectionView)CollectionViewSource.GetDefaultView(OverviewData);
                //DataView.GroupDescriptions.Add(new PropertyGroupDescription("GroupingDesc"));
            }
        }

        private ObservableCollection<CityOverview> m_OverviewData = new ObservableCollection<CityOverview>();

        public ObservableCollection<CityOverview> OverviewData
        {
            get { return m_OverviewData; }
            set { SetProperty(ref m_OverviewData, value); }
        }

        private CollectionView m_DataView;

        public CollectionView DataView
        {
            get { return m_DataView; }
            set { SetProperty(ref m_DataView, value); }
        }


        private string m_FilterTxt;

        public string FilterTxt
        {
            get { return m_FilterTxt; }
            set
            {
                SetProperty(ref m_FilterTxt, value);
            }
        }

        private bool? m_CitiesWithoutCommandsOnly;

        public bool? CitiesWithoutCommandsOnly
        {
            get { return m_CitiesWithoutCommandsOnly; }
            set
            {
                if(SetProperty(ref m_CitiesWithoutCommandsOnly, value))
                    Filter();
            }
        }


        #region Polecenia ---------------------------------

        public ICommand RefreshCmd { get; set; }

        private void DoRefreshCmd()
        {
            JSInterface.RefreshTroopsOverview();
        }

        public ICommand FilterCmd { get; set; }

        private void Filter()
        {
            //var total = new CityOverview();
            OverviewData.Clear();

            foreach (var city in m_Cache.Where(x => x.TotalTroops.TotalTS > 0))
            {
                if(CitiesWithoutCommandsOnly ?? false)
                {
                    if (city.TotalTroops.TotalTS != city.TroopsAtHome.TotalTS)
                        continue; //to pomijam bo coś tam robią :)
                }

                if (city.ContainsSearchExp(FilterTxt))
                {
                    if (string.IsNullOrWhiteSpace(FilterTxt))
                        city.GroupingDesc = "All cities";
                    else
                        city.GroupingDesc = "Row contains: " + FilterTxt;

                    OverviewData.Add(city);
                    //total.TroopsAtHome.Add(city.TroopsAtHome);
                    //total.TotalTroops.Add(city.TotalTroops);
                }
            }

            //total.GroupingDesc = "TOTAL";
            //total.TroopsAtHome.RecalcFields();
            //total.TotalTroops.RecalcFields();
            //OverviewData.Add(total);
        }

        #endregion

        private void JSInterface_TroopsOverview(object sender, List<CityOverview> e)
        {
            m_Cache = new List<CityOverview>(e);

            foreach (var c in e)
            {
                c.TotalTroops.RecalcFields();
                c.TroopsAtHome.RecalcFields();
            }

            Filter();
        }

        protected JScriptInterface JSInterface { get; set; }

        protected List<CityOverview> m_Cache = new List<CityOverview>();
    }
}
