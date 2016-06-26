using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GotGLib.DTO
{
    public class Player : BaseDTO
    {
        public int? Id { get; protected set; }
        public string PlayerName { get; set; }
        public DateTime? LastLoginDT { get; set; }
        public int? HasAccess2Reports { get; set; }
    }
}
