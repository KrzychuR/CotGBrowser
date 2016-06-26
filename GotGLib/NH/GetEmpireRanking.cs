using GotGLib.DB;
using GotGLib.DTO;
using GotGLib.NH.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GotGLib.NH
{
    public class GetEmpireRanking : NHUnitOfWork
    {
        public int Continent { get; set; }

        public List<CurrentEmpireRanking> Result { get; set; }

        public override void Execute()
        {
            var q = Session.QueryOver<DBCurrentEmpireRanking>();

            if (Continent > 0)
                q.Where(x => x.Continent == this.Continent);

            Result = AutoMapper.Mapper.Map<List<CurrentEmpireRanking>>(q.List());
            
            //jeżeli kontynent <> 56 to pobieram dodatkowo pewne dane globalne
            if(Continent != 56)
            {
                var q56 = Session.QueryOver<DBCurrentEmpireRanking>()
                    .Where(x => x.Continent == 56);

                foreach(var totalEmpireData in q56.List())
                {
                    foreach(var e in Result.Where(x => x.PlayerName == totalEmpireData.PlayerName))
                    {
                        e.UnitsKills = totalEmpireData.UnitsKills;
                        e.UnitsKillsRank = totalEmpireData.UnitsKillsRank;
                        e.Caverns = totalEmpireData.Caverns;
                        e.CavernsRank = totalEmpireData.CavernsRank;
                        e.UnitsKillsDiffAvg = totalEmpireData.UnitsKillsDiffAvg;
                        e.ScoreDiffAvg = totalEmpireData.ScoreDiffAvg;
                        e.DefReputation = totalEmpireData.DefReputation;
                        e.DefReputationDiffAvg = totalEmpireData.DefReputationDiffAvg;
                        e.DefReputationRank = totalEmpireData.DefReputationRank;
                        e.OffReputation = totalEmpireData.OffReputation;
                        e.OffReputationDiffAvg = totalEmpireData.OffReputationDiffAvg;
                        e.OffReputationRank = totalEmpireData.OffReputationRank;                       
                    }
                }
            }
        }
    }
}
