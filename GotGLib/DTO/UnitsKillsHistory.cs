using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GotGLib.DTO
{
    public class UnitsKillsHistory: BaseDTO
    {
        public int? Id { get; protected set; }
        public string PlayerName { get; set; }
        public int Rank { get; set; }
        public long Score { get; set; }
        public DateTime? CreateDT { get; set; }
    }
}
