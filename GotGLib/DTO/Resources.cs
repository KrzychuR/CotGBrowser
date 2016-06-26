using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GotGLib.DTO
{
    public class Resources: BaseDTO
    {
        public Resources()
        {

        }

        public Resources(JObject token)
        {
            foreach(var p in token)
            {
                switch (p.Key)
                {
                    case "w": this.Wood = (int)p.Value; break;
                    case "s": this.Stone = (int)p.Value; break;
                    case "i": this.Iron = (int)p.Value; break;
                    case "f": this.Food = (int)p.Value; break;
                    case "g": this.Gold = (int)p.Value; break;

                    default:
                        break;
                }
            }
        }

        private int? m_Wood;

        public int? Wood
        {
            get { return m_Wood; }
            set
            {
                if (m_Wood != value)
                {
                    m_Wood = value;
                    DoPropertyChanged();
                }
            }
        }

        private int? m_Stone;

        public int? Stone
        {
            get { return m_Stone; }
            set
            {
                if (m_Stone != value)
                {
                    m_Stone = value;
                    DoPropertyChanged();
                }
            }
        }

        private int? m_Iron;

        public int? Iron
        {
            get { return m_Iron; }
            set
            {
                if (m_Iron != value)
                {
                    m_Iron = value;
                    DoPropertyChanged();
                }
            }
        }

        private int? m_Food;

        public int? Food
        {
            get { return m_Food; }
            set
            {
                if (m_Food != value)
                {
                    m_Food = value;
                    DoPropertyChanged();
                }
            }
        }

        private int m_Total;

        public int Total
        {
            get { return m_Total; }
            set { SetProperty(ref m_Total, value); }
        }

        private int? m_Gold;

        public int? Gold
        {
            get { return m_Gold; }
            set
            {
                if (m_Gold != value)
                {
                    m_Gold = value;
                    DoPropertyChanged();
                }
            }
        }

        private int m_TotalWithoutGold;

        public int TotalWithoutGold
        {
            get { return m_TotalWithoutGold; }
            set { SetProperty(ref m_TotalWithoutGold, value); }
        }


        public void RefreshCalcFields()
        {
            Total =
                (Wood ?? 0)
                +(Stone ?? 0)
                +(Iron ?? 0)
                +(Food ?? 0)
                + (Gold ?? 0)
                ;

            TotalWithoutGold =
                (Wood ?? 0)
                + (Stone ?? 0)
                + (Iron ?? 0)
                + (Food ?? 0)
                ;
        }
    }
}
