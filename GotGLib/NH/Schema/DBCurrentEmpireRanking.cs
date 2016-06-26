using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GotGLib.NH.Schema
{
    public class DBCurrentEmpireRanking
    {
        virtual public int? Id { get; protected set; }
        virtual public string PlayerName { get; set; }
        virtual public int? Rank { get; set; }
        virtual public int? Score { get; set; }
        virtual public string AlianceName { get; set; }
        virtual public int? CitiesNo { get; set; }
        virtual public int Continent { get; set; }
        virtual public DateTime? UpdateDT { get; set; }
        virtual public long? UnitsKills { get; set; }
        virtual public int? UnitsKillsRank { get; set; }
        virtual public long? Caverns { get; set; }
        virtual public int? CavernsRank { get; set; }
        virtual public double? UnitsKillsDiffAvg { get; set; }
        virtual public double? ScoreDiffAvg { get; set; }
        virtual public long? DefReputation { get; set; }
        virtual public int? DefReputationRank { get; set; }
        virtual public int? DefReputationRankLastChange { get; set; }
        virtual public long? OffReputation { get; set; }
        virtual public int? OffReputationRank { get; set; }
        virtual public double? DefReputationDiffAvg { get; set; }
        virtual public double? OffReputationDiffAvg { get; set; }
        virtual public int? RankLastChange { get; set; }
        virtual public int? ScoreLastChange { get; set; }
        virtual public int? CitiesNoLastChange { get; set; }
    }
}
