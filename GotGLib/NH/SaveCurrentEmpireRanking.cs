using AutoMapper;
using GotGLib.DB;
using GotGLib.DTO;
using GotGLib.JS;
using GotGLib.NH.Schema;
using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GotGLib.NH
{
    public class SaveCurrentEmpireRanking : NHUnitOfWork
    {
        //public CurrentEmpireRanking CurrentRanking { get; set; }


        /*
        public override void Execute()
        {
            //zobaczmy czy mamy user na tym kontynencie...

            var dbRank = Session.QueryOver<DBCurrentEmpireRanking>()
                .Where(x => x.Continent == CurrentRanking.Continent && x.PlayerName == CurrentRanking.PlayerName)
                .SingleOrDefault();

            //dodajemy
            if(dbRank == null)
            {
                dbRank = new DBCurrentEmpireRanking();
                dbRank.Continent = CurrentRanking.Continent;
                dbRank.PlayerName = CurrentRanking.PlayerName;
            }

            //i aktualizujemy

            if(dbRank.Score.HasValue) //sojusze tylko dla ogólnej punktacji są
                dbRank.AlianceName = CurrentRanking.AlianceName;

            dbRank.UpdateDT = DateTime.Now;

            if (CurrentRanking.Rank.HasValue)
            {
                if (dbRank.Id.HasValue)
                    dbRank.RankLastChange = dbRank.Rank - CurrentRanking.Rank;

                dbRank.Rank = CurrentRanking.Rank;
            }

            if (CurrentRanking.Score.HasValue)
            {
                if (dbRank.Id.HasValue)
                    dbRank.ScoreLastChange = CurrentRanking.Score - dbRank.Score;

                dbRank.Score = CurrentRanking.Score;
            }

            if (CurrentRanking.CitiesNo.HasValue)
            {
                if (dbRank.Id.HasValue)
                    dbRank.CitiesNoLastChange = CurrentRanking.CitiesNo - dbRank.CitiesNo;

                dbRank.CitiesNo = CurrentRanking.CitiesNo;
            }

            if (CurrentRanking.UnitsKills.HasValue)
            {
                dbRank.UnitsKills = CurrentRanking.UnitsKills;
            }

            if (CurrentRanking.UnitsKillsRank.HasValue)
                dbRank.UnitsKillsRank = CurrentRanking.UnitsKillsRank;

            if (CurrentRanking.Caverns.HasValue)
                dbRank.Caverns = CurrentRanking.Caverns;

            if (CurrentRanking.CavernsRank.HasValue)
                dbRank.CavernsRank = CurrentRanking.CavernsRank;

            if (CurrentRanking.DefReputation.HasValue)
                dbRank.DefReputation = CurrentRanking.DefReputation;

            if (CurrentRanking.DefReputationRank.HasValue)
                dbRank.DefReputationRank = CurrentRanking.DefReputationRank;

            if (CurrentRanking.OffReputation.HasValue)
                dbRank.OffReputation = CurrentRanking.OffReputation;

            if (CurrentRanking.OffReputationRank.HasValue)
                dbRank.OffReputationRank = CurrentRanking.OffReputationRank;

            if (CurrentRanking.Score.HasValue)
            {
                //historia głównego rankingu
                var histRank = new DBRankingsEmpireScore();
                histRank.AlianceName = dbRank.AlianceName;
                histRank.CitiesNo = dbRank.CitiesNo.Value;
                histRank.Continent = dbRank.Continent;
                histRank.CreateDT = dbRank.UpdateDT;
                histRank.PlayerName = dbRank.PlayerName;
                histRank.Rank = dbRank.Rank.Value;
                histRank.Score = dbRank.Score.Value;
                Session.SaveOrUpdate(histRank);

                //odczytuję historię ostanich 3 pomiarów do obliczenia średniej z przyrostów
                var last3Rows = Session.QueryOver<DBRankingsEmpireScore>()
                    .Where(x => x.PlayerName == dbRank.PlayerName && x.Continent == dbRank.Continent)
                    .OrderBy(x => x.CreateDT).Desc
                    .Take(3)
                    .List();

                if (last3Rows.Count > 0)
                    dbRank.ScoreDiffAvg = (last3Rows.Max(x => x.Score) - last3Rows.Min(x => x.Score)) / last3Rows.Count;
                else
                    dbRank.ScoreDiffAvg = 0;
            }

            if (CurrentRanking.UnitsKills.HasValue)
            {
                //historia ubitych jednostek
                var histKills = new DBUnitsKillsHistory();
                histKills.PlayerName = dbRank.PlayerName;
                histKills.Rank = dbRank.UnitsKillsRank.Value;
                histKills.Score = dbRank.UnitsKills.Value;
                Session.SaveOrUpdate(histKills);

                //odczytuję historię ostanich 3 pomiarów do obliczenia średniego przyrostu
                var last3Rows = Session.QueryOver<DBUnitsKillsHistory>()
                    .Where(x => x.PlayerName == dbRank.PlayerName)
                    .OrderBy(x => x.CreateDT).Desc
                    .Take(3)
                    .List();

                if (last3Rows.Count > 0)
                    dbRank.UnitsKillsDiffAvg = (last3Rows.Max(x => x.Score) - last3Rows.Min(x => x.Score)) / last3Rows.Count;
                else
                    dbRank.UnitsKillsDiffAvg = 0;
            }

            if (CurrentRanking.DefReputation.HasValue)
            {
                //historia chwały w obronie
                var hist = new DBDefReputationHistory();
                hist.PlayerName = dbRank.PlayerName;
                hist.Rank = dbRank.DefReputationRank.Value;
                hist.Score = dbRank.DefReputation.Value;
                Session.SaveOrUpdate(hist);

                //odczytuję historię ostanich 3 pomiarów do obliczenia średniego przyrostu
                var last3Rows = Session.QueryOver<DBDefReputationHistory>()
                    .Where(x => x.PlayerName == dbRank.PlayerName)
                    .OrderBy(x => x.CreateDT).Desc
                    .Take(3)
                    .List();

                if (last3Rows.Count > 0)
                    dbRank.DefReputationDiffAvg = (last3Rows.Max(x => x.Score) - last3Rows.Min(x => x.Score)) / last3Rows.Count;
                else
                    dbRank.DefReputationDiffAvg = 0;
            }

            if (CurrentRanking.OffReputation.HasValue)
            {
                //historia chwały w ataku
                var hist = new DBOffReputationHistory();
                hist.PlayerName = dbRank.PlayerName;
                hist.Rank = dbRank.OffReputationRank.Value;
                hist.Score = dbRank.OffReputation.Value;
                Session.SaveOrUpdate(hist);

                //odczytuję historię ostanich 3 pomiarów do obliczenia średniego przyrostu
                var last3Rows = Session.QueryOver<DBOffReputationHistory>()
                    .Where(x => x.PlayerName == dbRank.PlayerName)
                    .OrderBy(x => x.CreateDT).Desc
                    .Take(3)
                    .List();

                if (last3Rows.Count > 0)
                    dbRank.OffReputationDiffAvg = (last3Rows.Max(x => x.Score) - last3Rows.Min(x => x.Score)) / last3Rows.Count;
                else
                    dbRank.OffReputationDiffAvg = 0;
            }

            if (CurrentRanking.Caverns.HasValue)
            {
                //historia lochów
                var histCav = new DBCavernsHistory();
                histCav.PlayerName = dbRank.PlayerName;
                histCav.Rank = dbRank.CavernsRank.Value;
                histCav.Score = dbRank.Caverns.Value;
                Session.SaveOrUpdate(histCav);
            }

            //tutaj bo wyżej jeszcze średnią wyliczam...
            Session.SaveOrUpdate(dbRank);
        }
        */

        public SaveCurrentEmpireRanking()
        {
            m_Log = log4net.LogManager.GetLogger(GetType());
        }

        public List<CurrentEmpireRanking> CurrentRankings { get; set; }

        private IList<DBCurrentEmpireRanking> m_EmpiresCache;
        private IList<DBRankingsEmpireScore> m_ScoreHistCache;
        private IList<DBUnitsKillsHistory> m_KilsHistCache;
        private IList<DBDefReputationHistory> m_DefHistCache;
        private IList<DBOffReputationHistory> m_OffHistCache;

        private ILog m_Log;

        private int m_TotalSteps;
        private int m_CurrentStep;

        private void LoadEmpires2Cache()
        {
            //zakładam że jest tylko 1 kontynent.. jak więcej to błąd!
            if(CurrentRankings.GroupBy(x => x.Continent).Count() > 1)
                throw new InvalidOperationException("Przekazano > 1 kontynent");

            var tmpEmpire = CurrentRankings.First();

            m_Log.InfoFormat("Loading empires to cache, continent: {0}", tmpEmpire.Continent);
            DoProgress(string.Format("Loading empires to cache, continent: {0}", tmpEmpire.Continent));

            m_EmpiresCache = Session.QueryOver<DBCurrentEmpireRanking>()
                .Where(x => x.Continent == tmpEmpire.Continent)
                .List();

            m_Log.InfoFormat("Loaded: {0} empires", m_EmpiresCache.Count);
            DoProgress(string.Format("Loaded: {0} empires", m_EmpiresCache.Count));

            m_Log.InfoFormat("Loading score history to cache, continent: {0}", tmpEmpire.Continent);
            DoProgress(string.Format("Loading score history to cache, continent: {0}", tmpEmpire.Continent));

            //jak duża historia? zakłądam że 5 dni starczy...
            var maxHistDT = DateTime.Now.AddDays(-5);

            m_ScoreHistCache = Session.QueryOver<DBRankingsEmpireScore>()
                .Where(x => x.Continent == tmpEmpire.Continent && x.CreateDT > maxHistDT)
                .List();

            m_Log.InfoFormat("Loaded: {0} rows", m_ScoreHistCache.Count);
            DoProgress(string.Format("Loaded: {0} rows", m_ScoreHistCache.Count));
        }

        public void LoadKillsCache()
        {
            m_Log.Info("Loading kills history to cache");
            DoProgress("Loading kills history to cache");

            //jak duża historia? zakłądam że 5 dni starczy...
            var maxHistDT = DateTime.Now.AddDays(-5);

            m_KilsHistCache = Session.QueryOver<DBUnitsKillsHistory>()
                .Where(x => x.CreateDT > maxHistDT)
                .List();

            m_Log.InfoFormat("Loaded: {0} rows", m_KilsHistCache.Count);
            DoProgress(string.Format("Loaded: {0} rows", m_KilsHistCache.Count));
        }

        public void LoadDefCache()
        {
            m_Log.Info("Loading def history to cache");
            DoProgress("Loading def history to cache");

            //jak duża historia? zakłądam że 5 dni starczy...
            var maxHistDT = DateTime.Now.AddDays(-5);

            m_DefHistCache = Session.QueryOver<DBDefReputationHistory>()
                .Where(x => x.CreateDT > maxHistDT)
                .List();

            m_Log.InfoFormat("Loaded: {0} rows", m_DefHistCache.Count);
            DoProgress(string.Format("Loaded: {0} rows", m_DefHistCache.Count));
        }

        public void LoadOffCache()
        {
            m_Log.Info("Loading off history to cache");
            DoProgress("Loading off history to cache");

            //jak duża historia? zakłądam że 5 dni starczy...
            var maxHistDT = DateTime.Now.AddDays(-5);

            m_OffHistCache = Session.QueryOver<DBOffReputationHistory>()
                .Where(x => x.CreateDT > maxHistDT)
                .List();

            m_Log.InfoFormat("Loaded: {0} rows", m_OffHistCache.Count);
            DoProgress(string.Format("Loaded: {0} rows", m_OffHistCache.Count));
        }

        public void UpdateEmpire(DBCurrentEmpireRanking dbRank, CurrentEmpireRanking currentRank)
        {
            if (dbRank.Score.HasValue) //sojusze tylko dla ogólnej punktacji są
                dbRank.AlianceName = currentRank.AlianceName;

            dbRank.UpdateDT = DateTime.Now;

            if (currentRank.Rank.HasValue)
            {
                if (dbRank.Id.HasValue)
                    dbRank.RankLastChange = dbRank.Rank - currentRank.Rank;

                dbRank.Rank = currentRank.Rank;
            }

            if (currentRank.Score.HasValue)
            {
                if (dbRank.Id.HasValue)
                    dbRank.ScoreLastChange = currentRank.Score - dbRank.Score;

                dbRank.Score = currentRank.Score;
            }

            if (currentRank.CitiesNo.HasValue)
            {
                if (dbRank.Id.HasValue)
                    dbRank.CitiesNoLastChange = currentRank.CitiesNo - dbRank.CitiesNo;

                dbRank.CitiesNo = currentRank.CitiesNo;
            }

            if (currentRank.UnitsKills.HasValue)
            {
                dbRank.UnitsKills = currentRank.UnitsKills;
            }

            if (currentRank.UnitsKillsRank.HasValue)
                dbRank.UnitsKillsRank = currentRank.UnitsKillsRank;

            if (currentRank.Caverns.HasValue)
                dbRank.Caverns = currentRank.Caverns;

            if (currentRank.CavernsRank.HasValue)
                dbRank.CavernsRank = currentRank.CavernsRank;

            if (currentRank.DefReputation.HasValue)
                dbRank.DefReputation = currentRank.DefReputation;

            if (currentRank.DefReputationRank.HasValue)
                dbRank.DefReputationRank = currentRank.DefReputationRank;

            if (currentRank.OffReputation.HasValue)
                dbRank.OffReputation = currentRank.OffReputation;

            if (currentRank.OffReputationRank.HasValue)
                dbRank.OffReputationRank = currentRank.OffReputationRank;

            if (currentRank.Score.HasValue)
            {
                //historia głównego rankingu
                var histRank = new DBRankingsEmpireScore();
                histRank.AlianceName = dbRank.AlianceName;
                histRank.CitiesNo = dbRank.CitiesNo.Value;
                histRank.Continent = dbRank.Continent;
                histRank.CreateDT = dbRank.UpdateDT;
                histRank.PlayerName = dbRank.PlayerName;
                histRank.Rank = dbRank.Rank.Value;
                histRank.Score = dbRank.Score.Value;
                Session.SaveOrUpdate(histRank);

                /*
                var last3Rows = Session.QueryOver<DBRankingsEmpireScore>()
                    .Where(x => x.PlayerName == dbRank.PlayerName && x.Continent == dbRank.Continent)
                    .OrderBy(x => x.CreateDT).Desc
                    .Take(3)
                    .List();    
                */

                //odczytuję historię ostanich 3 pomiarów do obliczenia średniej z przyrostów
                var last3Rows = m_ScoreHistCache
                    .Where(x => x.PlayerName == dbRank.PlayerName && x.Continent == dbRank.Continent)
                    .OrderByDescending(x => x.CreateDT)
                    .ToList();

                if (last3Rows.Count > 0)
                    dbRank.ScoreDiffAvg = (last3Rows.Max(x => x.Score) - last3Rows.Min(x => x.Score)) / last3Rows.Count;
                else
                    dbRank.ScoreDiffAvg = 0;
            }

            if (currentRank.UnitsKills.HasValue)
            {
                //historia ubitych jednostek
                var histKills = new DBUnitsKillsHistory();
                histKills.PlayerName = dbRank.PlayerName;
                histKills.Rank = dbRank.UnitsKillsRank.Value;
                histKills.Score = dbRank.UnitsKills.Value;
                histKills.CreateDT = dbRank.UpdateDT;
                Session.SaveOrUpdate(histKills);

                //odczytuję historię ostanich 3 pomiarów do obliczenia średniego przyrostu
                var last3Rows = m_KilsHistCache
                    .Where(x => x.PlayerName == dbRank.PlayerName)
                    .OrderByDescending(x => x.CreateDT)
                    .ToList();

                if (last3Rows.Count > 0)
                    dbRank.UnitsKillsDiffAvg = (last3Rows.Max(x => x.Score) - last3Rows.Min(x => x.Score)) / last3Rows.Count;
                else
                    dbRank.UnitsKillsDiffAvg = 0;
            }

            if (currentRank.DefReputation.HasValue)
            {
                //historia chwały w obronie
                var hist = new DBDefReputationHistory();
                hist.PlayerName = dbRank.PlayerName;
                hist.Rank = dbRank.DefReputationRank.Value;
                hist.Score = dbRank.DefReputation.Value;
                hist.CreateDT = dbRank.UpdateDT;
                Session.SaveOrUpdate(hist);

                //odczytuję historię ostanich 3 pomiarów do obliczenia średniego przyrostu
                var last3Rows = m_DefHistCache
                    .Where(x => x.PlayerName == dbRank.PlayerName)
                    .OrderByDescending(x => x.CreateDT)
                    .ToList();

                if (last3Rows.Count > 0)
                    dbRank.DefReputationDiffAvg = (last3Rows.Max(x => x.Score) - last3Rows.Min(x => x.Score)) / last3Rows.Count;
                else
                    dbRank.DefReputationDiffAvg = 0;
            }

            if (currentRank.OffReputation.HasValue)
            {
                //historia chwały w ataku
                var hist = new DBOffReputationHistory();
                hist.PlayerName = dbRank.PlayerName;
                hist.Rank = dbRank.OffReputationRank.Value;
                hist.Score = dbRank.OffReputation.Value;
                hist.CreateDT = dbRank.UpdateDT;
                Session.SaveOrUpdate(hist);

                //odczytuję historię ostanich 3 pomiarów do obliczenia średniego przyrostu
                var last3Rows = m_OffHistCache
                    .Where(x => x.PlayerName == dbRank.PlayerName)
                    .OrderByDescending(x => x.CreateDT)
                    .ToList();

                if (last3Rows.Count > 0)
                    dbRank.OffReputationDiffAvg = (last3Rows.Max(x => x.Score) - last3Rows.Min(x => x.Score)) / last3Rows.Count;
                else
                    dbRank.OffReputationDiffAvg = 0;
            }

            if (currentRank.Caverns.HasValue)
            {
                //historia lochów
                var histCav = new DBCavernsHistory();
                histCav.PlayerName = dbRank.PlayerName;
                histCav.Rank = dbRank.CavernsRank.Value;
                histCav.Score = dbRank.Caverns.Value;
                histCav.CreateDT = dbRank.UpdateDT;
                Session.SaveOrUpdate(histCav);
            }

            //tutaj bo wyżej jeszcze średnią wyliczam...
            Session.SaveOrUpdate(dbRank);
        }

        public override void Execute()
        {
            if (CurrentRankings.Count == 0)
                return;

            LoadEmpires2Cache(); //to zawsze

            //pozostała część... w zależności od danych... zakładam że są jednego 'typu' czyli score, off, deff.. itd...
            var tmpEmpire = CurrentRankings.First();

            if (tmpEmpire.UnitsKills.HasValue)
                LoadKillsCache();

            if (tmpEmpire.OffReputation.HasValue)
                LoadOffCache();

            if (tmpEmpire.DefReputation.HasValue)
                LoadDefCache();

            m_TotalSteps = CurrentRankings.Count;
            m_CurrentStep = 0;
            Stopwatch watch = new Stopwatch();
            watch.Restart();

            foreach (var emp in CurrentRankings)
            {
                //m_Log.InfoFormat("Empire: {0}", emp.PlayerName);

                var dbRank = m_EmpiresCache
                    .Where(x => x.Continent == emp.Continent && x.PlayerName == emp.PlayerName)
                    .SingleOrDefault();

                //dodajemy
                if (dbRank == null)
                {
                    //m_Log.InfoFormat("New empire: {0}", emp.PlayerName);
                    dbRank = new DBCurrentEmpireRanking();
                    dbRank.Continent = emp.Continent;
                    dbRank.PlayerName = emp.PlayerName;
                }

                //m_Log.Info("Updatin db...");
                UpdateEmpire(dbRank, emp);
                //m_Log.Info("Db updated.");
                m_CurrentStep++;

                if (watch.ElapsedMilliseconds > 500)
                {
                    m_Log.Info("Elapsed: "+watch.ElapsedMilliseconds.ToString());
                    DoProgress(string.Format("~Db updating.... {0}/{1}", m_CurrentStep, m_TotalSteps));
                    watch.Restart();
                }
            }
        }

        public event EventHandler<ProgressMessage> Progress;

        private void DoProgress(string msg)
        {
            var h = Progress;

            if (h != null)
            {
                h(this, new ProgressMessage() { Message = msg, Total = m_TotalSteps, Step = m_CurrentStep });
            }
        }

    }
}
