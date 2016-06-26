using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GotGLib;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace GotGLib_Tests
{
    [TestClass]
    public class Parsers
    {
        [TestMethod]
        public void T001_RankingsParser()
        {
            var jsonData = System.IO.File.ReadAllText(@".\test_data\rankings.json");
            var conv = new GameDataConverter();
            var rankings = conv.GetRankings(jsonData);
        }

        [TestMethod]
        public void T002_UnitsKillsParser()
        {
            var jsonData = System.IO.File.ReadAllText(@".\test_data\unitskills.json");
            var conv = new GameDataConverter();
            var rankings = conv.GetUnitsKills(jsonData);
        }

        [TestMethod]
        public void T003_ChatParser()
        {
            string res;
            string html = "<span style=\"color: white\">19:12:43</span><span style=\"color:#F3D298\"> [World] </span><span style=\"color:#F3D298\"><span class=\"playerlink\" data=\"PrincepDio\">PrincepDio</span>: was it 85\" or something, dont remember </span><br>";
            html += "<span style=\"color: white\">19:12:43</span><span style=\"color:#F3D298\"> [World] </span><span style=\"color:#F3D298\"><span class=\"playerlink\" data=\"PrincepDio2\">K.R.</span>: was it 85\" or something, dont remember </span><br>"; ;

            Regex r = new Regex("<span class=\"playerlink\" data=\"([a-zA-Z0-9_]+)\">");
            var matches = r.Matches(html);

            if (matches.Count > 0)
            {
                foreach(Match m in matches)
                {
                    Trace.TraceInformation(m.ToString());
                }  
            }

            html = "<span style=\"color:white\">14:49:51</span><span style=\"color:#78b042\"> [Alliance] </span><span style=\"color:#78b042\"><span class=\"playerlink\" data=\"Ava\">Ava</span>: Internal Attack from <span style=\"color:#7979FF !important\" class=\"playerblink\">Akoma</span> (<span class=\"allyblink chatblink \" style=\"color:#7979FF !important\">TheEvilHusaria</span>) <span class=\"cityblink shcitt\" style=\"color:#7979FF !important\" data=\"11403536\">272:174</span> to <span style=\"color:#7979FF !important\" class=\"playerblink\">Cassandre</span> C12-06 <span class=\"cityblink shcitt\" style=\"color:#7979FF !important\" data=\"11337997\">269:173</span> speed: 00:26:05</span><br>";
            r = new Regex("<span class=\"allyblink chatblink \" style=\"color:#7979FF !important\">([a-zA-Z0-9_]+)</span>");

            foreach (Match m in r.Matches(html))
            {
                Trace.TraceInformation(m.Groups[1].Value);
            }

            /*
            html = "<span style=\"color: white\">18:52:17</span><span style=\"color:#78b042\"> [Alliance] </span><span style=\"color:#78b042\"><span class=\"playerlink\" data=\"Akoma\">Akoma</span>:  <span class=\"replink gFrep\" style=\"color:#7979FF !important\" data=\"185445416\">Share Report:185445416</span> </span><br>";
            r = new Regex("<span class=\"replink gFrep\".*data=\"([0-9]*)\">");
            match = r.Match(html);

            if (match.Success)
            {
                res = match.Groups[1].Value;
            }
            */
        }

        [TestMethod]
        public void T004_RaidsRepList()
        {
            var jsonData = System.IO.File.ReadAllText(@".\test_data\raids_reports_list.json");
            var conv = new GameDataConverter();
            var reports = conv.GetRaidReports(jsonData);
        }

        [TestMethod]
        public void T005_RaidRep()
        {
            var jsonData = System.IO.File.ReadAllText(@".\test_data\raid_report.json");
            var conv = new GameDataConverter();
            var report = conv.GetRaidReport(jsonData);
        }

        [TestMethod]
        public void T006_TroopsOverviewRep()
        {
            var jsonData = System.IO.File.ReadAllText(@".\test_data\all_troops.json");
            var conv = new GameDataConverter();
            var troops = conv.GetTroopsOverview(jsonData);

        }

        [TestMethod]
        public void T007_BuildQueue()
        {
            var jsonData = System.IO.File.ReadAllText(@".\test_data\build_q.json");
            var conv = new GameDataConverter();
            var q = conv.GetBuidQueue(jsonData);
            Trace.TraceInformation(q.Count.ToString());
        }
    }
}
