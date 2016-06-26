using GotGLib.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CotGBrowser.Views
{
    /// <summary>
    /// Informacje o jednostce, klasa wykorzystywana przez kalkulator optymalnej ilości jednostek
    /// </summary>
    public class RaidCalcTroopData: BaseDTO
    {
        private string m_TroopName;

        public string TroopName
        {
            get { return m_TroopName; }
            set { SetProperty(ref m_TroopName, value); }
        }

        private int m_LootCapacity;

        public int LootCapacity
        {
            get { return m_LootCapacity; }
            set { SetProperty(ref m_LootCapacity, value); }
        }

        private double m_Weight;

        public double Weight
        {
            get { return m_Weight; }
            set { SetProperty(ref m_Weight, value); }
        }

        private int m_OptimalQty;

        public int OptimalQty
        {
            get { return m_OptimalQty; }
            set { SetProperty( ref m_OptimalQty, value); }
        }

        private string m_TropType;

        public string TropType
        {
            get { return m_TropType; }
            set
            {
                SetProperty(ref m_TropType, value);
            }
        }

    }
}
