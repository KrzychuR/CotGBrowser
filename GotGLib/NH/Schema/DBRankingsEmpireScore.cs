using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GotGLib.NH.Schema
{
    public class DBRankingsEmpireScore
    {
        virtual public int? Id { get; protected set; }
        virtual public string PlayerName { get; set; }
        virtual public int Rank { get; set; }
        virtual public int Score { get; set; }
        virtual public string AlianceName { get; set; }
        virtual public int CitiesNo { get; set; }
        virtual public int Continent { get; set; }
        virtual public DateTime? CreateDT { get; set; }
    }
}
