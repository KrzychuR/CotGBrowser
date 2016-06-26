using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GotGLib.NH.Schema
{
    public class DBCavernsHistory
    {
        virtual public int? Id { get; protected set; }
        virtual public string PlayerName { get; set; }
        virtual public int Rank { get; set; }
        virtual public long Score { get; set; }
        virtual public DateTime? CreateDT { get; set; }
    }
}
