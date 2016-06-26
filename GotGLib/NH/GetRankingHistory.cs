using GotGLib.DTO;
using GotGLib.DB;
using GotGLib.NH.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GotGLib.NH
{
    public class GetRankingHistory: NHUnitOfWork
    {
        public int Continent { get; set; }
        public String PlayerName { get; set; }

        public List<EmpireScoreHistory> Result { get; set; }

        public override void Execute()
        {
            var q = Session.QueryOver<DBRankingsEmpireScore>()
                .Where(x => x.PlayerName == this.PlayerName && x.Continent == this.Continent);

            Result = AutoMapper.Mapper.Map<List<EmpireScoreHistory>>(q.List());
        }
    }
}
