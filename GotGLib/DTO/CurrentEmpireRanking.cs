using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GotGLib.DTO
{
    public class CurrentEmpireRanking: BaseDTO
    {
        public int? Id { get; protected set; }
        public string PlayerName { get; set; }
        public int? Rank { get; set; }
        public int? Score { get; set; }
        public string AlianceName { get; set; }
        public int? CitiesNo { get; set; }
        public int Continent { get; set; }
        public DateTime? UpdateDT { get; set; }
        public long? UnitsKills { get; set; }
        public int? UnitsKillsRank { get; set; }
        public long? Caverns { get; set; }
        public int? CavernsRank { get; set; }
        public double? UnitsKillsDiffAvg { get; set; }
        public double? ScoreDiffAvg { get; set; }
        public long? DefReputation { get; set; }
        public int? DefReputationRank { get; set; }
        public int? DefReputationRankLastChange { get; set; }
        public long? OffReputation { get; set; }
        public int? OffReputationRank { get; set; }
        public double? DefReputationDiffAvg { get; set; }
        public double? OffReputationDiffAvg { get; set; }
        public int? RankLastChange { get; set; }
        public int? ScoreLastChange { get; set; }
        public int? CitiesNoLastChange { get; set; }

        public override bool ContainsSearchExp(string exp)
        {
            if (string.IsNullOrWhiteSpace(exp))
                return true;

            string upCaseExp = exp.ToUpper();

            if (PlayerName.ToUpper().Contains(upCaseExp))
            {
                return true;
            }
            else if (!string.IsNullOrWhiteSpace(AlianceName) && AlianceName.ToUpper().Contains(upCaseExp))
            {
                return true;
            }
            else
                return false;
        }
    }
}
