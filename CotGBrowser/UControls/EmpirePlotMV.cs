using CotGBrowser.Common;
using GotGLib.DTO;
using GotGLib.Res;
using OxyPlot;
using OxyPlot.Axes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CotGBrowser.UControls
{
    public class EmpirePlotMV: BaseModelView
    {
        private PlotModel m_PlotM;

        public PlotModel PlotM
        {
            get { return m_PlotM; }
            set { SetProperty(ref m_PlotM, value); }
        }

        private Dictionary<CurrentEmpireRanking, List<EmpireScoreHistory>> m_Empires;

        /// <summary>
        /// Dane graczy do pokazania, porównania
        /// </summary>
        public Dictionary<CurrentEmpireRanking, List<EmpireScoreHistory>> Empires
        {
            get { return m_Empires; }
            set
            {
                if(SetProperty(ref m_Empires, value))
                {
                    RefreshPlot();
                }
            }
        }

        private Dictionary<CurrentEmpireRanking, List<UnitsKillsHistory>> m_EmpiresUnitKills;

        /// <summary>
        /// Dane graczy do pokazania, porównania
        /// </summary>
        public Dictionary<CurrentEmpireRanking, List<UnitsKillsHistory>> EmpiresUnitKills
        {
            get { return m_EmpiresUnitKills; }
            set
            {
                if (SetProperty(ref m_EmpiresUnitKills, value))
                {
                    RefreshPlot();
                }
            }
        }

        private Dictionary<CurrentEmpireRanking, List<DefReputationHistory>> m_DefReputations;

        /// <summary>
        /// Dane graczy do pokazania, porównania
        /// </summary>
        public Dictionary<CurrentEmpireRanking, List<DefReputationHistory>> DefReputations
        {
            get { return m_DefReputations; }
            set
            {
                if (SetProperty(ref m_DefReputations, value))
                {
                    RefreshPlot();
                }
            }
        }

        private Dictionary<CurrentEmpireRanking, List<OffReputationHistory>> m_OffReputations;

        /// <summary>
        /// Dane graczy do pokazania, porównania
        /// </summary>
        public Dictionary<CurrentEmpireRanking, List<OffReputationHistory>> OffReputations
        {
            get { return m_OffReputations; }
            set
            {
                if (SetProperty(ref m_OffReputations, value))
                {
                    RefreshPlot();
                }
            }
        }


        private bool m_DiffDefReputation;

        /// <summary>
        /// Czy reputację różnicowo pokazywać?
        /// </summary>
        public bool DiffDefReputation
        {
            get { return m_DiffDefReputation; }
            set
            {
                if (SetProperty(ref m_DiffDefReputation, value))
                {
                    RefreshPlot();
                }
            }
        }

        private bool m_DiffScore;

        /// <summary>
        /// Czy punktacja ma być różnicowa ?
        /// </summary>
        public bool DiffScore
        {
            get { return m_DiffScore; }
            set
            {
                if(SetProperty(ref m_DiffScore, value))
                {
                    RefreshPlot();
                }
            }
        }

        private bool m_ShowCities;

        /// <summary>
        /// czy pokazywać ilość miast
        /// </summary>
        public bool ShowCities
        {
            get { return m_ShowCities; }
            set
            {
                if(SetProperty(ref m_ShowCities, value))
                {
                    RefreshPlot();
                }
            }
        }

        private bool m_ShowScore;

        /// <summary>
        /// Czy pokazywać punktację
        /// </summary>
        public bool ShowScore
        {
            get { return m_ShowScore; }
            set
            {
                if( SetProperty(ref m_ShowScore, value) )
                {
                    RefreshPlot();
                }
            }
        }

        private bool m_ShowUnitsKills;

        /// <summary>
        /// Czy pokazywać punktację zabitych jednostek
        /// </summary>
        public bool ShowUnitsKills
        {
            get { return m_ShowUnitsKills; }
            set
            {
                if (SetProperty(ref m_ShowUnitsKills, value))
                {
                    RefreshPlot();
                }
            }
        }

        private bool m_DiffKillsScore;

        /// <summary>
        /// Czy ilości zabitych jednostek też mają być różnicowe ?
        /// </summary>
        public bool DiffKillsScore
        {
            get { return m_DiffKillsScore; }
            set
            {
                if (SetProperty(ref m_DiffKillsScore, value))
                {
                    RefreshPlot();
                }
            }
        }

        private bool m_ShowKillsAsBars;

        /// <summary>
        /// Czy ilości zabitych jednostek też mają być różnicowe ?
        /// </summary>
        public bool ShowKillsAsBars
        {
            get { return m_ShowKillsAsBars; }
            set
            {
                if (SetProperty(ref m_ShowKillsAsBars, value))
                {
                    RefreshPlot();
                }
            }
        }

        private DateTime? m_FromDate = DateTime.Now.AddDays(-5);

        public DateTime? FromDate
        {
            get { return m_FromDate; }
            set
            {
                if( SetProperty(ref m_FromDate, value))
                {
                    RefreshPlot();
                }
            }
        }

        private int m_ServerTimeDiffH = -4;

        public int ServerTimeDiffH
        {
            get { return m_ServerTimeDiffH; }
            set
            {
                if(SetProperty(ref m_ServerTimeDiffH, value));
                {
                    RefreshPlot();
                }
            }
        }

        private bool m_ShowDefRep;

        /// <summary>
        /// Czy pokazywać reputację w obronie
        /// </summary>
        public bool ShowDefRep
        {
            get { return m_ShowDefRep; }
            set
            {
                if (SetProperty(ref m_ShowDefRep, value))
                {
                    RefreshPlot();
                }
            }
        }

        /// <summary>
        /// Punkty w czasie, kategorie na X
        /// </summary>
        private CategoryAxis m_XCatDates;

        private void RefreshPlot()
        {
            PlotModel m = new PlotModel();
            m.Title = Labels.EmpireStats;

            if (ShowScore)
                PlotScore(m);

            if (ShowCities)
                PlotCities(m);

            if (ShowUnitsKills)
            {
                if (ShowKillsAsBars)
                    PlotUnitKillsBar(m);
                else
                    PlotUnitKills(m);
            }

            if (ShowDefRep)
            {
                m_XCatDates = null;

                LinearAxis yAxis = new LinearAxis();
                yAxis.Title = Labels.Reputation;

                if (DiffDefReputation)
                    yAxis.Title += " "+Labels.Difference1;

                yAxis.Key = "reputation";
                yAxis.Position = AxisPosition.Left;
                yAxis.MajorGridlineStyle = LineStyle.Solid;
                yAxis.MinorGridlineStyle = LineStyle.Dash;
                m.Axes.Add(yAxis);

                PlotDefRepBars(m);
                PlotOffRepBars(m);
            }

            PlotM = m;
        }

        private void AddXDatesAxis(PlotModel m)
        {
            //oś X - z datami, w sumie zawsze :)
            DateTimeAxis xAxis = new DateTimeAxis();
            xAxis.Position = AxisPosition.Bottom;
            xAxis.Key = "x";
            xAxis.StringFormat = "yyyy-MM-dd	";
            xAxis.Angle = -45;
            xAxis.MajorGridlineStyle = LineStyle.Solid;
            xAxis.MinorGridlineStyle = LineStyle.Dot;
            m.Axes.Add(xAxis);
        }

        private void PlotScore(PlotModel m)
        {
            if (Empires == null)
                return;

            AddXDatesAxis(m);

            //punktacja
            LinearAxis scoreY = new LinearAxis();

            if (DiffScore)
                scoreY.Title = Labels.ScoreSpeed;
            else
                scoreY.Title = Labels.Score;

            scoreY.Key = "score";
            scoreY.Position = AxisPosition.Left;
            scoreY.MajorGridlineStyle = LineStyle.Solid;
            scoreY.MinorGridlineStyle = LineStyle.Dot;
            m.Axes.Add(scoreY);

            foreach (var empire in Empires.OrderBy(x => x.Key.Rank))
            {
                long lastScore = -1;
                double scoreValue = 0;
                DateTime lastTime = DateTime.Now;
                double deltaTime = 0;

                OxyPlot.Series.LineSeries ls = new OxyPlot.Series.LineSeries();
                ls.XAxisKey = "x";
                ls.YAxisKey = "score";
                ls.MarkerType = MarkerType.Diamond;
                ls.Title = string.Format("{0} ({1})", empire.Key.PlayerName, empire.Key.AlianceName);

                foreach (var hr in empire.Value.OrderBy(x => x.CreateDT.Value))
                {
                    if (DiffScore)
                    {
                        if (lastScore < 0)
                        {
                            //pierwszy wynik, zaczynam od zera
                            lastScore = hr.Score;
                            lastTime = hr.CreateDT.Value;
                            scoreValue = 0;
                        }
                        else
                        {
                            deltaTime = (hr.CreateDT.Value - lastTime).TotalMinutes;

                            //liczę przyrosty na minutę, jeżeli czas między pomiaram > 3h, inaczej nie ma to sensu - za wolno punkty przyrastają i będąg głupoty
                            if (deltaTime > 180)
                            {
                                scoreValue = (hr.Score - lastScore) / deltaTime;
                                lastScore = hr.Score;
                                lastTime = hr.CreateDT.Value;
                            }
                        }
                    }
                    else
                        scoreValue = hr.Score;

                    if (((DiffScore && (deltaTime > 60)) || !DiffScore) && ShowScore)
                        ls.Points.Add(DateTimeAxis.CreateDataPoint(hr.CreateDT.Value, scoreValue));
                }

                m.Series.Add(ls);
            }
        }

        private void PlotCities(PlotModel m)
        {
            if (Empires == null)
                return;

            AddXDatesAxis(m);

            //miasta
            LinearAxis citiesY = new LinearAxis();
            citiesY.Title = Labels.Cities;
            citiesY.Key = "cities";
            citiesY.Position = AxisPosition.Right;
            citiesY.MajorGridlineStyle = LineStyle.None;
            citiesY.MinorGridlineStyle = LineStyle.None;
            m.Axes.Add(citiesY);

            foreach (var empire in Empires.OrderBy(x => x.Key.CitiesNo))
            {
                OxyPlot.Series.StairStepSeries citiesSerie = new OxyPlot.Series.StairStepSeries();
                citiesSerie.XAxisKey = "x";
                citiesSerie.YAxisKey = "cities";
                citiesSerie.VerticalStrokeThickness = 0.2;

                citiesSerie.MarkerType = MarkerType.None;
                citiesSerie.Title = string.Format("{0} ({1})", empire.Key.PlayerName, empire.Key.AlianceName);

                foreach (var hr in empire.Value.OrderBy(x => x.CreateDT.Value))
                {
                    citiesSerie.Points.Add(DateTimeAxis.CreateDataPoint(hr.CreateDT.Value, hr.CitiesNo));
                }

                m.Series.Add(citiesSerie);
            }
        }

        private void PlotUnitKills(PlotModel m)
        {
            if (EmpiresUnitKills == null)
                return;

            AddXDatesAxis(m);

            LinearAxis citiesY = new LinearAxis();
            citiesY.Title = Labels.KilledUnits;
            citiesY.Key = "units_kills";
            citiesY.Position = AxisPosition.Right;
            m.Axes.Add(citiesY);

            long lastKills = -1;
            double killsValue = 0;

            foreach (var empire in EmpiresUnitKills.OrderBy(x => x.Key.UnitsKillsRank))
            {
                lastKills = -1;
                killsValue = 0;
                OxyPlot.Series.StairStepSeries s = new OxyPlot.Series.StairStepSeries();
                s.XAxisKey = "x";
                s.YAxisKey = "units_kills";
                s.VerticalStrokeThickness = 0.2;
                s.MarkerType = MarkerType.None;
                s.Title = string.Format("{0} ({1})", empire.Key.PlayerName, empire.Key.AlianceName);

                foreach (var hr in empire.Value.OrderBy(x => x.CreateDT.Value))
                {
                    if (DiffKillsScore)
                    {
                        if (lastKills < 0)
                        {
                            //pierwszy wynik, zaczynam od zera
                            lastKills = hr.Score;
                            killsValue = 0;
                        }
                        else
                        {
                            killsValue = hr.Score - lastKills;
                            lastKills = hr.Score;
                        }
                    }
                    else
                        killsValue = hr.Score;

                    s.Points.Add(DateTimeAxis.CreateDataPoint(hr.CreateDT.Value, killsValue));
                }

                m.Series.Add(s);
            }
        }

        private void PlotUnitKillsBar(PlotModel m)
        {
            if (EmpiresUnitKills == null)
                return;

            LinearAxis yAxis = new LinearAxis();
            yAxis.Title = Labels.KilledUnits;

            if (DiffKillsScore)
                yAxis.Title += " "+Labels.Difference1;

            yAxis.Key = "units_kills";
            yAxis.Position = AxisPosition.Left;
            yAxis.MajorGridlineStyle = LineStyle.Solid;
            yAxis.MinorGridlineStyle = LineStyle.Dot;
            m.Axes.Add(yAxis);

            //daty
            var categories = new CategoryAxis();
            categories.Title = Labels.Date;
            categories.Position = AxisPosition.Bottom;
            categories.GapWidth = 0.1;
            m.Axes.Add(categories);

            long lastKills = -1;
            double killsValue = 0;

            foreach (var empire in EmpiresUnitKills.OrderBy(x => x.Key.UnitsKillsRank))
            {
                lastKills = -1;
                killsValue = 0;
                OxyPlot.Series.ColumnSeries cs = new OxyPlot.Series.ColumnSeries();
                cs.LabelFormatString = "{0}";              
                cs.Title =  string.Format("{0} ({1})", empire.Key.PlayerName, empire.Key.AlianceName) ;
                int categoryIx = -1;

                foreach (var hr in empire.Value.OrderBy(x => x.CreateDT.Value))
                {
                    if (DiffKillsScore)
                    {
                        if (lastKills < 0)
                        {
                            //pierwszy wynik, zaczynam od zera
                            lastKills = hr.Score;
                            killsValue = 0;
                        }
                        else
                        {
                            killsValue = hr.Score - lastKills;
                            lastKills = hr.Score;
                        }
                    }
                    else
                        killsValue = hr.Score;

                    var serverTime = hr.CreateDT.Value.AddHours(ServerTimeDiffH);

                    if (FromDate.HasValue && serverTime >= FromDate)
                    {
                        string catName = string.Format("{0} h:{1}", serverTime.ToString("yy.MM.dd"), serverTime.ToString("HH"));
                        categoryIx = categories.Labels.IndexOf(catName);

                        if (categoryIx < 0)
                        {
                            categories.Labels.Add(catName);
                            categoryIx = categories.Labels.IndexOf(catName);
                        }

                        cs.Items.Add(new OxyPlot.Series.ColumnItem(killsValue, categoryIx));
                    }
                }

                m.Series.Add(cs);
            }
        }

        private void PlotDefRepBars(PlotModel m)
        {
            if (DefReputations == null)
                return;

            //daty
            if (m_XCatDates == null)
            {
                m_XCatDates = new CategoryAxis();
                m_XCatDates.Title = Labels.Date;
                m_XCatDates.Position = AxisPosition.Bottom;
                m_XCatDates.GapWidth = 0.1;
                m.Axes.Add(m_XCatDates);
            }

            long lastScore = -1;
            double scoreValue = 0;

            foreach (var empire in DefReputations.OrderBy(x => x.Key.DefReputation))
            {
                lastScore = -1;
                scoreValue = 0;
                var s = new OxyPlot.Series.ColumnSeries();
                s.LabelFormatString = "D: "+ empire.Key.PlayerName;
                s.Title = string.Format("D: {0} ({1})", empire.Key.PlayerName, empire.Key.AlianceName);
                s.YAxisKey = "reputation";
                s.StackGroup = empire.Key.PlayerName;
                s.IsStacked = true;
                s.FontSize = 9;
                s.LabelPlacement = OxyPlot.Series.LabelPlacement.Inside;
                int categoryIx = -1;

                foreach (var hr in empire.Value.OrderBy(x => x.CreateDT.Value))
                {
                    if (DiffDefReputation)
                    {
                        if (lastScore < 0)
                        {
                            //pierwszy wynik, zaczynam od zera
                            lastScore = hr.Score;
                            scoreValue = 0;
                        }
                        else
                        {
                            scoreValue = hr.Score - lastScore;
                            lastScore = hr.Score;
                        }
                    }
                    else
                        scoreValue = hr.Score;

                    if (scoreValue == 0)
                        continue;

                    var serverTime = hr.CreateDT.Value.AddHours(ServerTimeDiffH);

                    if (FromDate.HasValue && serverTime >= FromDate)
                    {
                        string catName = string.Format("{0} h:{1}", serverTime.ToString("yy.MM.dd"), serverTime.ToString("HH"));
                        categoryIx = m_XCatDates.Labels.IndexOf(catName);

                        if (categoryIx < 0)
                        {
                            m_XCatDates.Labels.Add(catName);
                            categoryIx = m_XCatDates.Labels.IndexOf(catName);
                        }

                        s.Items.Add(new OxyPlot.Series.ColumnItem(scoreValue, categoryIx));
                    }
                }

                m.Series.Add(s);
            }
        }

        private void PlotOffRepBars(PlotModel m)
        {
            if (OffReputations == null)
                return;

            //daty
            if (m_XCatDates == null)
            {
                m_XCatDates = new CategoryAxis();
                m_XCatDates.Title = Labels.Date;
                m_XCatDates.Position = AxisPosition.Bottom;
                m_XCatDates.GapWidth = 0.1;
                m.Axes.Add(m_XCatDates);
            }

            long lastScore = -1;
            double scoreValue = 0;

            foreach (var empire in OffReputations.OrderBy(x => x.Key.OffReputation))
            {
                lastScore = -1;
                scoreValue = 0;
                var s = new OxyPlot.Series.ColumnSeries();
                s.FontSize = 9;
                s.LabelFormatString = "A: " + empire.Key.PlayerName;
                s.Title = string.Format("A: {0} ({1})", empire.Key.PlayerName, empire.Key.AlianceName);
                s.YAxisKey = "reputation";
                s.StackGroup = empire.Key.PlayerName;
                s.IsStacked = true;
                s.LabelPlacement = OxyPlot.Series.LabelPlacement.Inside;
                int categoryIx = -1;

                foreach (var hr in empire.Value.OrderBy(x => x.CreateDT.Value))
                {
                    if (DiffDefReputation)
                    {
                        if (lastScore < 0)
                        {
                            //pierwszy wynik, zaczynam od zera
                            lastScore = hr.Score;
                            scoreValue = 0;
                        }
                        else
                        {
                            scoreValue = hr.Score - lastScore;
                            lastScore = hr.Score;
                        }
                    }
                    else
                        scoreValue = hr.Score;

                    if (scoreValue == 0)
                        continue;

                    var serverTime = hr.CreateDT.Value.AddHours(ServerTimeDiffH);

                    if (FromDate.HasValue && serverTime >= FromDate)
                    {
                        string catName = string.Format("{0} h:{1}", serverTime.ToString("yy.MM.dd"), serverTime.ToString("HH"));
                        categoryIx = m_XCatDates.Labels.IndexOf(catName);

                        if (categoryIx < 0)
                        {
                            m_XCatDates.Labels.Add(catName);
                            categoryIx = m_XCatDates.Labels.IndexOf(catName);
                        }

                        s.Items.Add(new OxyPlot.Series.ColumnItem(scoreValue, categoryIx));
                    }
                }

                m.Series.Add(s);
            }
        }

        private void PlotDefRep(PlotModel m)
        {
            if (DefReputations == null)
                return;

            //miasta
            LinearAxis yAxis = new LinearAxis();
            yAxis.Title = Labels.DefReputation;
            yAxis.Key = "def_rep";
            yAxis.Position = AxisPosition.Left;
            yAxis.MajorGridlineStyle = LineStyle.None;
            yAxis.MinorGridlineStyle = LineStyle.None;
            m.Axes.Add(yAxis);

            foreach (var empire in DefReputations.OrderBy(x => x.Key.DefReputation))
            {
                var s = new OxyPlot.Series.StairStepSeries();
                s.XAxisKey = "x";
                s.YAxisKey = "def_rep";
                //s.VerticalStrokeThickness = 0.2;

                s.MarkerType = MarkerType.Diamond;
                s.Title = string.Format("D: {0} ({1})", empire.Key.PlayerName, empire.Key.AlianceName);

                DateTime? lastTime = null;
                long lastScore = 0;

                foreach (var hr in empire.Value.OrderBy(x => x.CreateDT.Value))
                {
                    s.Points.Add(DateTimeAxis.CreateDataPoint(hr.CreateDT.Value, hr.Score));
                    lastScore = hr.Score;
                    lastTime = hr.CreateDT;
                }

                //sztucznie dodaję pomiar, żeb widać było poziomą kreskę
                if(lastTime.HasValue)
                {
                    s.Points.Add(DateTimeAxis.CreateDataPoint(lastTime.Value.AddHours(1), lastScore));
                }

                m.Series.Add(s);
            }
        }

        private void PlotOffRep(PlotModel m)
        {
            if (OffReputations == null)
                return;

            //miasta
            LinearAxis yAxis = new LinearAxis();
            yAxis.Title = Labels.OffReputation;
            yAxis.Key = "off_rep";
            yAxis.Position = AxisPosition.Right;
            yAxis.MajorGridlineStyle = LineStyle.None;
            yAxis.MinorGridlineStyle = LineStyle.None;
            m.Axes.Add(yAxis);

            foreach (var empire in OffReputations.OrderBy(x => x.Key.OffReputation))
            {
                var s = new OxyPlot.Series.StairStepSeries();
                s.XAxisKey = "x";
                s.YAxisKey = "off_rep";
                s.LineStyle = LineStyle.Dash;
                //s.VerticalStrokeThickness = 0.2;

                s.MarkerType = MarkerType.Diamond;
                s.Title = string.Format("A: {0} ({1})", empire.Key.PlayerName, empire.Key.AlianceName);

                DateTime? lastTime = null;
                long lastScore = 0;

                foreach (var hr in empire.Value.OrderBy(x => x.CreateDT.Value))
                {
                    s.Points.Add(DateTimeAxis.CreateDataPoint(hr.CreateDT.Value, hr.Score));
                    lastScore = hr.Score;
                    lastTime = hr.CreateDT;
                }

                //sztucznie dodaję pomiar, żeb widać było poziomą kreskę
                if (lastTime.HasValue)
                {
                    s.Points.Add(DateTimeAxis.CreateDataPoint(lastTime.Value.AddHours(1), lastScore));
                }

                m.Series.Add(s);
            }
        }
    }
}
