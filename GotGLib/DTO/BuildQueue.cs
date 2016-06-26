using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GotGLib.DTO
{
    public class BuildQueue: BaseDTO
    {
        public BuildQueue() { }

        public BuildQueue(Newtonsoft.Json.Linq.JArray arr)
        {
            if(arr != null)
            {
                CityId = arr[0].ToString();
            }
        }

        private string m_CityId;

        public string CityId
        {
            get { return m_CityId; }
            set { SetProperty(ref m_CityId, value); }
        }

        private string m_CityName;

        public string CityName
        {
            get { return m_CityName; }
            set { SetProperty(ref m_CityName, value); }
        }

        private string m_CityRemarks;

        public string CityRemarks
        {
            get { return m_CityRemarks; }
            set { SetProperty(ref m_CityRemarks, value); }
        }

        private int m_Score;

        public int Score
        {
            get { return m_Score; }
            set { SetProperty(ref m_Score, value); }
        }

        private int m_Wood;

        public int Wood
        {
            get { return m_Wood; }
            set { SetProperty(ref m_Wood, value); }
        }

        private int m_Stone;

        public int Stone
        {
            get { return m_Stone; }
            set { SetProperty(ref m_Stone, value); }
        }

        public double f3 { get; set; }
        public double f4 { get; set; }
        public double f5 { get; set; }
        public double f6 { get; set; }
        public double f7 { get; set; }
    }
}
