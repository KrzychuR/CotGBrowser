using FluentNHibernate.Mapping;
using NHibernate.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GotGLib.NH.Schema
{
    public class DBPlayerMap: ClassMap<DBPlayer>
    {
        public DBPlayerMap()
        {
            Table("Players");

            //Primary Key oraz sposób jego generacji
            Id(x => x.Id).GeneratedBy.Native();
            Map(x => x.PlayerName);
            Map(x => x.LastLoginDT)
                .CustomType<TimestampType>();
            Map(x => x.HasAccess2Reports);
        }
    }

    public class DBRankingsEmpireScoreMap: ClassMap<DBRankingsEmpireScore>
    {
        public DBRankingsEmpireScoreMap()
        {
            Table("RankingsEmpireScore");
            
            //Primary Key oraz sposób jego generacji
            Id(x => x.Id).GeneratedBy.Native();

            Map(x => x.PlayerName);
            Map(x => x.AlianceName);
            Map(x => x.CitiesNo);
            Map(x => x.Continent);
            Map(x => x.Rank);
            Map(x => x.Score);
            Map(x => x.CreateDT)
                .CustomType<TimestampType>(); 
        }
    }

    public class DBCurrentEmpireRankingMap : ClassMap<DBCurrentEmpireRanking>
    {
        public DBCurrentEmpireRankingMap()
        {
            Table("CurrentEmpireRanking");

            //Primary Key oraz sposób jego generacji
            Id(x => x.Id).GeneratedBy.Native();

            Map(x => x.PlayerName);
            Map(x => x.AlianceName);
            Map(x => x.CitiesNo);
            Map(x => x.Continent);
            Map(x => x.Rank);
            Map(x => x.Score);
            Map(x => x.UnitsKills);
            Map(x => x.UnitsKillsRank);
            Map(x => x.Caverns);
            Map(x => x.CavernsRank);
            Map(x => x.UnitsKillsDiffAvg);
            Map(x => x.ScoreDiffAvg);

            Map(x => x.DefReputation);
            Map(x => x.DefReputationRank);
            Map(x => x.DefReputationRankLastChange);
            Map(x => x.OffReputation);
            Map(x => x.OffReputationRank);
            Map(x => x.DefReputationDiffAvg);
            Map(x => x.OffReputationDiffAvg);
            Map(x => x.RankLastChange);
            Map(x => x.ScoreLastChange);
            Map(x => x.CitiesNoLastChange);

            Map(x => x.UpdateDT).CustomType<TimestampType>();
        }
    }

    public class DBUnitsKillsHistoryMap : ClassMap<DBUnitsKillsHistory>
    {
        public DBUnitsKillsHistoryMap()
        {
            Table("UnitsKillsHistory");

            //Primary Key oraz sposób jego generacji
            Id(x => x.Id).GeneratedBy.Native();

            Map(x => x.PlayerName);
            Map(x => x.Rank);
            Map(x => x.Score);
            Map(x => x.CreateDT)
                .CustomType<TimestampType>();
        }
    }

    public class DBOffReputationHistoryMap : ClassMap<DBOffReputationHistory>
    {
        public DBOffReputationHistoryMap()
        {
            Table("OffReputationHistory");

            //Primary Key oraz sposób jego generacji
            Id(x => x.Id).GeneratedBy.Native();

            Map(x => x.PlayerName);
            Map(x => x.Rank);
            Map(x => x.Score);
            Map(x => x.CreateDT)
                .CustomType<TimestampType>();
        }
    }

    public class DBDefReputationHistoryMap : ClassMap<DBDefReputationHistory>
    {
        public DBDefReputationHistoryMap()
        {
            Table("DefReputationHistory");

            //Primary Key oraz sposób jego generacji
            Id(x => x.Id).GeneratedBy.Native();

            Map(x => x.PlayerName);
            Map(x => x.Rank);
            Map(x => x.Score);
            Map(x => x.CreateDT)
                .CustomType<TimestampType>();
        }
    }

    public class DBAlianceScoreHistoryMap : ClassMap<DBAlianceScoreHistory>
    {
        public DBAlianceScoreHistoryMap()
        {
            Table("AlianceScoreHistory");

            //Primary Key oraz sposób jego generacji
            Id(x => x.Id).GeneratedBy.Native();

            Map(x => x.Rank);
            Map(x => x.Players);
            Map(x => x.AlianceName);
            Map(x => x.CitiesNo);
            Map(x => x.Continent);
            Map(x => x.Score);
            Map(x => x.CreateDT)
                .CustomType<TimestampType>();
        }
    }

    public class DBCurrentAlianceRankingMap : ClassMap<DBCurrentAlianceRanking>
    {
        public DBCurrentAlianceRankingMap()
        {
            Table("CurrentAlianceRanking");

            //Primary Key oraz sposób jego generacji
            Id(x => x.Id).GeneratedBy.Native();

            Map(x => x.Rank);
            Map(x => x.Players);
            Map(x => x.AlianceName);
            Map(x => x.CitiesNo);
            Map(x => x.Continent);
            Map(x => x.Score);
            Map(x => x.UpdateDT)
                .CustomType<TimestampType>();
        }
    }

    public class DBCavernsHistoryMap : ClassMap<DBCavernsHistory>
    {
        public DBCavernsHistoryMap()
        {
            Table("CavernsHistory");

            //Primary Key oraz sposób jego generacji
            Id(x => x.Id).GeneratedBy.Native();

            Map(x => x.PlayerName);
            Map(x => x.Rank);
            Map(x => x.Score);
            Map(x => x.CreateDT)
                .CustomType<TimestampType>();
        }
    }
}
