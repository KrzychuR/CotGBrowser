using AutoMapper;
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
    public class SaveCurrentAlianceRanking : NHUnitOfWork
    {
        public CurrentAlianceRanking AlianceScore { get; set; }

        public override void Execute()
        {
            var existing = Session.QueryOver<DBCurrentAlianceRanking>()
                .Where(x => x.AlianceName == AlianceScore.AlianceName && x.Continent == AlianceScore.Continent)
                .SingleOrDefault();

            if(existing == null)
            {
                existing = new DBCurrentAlianceRanking();
                existing.AlianceName = AlianceScore.AlianceName;
                existing.Continent = AlianceScore.Continent;
            }

            existing.Score = AlianceScore.Score;
            existing.CitiesNo = AlianceScore.CitiesNo;
            existing.Players = AlianceScore.Players;
            existing.Rank = AlianceScore.Rank;
            existing.UpdateDT = DateTime.Now;
            Session.SaveOrUpdate(existing);

            //to jeszcze historia...

            var hist = new DBAlianceScoreHistory();
            hist.AlianceName = AlianceScore.AlianceName;
            hist.CitiesNo = AlianceScore.CitiesNo;
            hist.Continent = AlianceScore.Continent;
            hist.CreateDT = existing.UpdateDT;
            hist.Rank = AlianceScore.Rank;
            hist.Players = AlianceScore.Players;
            hist.Score = AlianceScore.Score;
            Session.Save(hist);
        }
    }
}