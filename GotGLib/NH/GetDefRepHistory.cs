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
    public class GetDefRepHistory : NHUnitOfWork
    {
        public string PlayerName { get; set; }

        public List<DefReputationHistory> Result { get; set; }

        public override void Execute()
        {
            var q = Session.QueryOver<DBDefReputationHistory>()
                    .Where(x => x.PlayerName == this.PlayerName);

            Result = AutoMapper.Mapper.Map<List<DefReputationHistory>>(q.List());
        }
    }
}
