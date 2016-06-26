using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GotGLib;
using Microsoft.Practices.Unity;
using GotGLib.DB;
using GotGLib.NH.Schema;
using System.Diagnostics;
using AutoMapper;

namespace GotGLib_Tests
{
    [TestClass]
    public class DBTests
    {
        private IUnityContainer IoC;
        private Database Db;


        [TestInitialize]
        public void Init()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<GotGLib.NH.Schema.MapperProfile>();
            });
            Mapper.AssertConfigurationIsValid();

            IoC = IoCHelper.GetIoC();
            Db = IoC.Resolve<Database>();
        }

        [TestMethod]
        public void T001_Connection()
        {
            using (var s = Db.SessionFactory.OpenSession())
            {
                var q = s.CreateSQLQuery("select 1");
                var o = q.UniqueResult();
                Assert.AreEqual("1", o.ToString());
            }
        }

        [TestMethod]
        public void T002_SchemaTest()
        {
            using (var s = Db.SessionFactory.OpenSession())
            {
                {
                    var l = s.QueryOver<DBRankingsEmpireScore>().Take(1).List();
                    Trace.TraceInformation("DBRankingsEmpireScore, count: {0}", l.Count);
                }

                {
                    var l = s.QueryOver<DBCurrentEmpireRanking>().Take(1).List();
                    Trace.TraceInformation("DBCurrentEmpireRanking, count: {0}", l.Count);
                }

                {
                    var l = s.QueryOver<DBUnitsKillsHistory>().Take(1).List();
                    Trace.TraceInformation("DBUnitsKillsHistory, count: {0}", l.Count);
                }

                {
                    var l = s.QueryOver<DBAlianceScoreHistory>().Take(1).List();
                    Trace.TraceInformation("DBAlianceScoreHistory, count: {0}", l.Count);
                }

                {
                    var l = s.QueryOver<DBCurrentAlianceRanking>().Take(1).List();
                    Trace.TraceInformation("DBCurrentAlianceRanking, count: {0}", l.Count);
                }
            }
        }

        [TestMethod]
        public void T003_ImportAlianceScoreHistoryCSV()
        {
            using (var s = Db.SessionFactory.OpenSession())
            {
                var file = System.IO.File.OpenText(@"C:\projekty\ProITSoft\CotGLibs\CotGLibs\CotGBrowser\data\AlianceScoreHistory.csv");
                string line;

                var t = s.BeginTransaction();

                while ((line = file.ReadLine()) != null)
                {
                    var fields = line.Split(';');

                    if (fields[0] == "Id")
                        continue; //header

                    var row = new DBAlianceScoreHistory();
                    row.AlianceName = fields[1];
                    row.Score = int.Parse(fields[2]);
                    row.CitiesNo = int.Parse(fields[3]);
                    row.CreateDT = DateTime.Parse(fields[4]);
                    row.Continent = int.Parse(fields[5]);
                    row.Players = int.Parse(fields[6]);
                    row.Rank = int.Parse(fields[7]);

                    s.SaveOrUpdate(row);
                }

                t.Commit();
            }
        }

        [TestMethod]
        public void T004_ImportCavernsHistoryCSV()
        {
            using (var s = Db.SessionFactory.OpenSession())
            {
                var file = System.IO.File.OpenText(@"C:\projekty\ProITSoft\CotGLibs\CotGLibs\CotGBrowser\data\CavernsHistory.csv");
                string line;

                var t = s.BeginTransaction();

                while ((line = file.ReadLine()) != null)
                {
                    var fields = line.Split(';');

                    if (fields[0] == "Id")
                        continue; //header

                    var row = new DBCavernsHistory();
                    row.PlayerName = fields[1];
                    row.Score = long.Parse(fields[2]);
                    row.Rank = int.Parse(fields[3]);
                    row.CreateDT = DateTime.Parse(fields[4]);

                    s.SaveOrUpdate(row);
                }

                t.Commit();
            }
        }

        [TestMethod]
        public void T005_ImportCurrentAlianceRankingCSV()
        {
            using (var s = Db.SessionFactory.OpenSession())
            {
                var file = System.IO.File.OpenText(@"C:\projekty\ProITSoft\CotGLibs\CotGLibs\CotGBrowser\data\CurrentAlianceRanking.csv");
                string line;

                var t = s.BeginTransaction();

                while ((line = file.ReadLine()) != null)
                {
                    var fields = line.Split(';');

                    if (fields[0] == "Id")
                        continue; //header

                    var row = new DBCurrentAlianceRanking();
                    row.AlianceName = fields[1];
                    row.Score = int.Parse(fields[2]);
                    row.CitiesNo= int.Parse(fields[3]);
                    row.UpdateDT = DateTime.Parse(fields[4]);
                    row.Continent = int.Parse(fields[5]);
                    row.Players = int.Parse(fields[6]);
                    row.Rank = int.Parse(fields[7]);

                    s.SaveOrUpdate(row);
                }

                t.Commit();
            }
        }

        [TestMethod]
        public void T006_ImportCurrentEmpireRankingCSV()
        {
            using (var s = Db.SessionFactory.OpenSession())
            {
                var file = System.IO.File.OpenText(@"C:\projekty\ProITSoft\CotGLibs\CotGLibs\CotGBrowser\data\CurrentEmpireRanking.csv");
                string line;

                var t = s.BeginTransaction();

                while ((line = file.ReadLine()) != null)
                {
                    var fields = line.Split(';');

                    if (fields[0] == "Id")
                        continue; //header

                    var row = new DBCurrentEmpireRanking();
                    int fieldIx = 0;
                    row.PlayerName = fields[++fieldIx];
                    row.Rank = int.Parse(fields[++fieldIx]);
                    row.Score = int.Parse(fields[++fieldIx]);
                    row.AlianceName = fields[++fieldIx];
                    row.CitiesNo = int.Parse(fields[++fieldIx]);
                    row.UpdateDT = DateTime.Parse(fields[++fieldIx]);
                    row.Continent = int.Parse(fields[++fieldIx]);

                    if (!string.IsNullOrWhiteSpace(fields[++fieldIx]))
                        row.UnitsKills = long.Parse(fields[fieldIx]);

                    if (!string.IsNullOrWhiteSpace(fields[++fieldIx]))
                        row.UnitsKillsRank = int.Parse(fields[fieldIx]);

                    if (!string.IsNullOrWhiteSpace(fields[++fieldIx]))
                        row.Caverns = long.Parse(fields[fieldIx]);

                    if (!string.IsNullOrWhiteSpace(fields[++fieldIx]))
                        row.CavernsRank = int.Parse(fields[fieldIx]);

                    if (!string.IsNullOrWhiteSpace(fields[++fieldIx]))
                        row.UnitsKillsDiffAvg = double.Parse(fields[fieldIx]);

                    if (!string.IsNullOrWhiteSpace(fields[++fieldIx]))
                        row.ScoreDiffAvg = double.Parse(fields[fieldIx]);

                    if (!string.IsNullOrWhiteSpace(fields[++fieldIx]))
                        row.DefReputation = long.Parse(fields[fieldIx]);

                    if (!string.IsNullOrWhiteSpace(fields[++fieldIx]))
                        row.DefReputationRank = int.Parse(fields[fieldIx]);

                    if (!string.IsNullOrWhiteSpace(fields[++fieldIx]))
                        row.OffReputation = long.Parse(fields[fieldIx]);

                    if (!string.IsNullOrWhiteSpace(fields[++fieldIx]))
                        row.OffReputationRank = int.Parse(fields[fieldIx]);

                    if (!string.IsNullOrWhiteSpace(fields[++fieldIx]))
                        row.DefReputationDiffAvg = double.Parse(fields[fieldIx]);

                    if (!string.IsNullOrWhiteSpace(fields[++fieldIx]))
                        row.OffReputationDiffAvg = double.Parse(fields[fieldIx]);

                    if (!string.IsNullOrWhiteSpace(fields[++fieldIx]))
                        row.RankLastChange = int.Parse(fields[fieldIx]);

                    if (!string.IsNullOrWhiteSpace(fields[++fieldIx]))
                        row.ScoreLastChange = int.Parse(fields[fieldIx]);

                    if (!string.IsNullOrWhiteSpace(fields[++fieldIx]))
                        row.CitiesNoLastChange = int.Parse(fields[fieldIx]);

                    if(!string.IsNullOrWhiteSpace(fields[++fieldIx]))
                        row.DefReputationRankLastChange = int.Parse(fields[fieldIx]);

                    s.SaveOrUpdate(row);
                }

                t.Commit();
            }
        }

        [TestMethod]
        public void T007_ImportDefReputationHistoryCSV()
        {
            using (var s = Db.SessionFactory.OpenSession())
            {
                var file = System.IO.File.OpenText(@"C:\projekty\ProITSoft\CotGLibs\CotGLibs\CotGBrowser\data\DefReputationHistory.csv");
                string line;

                var t = s.BeginTransaction();

                while ((line = file.ReadLine()) != null)
                {
                    var fields = line.Split(';');

                    if (fields[0] == "Id")
                        continue; //header

                    var row = new DBDefReputationHistory();
                    int fieldIx = 0;
                    row.PlayerName = fields[++fieldIx];
                    row.Score = long.Parse(fields[++fieldIx]);
                    row.Rank = int.Parse(fields[++fieldIx]);
                    row.CreateDT = DateTime.Parse(fields[++fieldIx]);

                    s.SaveOrUpdate(row);
                }

                t.Commit();
            }
        }

        [TestMethod]
        public void T008_ImportEmpireRankingsCSV()
        {
            using (var s = Db.SessionFactory.OpenSession())
            {
                var file = System.IO.File.OpenText(@"C:\projekty\ProITSoft\CotGLibs\CotGLibs\CotGBrowser\data\EmpireRankings.csv");
                string line;

                var t = s.BeginTransaction();

                while ((line = file.ReadLine()) != null)
                {
                    var fields = line.Split(';');

                    if (fields[0] == "Id")
                        continue; //header

                    var row = new DBRankingsEmpireScore();
                    int fieldIx = 0;
                    row.PlayerName = fields[++fieldIx];
                    row.Rank = int.Parse(fields[++fieldIx]);
                    row.Score = int.Parse(fields[++fieldIx]);
                    row.AlianceName = fields[++fieldIx];
                    row.CitiesNo = int.Parse(fields[++fieldIx]);
                    row.CreateDT = DateTime.Parse(fields[++fieldIx]);
                    row.Continent = int.Parse(fields[++fieldIx]);

                    s.SaveOrUpdate(row);
                }

                t.Commit();
            }
        }

        [TestMethod]
        public void T009_ImportOffReputationHistoryCSV()
        {
            using (var s = Db.SessionFactory.OpenSession())
            {
                var file = System.IO.File.OpenText(@"C:\projekty\ProITSoft\CotGLibs\CotGLibs\CotGBrowser\data\OffReputationHistory.csv");
                string line;

                var t = s.BeginTransaction();

                while ((line = file.ReadLine()) != null)
                {
                    var fields = line.Split(';');

                    if (fields[0] == "Id")
                        continue; //header

                    var row = new DBOffReputationHistory();
                    int fieldIx = 0;
                    row.PlayerName = fields[++fieldIx];
                    row.Score = long.Parse(fields[++fieldIx]);
                    row.Rank = int.Parse(fields[++fieldIx]);
                    row.CreateDT = DateTime.Parse(fields[++fieldIx]);

                    s.SaveOrUpdate(row);
                }

                t.Commit();
            }
        }

        [TestMethod]
        public void T010_ImportUnitsKillsHistoryCSV()
        {
            using (var s = Db.SessionFactory.OpenSession())
            {
                var file = System.IO.File.OpenText(@"C:\projekty\ProITSoft\CotGLibs\CotGLibs\CotGBrowser\data\UnitsKillsHistory.csv");
                string line;

                var t = s.BeginTransaction();

                while ((line = file.ReadLine()) != null)
                {
                    var fields = line.Split(';');

                    if (fields[0] == "Id")
                        continue; //header

                    var row = new DBUnitsKillsHistory();
                    int fieldIx = 0;
                    row.PlayerName = fields[++fieldIx];
                    row.Score = long.Parse(fields[++fieldIx]);
                    row.Rank = int.Parse(fields[++fieldIx]);
                    row.CreateDT = DateTime.Parse(fields[++fieldIx]);

                    s.SaveOrUpdate(row);
                }

                t.Commit();
            }
        }
    }
}
