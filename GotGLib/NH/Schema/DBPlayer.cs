using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GotGLib.NH.Schema
{
    public class DBPlayer
    {
        virtual public int? Id { get; protected set; }
        virtual public string PlayerName { get; set; }
        virtual public DateTime? LastLoginDT { get; set; }
        virtual public int? HasAccess2Reports { get; set; }
    }
}
