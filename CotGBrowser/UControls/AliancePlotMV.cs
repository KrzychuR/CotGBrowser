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
    public class AliancePlotMV : BaseModelView
    {
        private PlotModel m_PlotM;

        public PlotModel PlotM
        {
            get { return m_PlotM; }
            set { SetProperty(ref m_PlotM, value); }
        }

        private Dictionary<CurrentAlianceRanking, List<AlianceScoreHistory>> m_Aliances;

        /// <summary>
        /// Dane graczy do pokazania, porównania
        /// </summary>
        public Dictionary<CurrentAlianceRanking, List<AlianceScoreHistory>> Aliances
        {
            get { return m_Aliances; }
            set
            {
                if (SetProperty(ref m_Aliances, value))
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
                if (SetProperty(ref m_DiffScore, value))
                {
                    RefreshPlot();
                }
            }
        }

        private bool m_ShowCities;

        /// <summary>
        /// Czy pokazywać miasta
        /// </summary>
        public bool ShowCities
        {
            get { return m_ShowCities; }
            set
            {
                if (SetProperty(ref m_ShowCities, value))
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
                if (SetProperty(ref m_ShowScore, value))
                {
                    RefreshPlot();
                }
            }
        }

        private void RefreshPlot()
        {
            PlotModel m = new PlotModel();

            m.Title = Labels.AlianceScoreAndCities;

            //oś X - z datami, w sumie zawsze :)
            DateTimeAxis xAxis = new DateTimeAxis();
            xAxis.Position = AxisPosition.Bottom;
            xAxis.Key = "x";
            xAxis.StringFormat = "yyyy-MM-dd	";
            xAxis.Angle = -45;
            xAxis.MajorGridlineStyle = LineStyle.Solid;
            xAxis.MinorGridlineStyle = LineStyle.Dot;
            m.Axes.Add(xAxis);

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

            if(ShowScore)
                m.Axes.Add(scoreY);

            //miasta
            LinearAxis citiesY = new LinearAxis();
            citiesY.Title = Labels.Cities;
            citiesY.Key = "cities";
            citiesY.Position = AxisPosition.Right;

            if (ShowCities)
                m.Axes.Add(citiesY);

            foreach (var aliance in Aliances.OrderBy(x => x.Key.Rank))
            {
                long lastScore = -1;
                double scoreValue = 0;
                DateTime lastTime = DateTime.Now;
                double deltaTime = 0;

                OxyPlot.Series.LineSeries ls = new OxyPlot.Series.LineSeries();
                ls.XAxisKey = "x";
                ls.YAxisKey = "score";
                ls.MarkerType = MarkerType.Diamond;
                ls.Title = aliance.Key.AlianceName;

                OxyPlot.Series.StairStepSeries citiesSerie = new OxyPlot.Series.StairStepSeries();
                citiesSerie.XAxisKey = "x";
                citiesSerie.YAxisKey = "cities";
                citiesSerie.VerticalStrokeThickness = 0.2;

                citiesSerie.MarkerType = MarkerType.None;
                citiesSerie.Title = aliance.Key.AlianceName;

                foreach (var hr in aliance.Value.OrderBy(x => x.CreateDT.Value))
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

                    if (ShowCities)
                        citiesSerie.Points.Add(DateTimeAxis.CreateDataPoint(hr.CreateDT.Value, hr.CitiesNo));
                }

                if(ShowScore)
                    m.Series.Add(ls);

                if (ShowCities)
                    m.Series.Add(citiesSerie);
            }
            PlotM = m;
        }
    }
}