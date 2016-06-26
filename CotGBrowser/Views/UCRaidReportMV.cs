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

namespace CotGBrowser.Views
{
    public class UCRaidReportMV: BaseModelView
    {
        public UCRaidReportMV()
        {
            if (IoCHelper.IsInitialized)
            {
                JSInterface = IoCHelper.GetIoC().Resolve<JScriptInterface>();
                JSInterface.CurrentRaidReport += JSInterface_CurrentRaidReport;
            }

            CapacityReserve = 15;
            CalcTroops = new ObservableCollection<RaidCalcTroopData>();

            CalcTroops.Add(new RaidCalcTroopData()
            {
                TroopName = "Vanquisher",
                LootCapacity = 10,
                Weight = 0,
                OptimalQty = 0,
                TropType = "5"
            });

            CalcTroops.Add(new RaidCalcTroopData()
            {
                TroopName = "Triari",
                LootCapacity = 20,
                Weight = 0,
                OptimalQty = 0,
                TropType = "3"
            });

            CalcTroops.Add(new RaidCalcTroopData()
            {
                TroopName = "Ranger",
                LootCapacity = 10,
                Weight = 0,
                OptimalQty = 0,
                TropType = "2"
            });

            CalcTroops.Add(new RaidCalcTroopData()
            {
                TroopName = "Praetor",
                LootCapacity = 20,
                Weight = 0,
                OptimalQty = 0,
                TropType = "9"
            });

            CalcTroops.Add(new RaidCalcTroopData()
            {
                TroopName = "Arbalist",
                LootCapacity = 15,
                Weight = 0,
                OptimalQty = 0,
                TropType = "8"
            });

            CalcTroops.Add(new RaidCalcTroopData()
            {
                TroopName = "Horseman",
                LootCapacity = 15,
                Weight = 0,
                OptimalQty = 0,
                TropType = "10"
            });

            CalcTroops.Add(new RaidCalcTroopData()
            {
                TroopName = "Sorcerer",
                LootCapacity = 5,
                Weight = 0,
                OptimalQty = 0,
                TropType = "6"
            });

            CalcTroops.Add(new RaidCalcTroopData()
            {
                TroopName = "Druid",
                LootCapacity = 10,
                Weight = 0,
                OptimalQty = 0,
                TropType = "11"
            });

            CalcTroops.Add(new RaidCalcTroopData()
            {
                TroopName = "Priestess",
                LootCapacity = 10,
                Weight = 0,
                OptimalQty = 0,
                TropType = "4"
            });

            foreach (var ct in CalcTroops)
                ct.PropertyChanged += (s, e) => CalcOptimalSquad();
        }

        private RaidReport m_SelectedReport;

        public RaidReport SelectedReport
        {
            get { return m_SelectedReport; }
            set
            {
                if (SetProperty(ref m_SelectedReport, value))
                {
                    if(value != null)
                        RefreshReportData();
                }
            }
        }

        #region Kalkulacja jednostek

        private int m_ResToLot;

        /// <summary>
        /// Zasoby do zgrabienia
        /// </summary>
        public int ResToLot
        {
            get { return m_ResToLot; }
            set { SetProperty(ref m_ResToLot, value); }
        }

        private int m_CapacityReserve;

        /// <summary>
        /// Zapas pojemnosci jednostek - domyślnie 15%
        /// </summary>
        public int CapacityReserve
        {
            get { return m_CapacityReserve; }
            set
            {
                if( SetProperty(ref m_CapacityReserve, value))
                {
                    CalcOptimalSquad();
                }
            }
        }

        private int m_TargetSquadCapacity;

        /// <summary>
        /// Docelowa pojemność drużyny
        /// </summary>
        public int TargetSquadCapacity
        {
            get { return m_TargetSquadCapacity; }
            set { SetProperty(ref m_TargetSquadCapacity, value); }
        }

        private ObservableCollection<RaidCalcTroopData> m_CalcTroops;

        /// <summary>
        /// Kalkulacja, dane i wyniki
        /// </summary>
        public ObservableCollection<RaidCalcTroopData> CalcTroops
        {
            get { return m_CalcTroops; }
            set { SetProperty(ref m_CalcTroops, value); }
        }

        #endregion

        protected JScriptInterface JSInterface { get; set; }

        private void RefreshReportData()
        {
            if (SelectedReport != null)
            {
                JSInterface.RefreshRaidReport(SelectedReport.ReportId);
            }
        }

        private void JSInterface_CurrentRaidReport(object sender, RaidReport e)
        {
            //uaktualniam dane które już mam, scalam
            if(SelectedReport != null && SelectedReport.ReportId == e.ReportId)
            {
                e.CavernType = SelectedReport.CavernType;
                e.AttackCity = SelectedReport.AttackCity;
                e.CarryCapacity = SelectedReport.CarryCapacity;
                e.ReportDT = SelectedReport.ReportDT;
                e.CavernCoords = SelectedReport.CavernCoords;
                e.Progress = SelectedReport.Progress;

                //resztę przekopiuję z automatu
                AutoMapper.Mapper.Map<RaidReport, RaidReport>(e, SelectedReport);

                //domyślnie ustawiam wagi dla jednostek biorących udział w ataku

                foreach (var ct in CalcTroops)
                    ct.Weight = 0;

                foreach(var attackT in SelectedReport.AttackTrops)
                {
                    var ct = CalcTroops.FirstOrDefault(x => x.TropType == attackT.TropType);

                    if (ct != null)
                        ct.Weight = 1;
                }

                CalcOptimalSquad();
            }
        }

        private void CalcOptimalSquad()
        {
            ResToLot = 0;

            if (SelectedReport == null)
                return;

            if (SelectedReport.RtRes == null)
                return;

            ResToLot = SelectedReport.RtRes.TotalWithoutGold;
            TargetSquadCapacity =  (int)Math.Round(ResToLot * (1 + ((double)CapacityReserve / 100.0)));

            
            //obliczam jednostkową pojemność wyszystkich jednostek, które mają wagę > 0
            double baseCapacity = 0;

            foreach (var u in CalcTroops)
                baseCapacity += u.Weight * u.LootCapacity;

            if (baseCapacity == 0)
                return; //nie wybrano jednostek

            //wyliczam współczynnik (udział) jednostki w docelowym łupie oraz optymalną ilość
            foreach (var u in CalcTroops)
            {
                double capacityFactor = u.LootCapacity / baseCapacity;
                u.OptimalQty = (int)Math.Round( (u.Weight * TargetSquadCapacity * capacityFactor) / u.LootCapacity );
            }
        }
    }
}
