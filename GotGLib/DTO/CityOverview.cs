using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GotGLib.DTO
{
    public class CityOverview: BaseDTO
    {
        public CityOverview()
        { }

        public CityOverview(JObject token)
        {
            foreach (var p in token)
            {
                switch (p.Key)
                {
                    case "c": this.CityName = p.Value.ToString(); break;
                    case "l": this.Location = p.Value.ToString(); break;
                    case "wall_lvl": this.WallLevel = (int)p.Value; break;
                    case "spot_time": this.SpootTime = p.Value.ToString(); break;

                    case "Guard_home": this.TroopsAtHome.Guard = (int)p.Value; break;
                    case "Ballista_home": this.TroopsAtHome.Balista = (int)p.Value; break;
                    case "Ranger_home": this.TroopsAtHome.Ranger = (int)p.Value; break;
                    case "Triari_home": this.TroopsAtHome.Triari = (int)p.Value; break;
                    case "Priestess_home": this.TroopsAtHome.Priestess = (int)p.Value; break;
                    case "Vanquisher_home": this.TroopsAtHome.Vanquisher = (int)p.Value; break;
                    case "Sorcerer_home": this.TroopsAtHome.Sorcerer = (int)p.Value; break;
                    case "Scout_home": this.TroopsAtHome.Scout = (int)p.Value; break;
                    case "Arbalist_home": this.TroopsAtHome.Arbalist = (int)p.Value; break;
                    case "Praetor_home": this.TroopsAtHome.Praetor= (int)p.Value; break;
                    case "Horseman_home": this.TroopsAtHome.Horseman = (int)p.Value; break;
                    case "Druid_home": this.TroopsAtHome.Druid = (int)p.Value; break;
                    case "Ram_home": this.TroopsAtHome.Ram = (int)p.Value; break;
                    case "Scorpion_home": this.TroopsAtHome.Scorpion = (int)p.Value; break;
                    case "Galley_home": this.TroopsAtHome.Galley = (int)p.Value; break;
                    case "Stinger_home": this.TroopsAtHome.Stinger = (int)p.Value; break;
                    case "Warship_home": this.TroopsAtHome.Warship = (int)p.Value; break;
                    case "Senator_home": this.TroopsAtHome.Senator = (int)p.Value; break;

                    case "Guard_total": this.TotalTroops.Guard = (int)p.Value; break;
                    case "Ballista_total": this.TotalTroops.Balista = (int)p.Value; break;
                    case "Ranger_total": this.TotalTroops.Ranger = (int)p.Value; break;
                    case "Triari_total": this.TotalTroops.Triari = (int)p.Value; break;
                    case "Priestess_total": this.TotalTroops.Priestess = (int)p.Value; break;
                    case "Vanquisher_total": this.TotalTroops.Vanquisher = (int)p.Value; break;
                    case "Sorcerer_total": this.TotalTroops.Sorcerer = (int)p.Value; break;
                    case "Scout_total": this.TotalTroops.Scout = (int)p.Value; break;
                    case "Arbalist_total": this.TotalTroops.Arbalist = (int)p.Value; break;
                    case "Praetor_total": this.TotalTroops.Praetor = (int)p.Value; break;
                    case "Horseman_total": this.TotalTroops.Horseman = (int)p.Value; break;
                    case "Druid_total": this.TotalTroops.Druid = (int)p.Value; break;
                    case "Ram_total": this.TotalTroops.Ram = (int)p.Value; break;
                    case "Scorpion_total": this.TotalTroops.Scorpion = (int)p.Value; break;
                    case "Galley_total": this.TotalTroops.Galley = (int)p.Value; break;
                    case "Stinger_total": this.TotalTroops.Stinger = (int)p.Value; break;
                    case "Warship_total": this.TotalTroops.Warship = (int)p.Value; break;
                    case "Senator_total": this.TotalTroops.Senator = (int)p.Value; break;

                    default:
                        break;
                }
            }
        }

        private string m_GroupingDesc ="";

        public string GroupingDesc
        {
            get { return m_GroupingDesc; }
            set { SetProperty(ref m_GroupingDesc, value); }
        }

        private string m_CityName;

        public string CityName
        {
            get { return m_CityName; }
            set { SetProperty(ref m_CityName, value); }
        }

        private string m_Location;

        public string Location
        {
            get { return m_Location; }
            set { SetProperty(ref m_Location, value); }
        }

        private int m_WallLevel;

        public int WallLevel
        {
            get { return m_WallLevel; }
            set { SetProperty(ref m_WallLevel, value); }
        }

        private string m_SpootTime;

        public string SpootTime
        {
            get { return m_SpootTime; }
            set { SetProperty(ref m_SpootTime, value); }
        }

        private CityTroops m_TroopsAtHome = new CityTroops();

        public CityTroops TroopsAtHome
        {
            get { return m_TroopsAtHome; }
            set { SetProperty(ref m_TroopsAtHome, value); }
        }

        private CityTroops m_TotalTroops = new CityTroops();

        public CityTroops TotalTroops
        {
            get { return m_TotalTroops; }
            set { SetProperty(ref m_TotalTroops, value); }
        }

        public override bool ContainsSearchExp(string exp)
        {
            if (string.IsNullOrWhiteSpace(exp))
                return true;

            if (!string.IsNullOrWhiteSpace(CityName) && CityName.Contains(exp))
                return true;

            if (!string.IsNullOrWhiteSpace(Location) && Location.Contains(exp))
                return true;

            return false;
        }
    }
}
