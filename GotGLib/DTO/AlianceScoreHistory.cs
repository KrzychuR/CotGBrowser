using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GotGLib.DTO
{
    public class AlianceScoreHistory: BaseDTO
    {
        public int? Id { get; protected set; }
        public int Rank { get; set; }
        public int Score { get; set; }
        public string AlianceName { get; set; }
        public int CitiesNo { get; set; }
        public int Continent { get; set; }
        public DateTime? CreateDT { get; set; }
        public int Players { get; set; }
    }
}
