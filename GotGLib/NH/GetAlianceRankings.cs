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
    public class GetAlianceRankings: NHUnitOfWork
    {
        public int Continent { get; set; }

        public List<CurrentAlianceRanking> Result;

        public override void Execute()
        {
            var q = Session.QueryOver<DBCurrentAlianceRanking>();

            if (Continent > 0)
                q.Where(x => x.Continent == this.Continent);

            Result = AutoMapper.Mapper.Map<List<CurrentAlianceRanking>>(q.List());
        }
    }
}
