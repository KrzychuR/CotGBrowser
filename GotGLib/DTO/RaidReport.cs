using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GotGLib.DTO
{
    public class RaidReport: BaseDTO
    {
        private bool m_IsNew;

        public bool IsNew
        {
            get { return m_IsNew; }
            set { SetProperty(ref m_IsNew, value); }
        }

        public override bool ContainsSearchExp(string exp)
        {
            if (string.IsNullOrWhiteSpace(exp))
                return true;

            if (CavernType.Contains(exp))
                return true;

            if (AttackCity.Contains(exp))
                return true;

            if (CavernCoords.Contains(exp))
                return true;

            if (Title != null && Title.Contains(exp))
                return true;

            return false;
        }

        private string m_CavernType;

        public string CavernType
        {
            get { return m_CavernType; }
            set
            {
                if (m_CavernType != value)
                {
                    m_CavernType = value;
                    DoPropertyChanged();
                }
            }
        }

        private int m_Level;

        public int Level
        {
            get { return m_Level; }
            set
            {
                if (m_Level != value)
                {
                    m_Level = value;
                    DoPropertyChanged();
                }
            }
        }

        private string m_AttackCity;

        public string AttackCity
        {
            get { return m_AttackCity; }
            set
            {
                if (m_AttackCity != value)
                {
                    m_AttackCity = value;
                    DoPropertyChanged();
                }
            }
        }

        private double m_CarryCapacity;

        public double CarryCapacity
        {
            get { return m_CarryCapacity; }
            set
            {
                if (m_CarryCapacity != value)
                {
                    m_CarryCapacity = value;
                    DoPropertyChanged();
                }
            }
        }

        private DateTime m_ReportDT;

        public DateTime ReportDT
        {
            get { return m_ReportDT; }
            set
            {
                if (m_ReportDT != value)
                {
                    m_ReportDT = value;
                    DoPropertyChanged();
                }
            }
        }

        private string m_CavernCoords;

        public string CavernCoords
        {
            get { return m_CavernCoords; }
            set
            {
                if (m_CavernCoords != value)
                {
                    m_CavernCoords = value;
                    DoPropertyChanged();
                }
            }
        }

        private string m_ReportId;

        public string ReportId
        {
            get { return m_ReportId; }
            set
            {
                if (m_ReportId != value)
                {
                    m_ReportId = value;
                    DoPropertyChanged();
                }
            }
        }

        private double m_Progress;

        public double Progress
        {
            get { return m_Progress; }
            set
            {
                if (m_Progress != value)
                {
                    m_Progress = value;
                    DoPropertyChanged();
                }
            }
        }

        private string m_Title;

        public string Title
        {
            get { return m_Title; }
            set
            {
                if (m_Title != value)
                {
                    m_Title = value;
                    DoPropertyChanged();
                }
            }
        }

        private string m_TargetCont;

        public string TargetCont
        {
            get { return m_TargetCont; }
            set
            {
                if (m_TargetCont != value)
                {
                    m_TargetCont = value;
                    DoPropertyChanged();
                }
            }
        }

        private string m_AttackPlayer;

        public string AttackPlayer
        {
            get { return m_AttackPlayer; }
            set
            {
                if (m_AttackPlayer != value)
                {
                    m_AttackPlayer = value;
                    DoPropertyChanged();
                }
            }
        }

        private string m_AttackCont;

        public string AttackCont
        {
            get { return m_AttackCont; }
            set
            {
                if (m_AttackCont != value)
                {
                    m_AttackCont = value;
                    DoPropertyChanged();
                }
            }
        }

        private string m_AttackCoords;

        public string AttackCoords
        {
            get { return m_AttackCoords; }
            set
            {
                if (m_AttackCoords != value)
                {
                    m_AttackCoords = value;
                    DoPropertyChanged();
                }
            }
        }

        private Resources m_AnalRes;

        public Resources AnalRes
        {
            get { return m_AnalRes; }
            set
            {
                if (m_AnalRes != value)
                {
                    m_AnalRes = value;
                    DoPropertyChanged();
                }
            }
        }

        private int m_Anal_p;

        public int Anal_p
        {
            get { return m_Anal_p; }
            set
            {
                if (m_Anal_p != value)
                {
                    m_Anal_p = value;
                    DoPropertyChanged();
                }
            }
        }

        private int m_Anal_cp;

        public int Anal_cp
        {
            get { return m_Anal_cp; }
            set
            {
                if (m_Anal_cp != value)
                {
                    m_Anal_cp = value;
                    DoPropertyChanged();
                }
            }
        }

        private int m_CarryCapacityPcs;

        public int CarryCapacityPcs
        {
            get { return m_CarryCapacityPcs; }
            set
            {
                if (m_CarryCapacityPcs != value)
                {
                    m_CarryCapacityPcs = value;
                    DoPropertyChanged();
                }
            }
        }

        private Resources m_RtRes;

        public Resources RtRes
        {
            get { return m_RtRes; }
            set
            {
                if (m_RtRes != value)
                {
                    m_RtRes = value;
                    DoPropertyChanged();
                }
            }
        }

        private Resources m_RRes;

        public Resources RRes
        {
            get { return m_RRes; }
            set
            {
                if (m_RRes != value)
                {
                    m_RRes = value;
                    DoPropertyChanged();
                }
            }
        }

        private Resources m_RlRes;

        public Resources RlRes
        {
            get { return m_RlRes; }
            set
            {
                if (m_RlRes != value)
                {
                    m_RlRes = value;
                    DoPropertyChanged();
                }
            }
        }

        private Resources m_NotCaried;

        public Resources NotCaried
        {
            get { return m_NotCaried; }
            set { SetProperty(ref m_NotCaried, value); }
        }

        private Resources m_Profit;

        public Resources Profit
        {
            get { return m_Profit; }
            set { SetProperty(ref m_Profit, value); }
        }

        private double m_TotalProfitPerc;

        public double TotalProfitPerc
        {
            get { return m_TotalProfitPerc; }
            set { SetProperty(ref m_TotalProfitPerc, value); }
        }

        private ObservableCollection<ReportTrops> m_AttackTrops;

        public ObservableCollection<ReportTrops> AttackTrops
        {
            get { return m_AttackTrops; }
            set
            {
                if (m_AttackTrops != value)
                {
                    m_AttackTrops = value;
                    DoPropertyChanged();
                }
            }
        }

        private ObservableCollection<ReportTrops> m_DefTrops;

        public ObservableCollection<ReportTrops> DefTrops
        {
            get { return m_DefTrops; }
            set
            {
                if (m_DefTrops != value)
                {
                    m_DefTrops = value;
                    DoPropertyChanged();
                }
            }
        }

        public void RefreshCalcFields()
        {
            var notCaried = new Resources();

            if (RtRes != null && RRes != null)
            {
                notCaried.Wood = RtRes.Wood - RRes.Wood;
                notCaried.Stone = RtRes.Stone - RRes.Stone;
                notCaried.Iron = RtRes.Iron - RRes.Iron;
                notCaried.Food = RtRes.Food - RRes.Food;
            }

            NotCaried = notCaried;

            var profit = new Resources();

            if (RRes != null && RlRes != null)
            {
                profit.Gold = RRes.Gold - RlRes.Gold;
                profit.Wood = RRes.Wood - RlRes.Wood;
                profit.Stone = RRes.Stone - RlRes.Stone;
                profit.Iron = RRes.Iron - RlRes.Iron;
                profit.Food = RRes.Food - RlRes.Food;
            }

            Profit = profit;

            if (RtRes != null)
                RtRes.RefreshCalcFields();

            if (RRes != null)
                RRes.RefreshCalcFields();

            if (RlRes != null)
                RlRes.RefreshCalcFields();

            NotCaried.RefreshCalcFields();
            Profit.RefreshCalcFields();

            if (RtRes != null && RtRes.Total != 0)
            {
                TotalProfitPerc = (1 - (double)(RtRes.Total - Profit.Total) / RtRes.Total) * 100.0;
            }
        }
    }
}
