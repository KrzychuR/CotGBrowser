using GotGLib.DTO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GotGLib
{
    public class GameDataConverter
    {
        private int ParseInt(string data)
        {
            int tmp;

            if (int.TryParse(data, out tmp))
                return tmp;
            else
                return 0;
        }

        private long ParseLong(string data)
        {
            long tmp;

            if (long.TryParse(data, out tmp))
                return tmp;
            else
                return 0;
        }

        public List<CurrentEmpireRanking> GetRankings(string jsonRankingData)
        {
            var res = new List<CurrentEmpireRanking>();

            var allTokens = JsonConvert.DeserializeObject(jsonRankingData) as Newtonsoft.Json.Linq.JArray;

            if (allTokens != null)
            {
                var ranksArray = allTokens.First();

                foreach(var rankRow in ranksArray)
                {
                    var rank = new CurrentEmpireRanking();

                    foreach (Newtonsoft.Json.Linq.JProperty rankRowProperty in rankRow)
                    {
                        var name = rankRowProperty.Name;

                        switch (name)
                        {
                            case "1":
                                rank.PlayerName = rankRowProperty.Value.ToString();
                                break;
                            case "2":
                                rank.Rank = ParseInt(rankRowProperty.Value.ToString());
                                break;
                            case "3":
                                rank.Score = ParseInt(rankRowProperty.Value.ToString());
                                break;
                            case "4":
                                rank.AlianceName = rankRowProperty.Value.ToString();
                                break;
                            case "5":
                                rank.CitiesNo = ParseInt(rankRowProperty.Value.ToString());
                                break;
                            default:
                                break;
                        }
                    }

                    rank.UpdateDT = DateTime.Now;
                    res.Add(rank);
                }
            }

            return res;
        }

        public List<CurrentEmpireRanking> GetUnitsKills(string jsonRankingData)
        {
            var res = new List<CurrentEmpireRanking>();

            var allTokens = JsonConvert.DeserializeObject(jsonRankingData) as Newtonsoft.Json.Linq.JObject;

            if (allTokens != null)
            {
                var ranksArray = allTokens.First.First;

                foreach (var rankRow in ranksArray)
                {
                    var rank = new CurrentEmpireRanking();

                    foreach (Newtonsoft.Json.Linq.JProperty rankRowProperty in rankRow)
                    {
                        var name = rankRowProperty.Name;
                        rank.Continent = 56; //zawsze.. bo to ranking globalny

                        switch (name)
                        {
                            case "1":
                                rank.PlayerName = rankRowProperty.Value.ToString();
                                break;
                            case "2":
                                rank.UnitsKillsRank = ParseInt(rankRowProperty.Value.ToString());
                                break;
                            case "3":
                                rank.AlianceName = rankRowProperty.Value.ToString();
                                break;
                            case "4":
                                rank.UnitsKills = ParseLong(rankRowProperty.Value.ToString());
                                break;
                            default:
                                break;
                        }
                    }

                    rank.UpdateDT = DateTime.Now;
                    res.Add(rank);
                }
            }

            return res;
        }

        public List<CurrentAlianceRanking> GetAliancesRanking(string jsonRankingData)
        {
            var res = new List<CurrentAlianceRanking>();

            var allTokens = JsonConvert.DeserializeObject(jsonRankingData) as Newtonsoft.Json.Linq.JObject;

            if (allTokens != null)
            {
                var ranksArray = allTokens.First.First;

                foreach (var rankRow in ranksArray)
                {
                    var rank = new CurrentAlianceRanking();

                    foreach (Newtonsoft.Json.Linq.JProperty rankRowProperty in rankRow)
                    {
                        var name = rankRowProperty.Name;

                        switch (name)
                        {
                            case "1":
                                rank.AlianceName = rankRowProperty.Value.ToString();
                                break;
                            case "2":
                                rank.Rank = ParseInt(rankRowProperty.Value.ToString());
                                break;
                            case "3":
                                rank.Score = ParseInt(rankRowProperty.Value.ToString());
                                break;
                            case "4":
                                rank.Players = ParseInt(rankRowProperty.Value.ToString());
                                break;
                            case "5":
                                rank.CitiesNo = ParseInt(rankRowProperty.Value.ToString());
                                break;
                            default:
                                break;
                        }
                    }

                    rank.UpdateDT = DateTime.Now;
                    res.Add(rank);
                }
            }

            return res;
        }

        public List<CurrentEmpireRanking> GetCaverns(string jsonRankingData)
        {
            var res = new List<CurrentEmpireRanking>();

            var allTokens = JsonConvert.DeserializeObject(jsonRankingData) as Newtonsoft.Json.Linq.JObject;

            if (allTokens != null)
            {
                var ranksArray = allTokens.First.First;

                foreach (var rankRow in ranksArray)
                {
                    var rank = new CurrentEmpireRanking();

                    foreach (Newtonsoft.Json.Linq.JProperty rankRowProperty in rankRow)
                    {
                        var name = rankRowProperty.Name;
                        rank.Continent = 56; //zawsze.. bo to ranking globalny

                        switch (name)
                        {
                            case "1":
                                rank.PlayerName = rankRowProperty.Value.ToString();
                                break;
                            case "2":
                                rank.CavernsRank = ParseInt(rankRowProperty.Value.ToString());
                                break;
                            case "3":
                                rank.AlianceName = rankRowProperty.Value.ToString();
                                break;
                            case "4":
                                rank.Caverns = ParseLong(rankRowProperty.Value.ToString());
                                break;
                            default:
                                break;
                        }
                    }

                    rank.UpdateDT = DateTime.Now;
                    res.Add(rank);
                }
            }

            return res;
        }

        public List<CurrentEmpireRanking> GetDefReputation(string jsonRankingData)
        {
            var res = new List<CurrentEmpireRanking>();

            var allTokens = JsonConvert.DeserializeObject(jsonRankingData) as Newtonsoft.Json.Linq.JObject;

            if (allTokens != null)
            {
                var ranksArray = allTokens.First.First;

                foreach (var rankRow in ranksArray)
                {
                    var rank = new CurrentEmpireRanking();

                    foreach (Newtonsoft.Json.Linq.JProperty rankRowProperty in rankRow)
                    {
                        var name = rankRowProperty.Name;
                        rank.Continent = 56; //zawsze.. bo to ranking globalny

                        switch (name)
                        {
                            case "1":
                                rank.PlayerName = rankRowProperty.Value.ToString();
                                break;
                            case "2":
                                rank.DefReputationRank = ParseInt(rankRowProperty.Value.ToString());
                                break;
                            case "3":
                                rank.AlianceName = rankRowProperty.Value.ToString();
                                break;
                            case "4":
                                rank.DefReputation = ParseLong(rankRowProperty.Value.ToString());
                                break;
                            default:
                                break;
                        }
                    }

                    rank.UpdateDT = DateTime.Now;
                    res.Add(rank);
                }
            }

            return res;
        }

        public List<CurrentEmpireRanking> GetOffReputation(string jsonRankingData)
        {
            var res = new List<CurrentEmpireRanking>();

            var allTokens = JsonConvert.DeserializeObject(jsonRankingData) as Newtonsoft.Json.Linq.JObject;

            if (allTokens != null)
            {
                var ranksArray = allTokens.First.First;

                foreach (var rankRow in ranksArray)
                {
                    var rank = new CurrentEmpireRanking();

                    foreach (Newtonsoft.Json.Linq.JProperty rankRowProperty in rankRow)
                    {
                        var name = rankRowProperty.Name;
                        rank.Continent = 56; //zawsze.. bo to ranking globalny

                        switch (name)
                        {
                            case "1":
                                rank.PlayerName = rankRowProperty.Value.ToString();
                                break;
                            case "2":
                                rank.OffReputationRank = ParseInt(rankRowProperty.Value.ToString());
                                break;
                            case "3":
                                rank.AlianceName = rankRowProperty.Value.ToString();
                                break;
                            case "4":
                                rank.OffReputation = ParseLong(rankRowProperty.Value.ToString());
                                break;
                            default:
                                break;
                        }
                    }

                    rank.UpdateDT = DateTime.Now;
                    res.Add(rank);
                }
            }

            return res;
        }

        /// <summary>
        /// Dane na liście
        /// </summary>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        public List<RaidReport> GetRaidReports(string jsonData)
        {
            var res = new List<RaidReport>();

            var allTokens = JsonConvert.DeserializeObject(jsonData) as Newtonsoft.Json.Linq.JObject;

            if (allTokens != null)
            {
                var repArray = allTokens["b"] as Newtonsoft.Json.Linq.JArray;

                if(repArray != null)
                {
                    foreach(Newtonsoft.Json.Linq.JArray jRep in repArray)
                    {
                        var rep = new RaidReport();

                        rep.CavernType = (jRep[0] as Newtonsoft.Json.Linq.JValue).Value as string;
                        rep.AttackCity = (jRep[1] as Newtonsoft.Json.Linq.JValue).Value as string;
                        rep.CarryCapacity = double.Parse((jRep[3] as Newtonsoft.Json.Linq.JValue).Value.ToString());

                        var strDT = ((jRep[4] as Newtonsoft.Json.Linq.JValue).Value as string).Split(' ');
                        var time = TimeSpan.Parse(strDT[0]);
                        DateTime date = new DateTime(DateTime.Now.Year,
                            int.Parse(strDT[1].Split(new string[] { "/" }, StringSplitOptions.None)[1]),
                            int.Parse(strDT[1].Split(new string[] { "/" }, StringSplitOptions.None)[0]),
                            0, 0, 0);

                        rep.ReportDT = date.Add(time);
                        rep.CavernCoords = (jRep[5] as Newtonsoft.Json.Linq.JValue).Value as string;
                        rep.ReportId = (jRep[6] as Newtonsoft.Json.Linq.JValue).Value.ToString();
                        rep.IsNew = (jRep[7] as Newtonsoft.Json.Linq.JValue).Value.ToString() == "0";
                        rep.Progress = double.Parse((jRep[8] as Newtonsoft.Json.Linq.JValue).Value.ToString());

                        res.Add(rep);
                    }
                }
            }

            return res;
        }

        /// <summary>
        /// Detale
        /// </summary>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        public RaidReport GetRaidReport(string jsonData)
        {
            RaidReport res = null;
            var allTokens = JsonConvert.DeserializeObject(jsonData) as Newtonsoft.Json.Linq.JObject;

            if(allTokens != null)
            {
                res = new RaidReport();

                foreach(var token in allTokens)
                {
                    switch(token.Key)
                    {
                        case "rid":
                            res.ReportId = token.Value.ToString();
                            break;
                        case "tn": res.Title = token.Value.ToString();
                            break;
                        case "tcont":
                            res.TargetCont = token.Value.ToString();
                            break;
                        case "apn":
                            res.AttackPlayer = token.Value.ToString();
                            break;
                        case "acont":
                            res.AttackCont = token.Value.ToString();
                            break;
                        case "acoord":
                            res.AttackCoords = token.Value.ToString();
                            break;
                        case "anal":
                            res.AnalRes = new Resources(token.Value as JObject);
                            break;
                        case "tc":
                            res.CarryCapacityPcs= (int)token.Value;
                            break;
                        case "rt":
                            res.RtRes = new Resources(token.Value as JObject);
                            break;
                        case "r":
                            res.RRes = new Resources(token.Value as JObject);
                            break;
                        case "rl":
                            res.RlRes = new Resources(token.Value as JObject);
                            break;
                        case "at":
                            res.AttackTrops = new ObservableCollection<ReportTrops>();

                            foreach (var trop in token.Value)
                                res.AttackTrops.Add(new ReportTrops(trop as JObject));
                            
                            break;
                        case "dt":
                            res.DefTrops = new ObservableCollection<ReportTrops>();

                            foreach (var trop in token.Value)
                                res.DefTrops.Add(new ReportTrops(trop as JObject));

                            break;
                        default:
                            break;
                    }
                }
            }

            res.RefreshCalcFields();
            return res;
        }

        public List<CityOverview> GetTroopsOverview(string jsonData)
        {
            var res = new List<CityOverview>();
            var arr = JsonConvert.DeserializeObject(jsonData) as Newtonsoft.Json.Linq.JArray;

            if (arr != null)
            {
                foreach (JObject obj in arr)
                    res.Add(new CityOverview(obj));
            }

            return res;
        }

        public List<BuildQueue> GetBuidQueue(string jsonData)
        {
            var res = new List<BuildQueue>();
            var arr = JsonConvert.DeserializeObject(jsonData) as Newtonsoft.Json.Linq.JArray;

            if (arr != null)
            {
                foreach (Newtonsoft.Json.Linq.JArray item in arr)
                    res.Add(new BuildQueue(item));
            }

            return res;
        }
    }
}
