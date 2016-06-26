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
    public class GetKillsHistory : NHUnitOfWork
    {
        public string PlayerName { get; set; }

        public List<UnitsKillsHistory> Result { get; set; }

        public override void Execute()
        {
            var q = Session.QueryOver<DBUnitsKillsHistory>()
                    .Where(x => x.PlayerName == this.PlayerName);

            Result = AutoMapper.Mapper.Map<List<UnitsKillsHistory>>(q.List());
        }
    }
}
