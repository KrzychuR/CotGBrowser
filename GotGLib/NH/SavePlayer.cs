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
    public class SavePlayer : NHUnitOfWork
    {
        public Player Player2Save { get; set; }

        public Player Result { get; set; }

        public override void Execute()
        {
            var p = Session.QueryOver<DBPlayer>()
                    .Where(x => x.PlayerName == this.Player2Save.PlayerName)
                    .SingleOrDefault();

            if (p == null)
            {
                p = new DBPlayer();
            }

            AutoMapper.Mapper.Map<Player, DBPlayer>(Player2Save, p);
            Session.SaveOrUpdate(p);
            Result = AutoMapper.Mapper.Map<Player>(p);
        }
    }
}
