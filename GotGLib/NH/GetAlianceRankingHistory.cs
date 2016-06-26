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
    public class GetAlianceRankingHistory : NHUnitOfWork
    {
        public List<AlianceScoreHistory> Result;
        public string AlianceName { get; set; }
        public int Continent { get; set; }

        public override void Execute()
        {
            var q = Session.QueryOver<DBAlianceScoreHistory>()
                .Where(x => x.AlianceName == this.AlianceName && x.Continent == this.Continent);

            Result = AutoMapper.Mapper.Map<List<AlianceScoreHistory>>(q.List());
        }
    }
}
