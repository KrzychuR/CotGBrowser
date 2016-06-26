using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GotGLib.DTO
{
    public class CurrentAlianceRanking: BaseDTO
    {
        public int? Id { get; protected set; }
        public int Rank { get; set; }
        public int Score { get; set; }
        public string AlianceName { get; set; }
        public int CitiesNo { get; set; }
        public int Continent { get; set; }
        public DateTime? UpdateDT { get; set; }
        public int Players { get; set; }

        public override bool ContainsSearchExp(string exp)
        {
            if (string.IsNullOrWhiteSpace(exp))
                return true;

            string upCaseExp = exp.ToUpper();

            if (AlianceName.ToUpper().Contains(upCaseExp))
            {
                return true;
            }
            else
                return false;
        }
    }
}
