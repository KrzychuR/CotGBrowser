using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GotGLib.DTO
{
    public class ReportTrops: BaseDTO
    {
        public ReportTrops()
        {

        }

        public ReportTrops(JObject token)
        {
            foreach (var p in token)
            {
                switch (p.Key)
                {
                    case "tt": this.TropType= p.Value.ToString(); break;
                    case "sa": this.Sent = (int)p.Value; break;
                    case "la": this.Lost = (int)p.Value; break;
                    case "ea": this.LiveTrops = (int)p.Value; break;
                    case "rb": this.tb = (int)p.Value; break;

                    default:
                        break;
                }
            }
        }

        private string m_TropType;

        public string TropType
        {
            get { return m_TropType; }
            set
            {
                if (SetProperty(ref m_TropType, value))
                {
                    if(!string.IsNullOrWhiteSpace(value))
                    {
                        TropTypeDesc = GetTroopName(value);
                    }
                }
            }
        }

        private string m_TropTypeDesc;

        public string TropTypeDesc
        {
            get { return m_TropTypeDesc; }
            set { SetProperty(ref m_TropTypeDesc, value); }
        }

        private int m_Sent;

        public int Sent
        {
            get { return m_Sent; }
            set
            {
                if (m_Sent != value)
                {
                    m_Sent = value;
                    DoPropertyChanged();
                }
            }
        }


        private int m_Lost;

        public int Lost
        {
            get { return m_Lost; }
            set
            {
                if (m_Lost != value)
                {
                    m_Lost = value;
                    DoPropertyChanged();
                }
            }
        }


        private int m_LiveTrops;

        public int LiveTrops
        {
            get { return m_LiveTrops; }
            set
            {
                if (m_LiveTrops != value)
                {
                    m_LiveTrops = value;
                    DoPropertyChanged();
                }
            }
        }

        private int m_tb;

        public int tb
        {
            get { return m_tb; }
            set
            {
                if (m_tb != value)
                {
                    m_tb = value;
                    DoPropertyChanged();
                }
            }
        }

        public static string GetTroopName(string tropId)
        {
            switch (tropId)
            {
                case "0": return "Guard";
                case "5": return "Vanquisher";
                case "2": return "Ranger";
                case "3" : return "Triari";
                case "7" : return "Scout";
                case "8": return "Arbalist";
                case "10": return "Horsemen";
                case "6": return "Sorcerer";
                case "11": return "Druid";
                case "4": return "Priestess";
                case "9": return "Preator";
                case "17": return "Senator";
                case "12": return "Battering ram";
                case "1": return "Balista";
                case "13": return "Scorpion";
                case "15": return "Stinger";
                case "14": return "Galley";
                case "16": return "Warship";

                default:
                    return tropId;
            }
        }
    }
}
