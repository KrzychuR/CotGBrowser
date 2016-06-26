using CotGBrowser.Common;
using GotGLib.DTO;
using GotGLib.Res;
using OxyPlot;
using OxyPlot.Axes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CotGBrowser.UControls
{
    public class WorldAliancesMV : BaseModelView
    {
        private PlotModel m_PlotM;

        public PlotModel PlotM
        {
            get { return m_PlotM; }
            set { SetProperty(ref m_PlotM, value); }
        }

        private List<CurrentAlianceRanking> m_AlianceRankings;

        /// <summary>
        /// Wszystkie (odfiltrowane) rankingi sojuszu
        /// </summary>
        public List<CurrentAlianceRanking> AlianceRankings
        {
            get { return m_AlianceRankings; }
            set
            {
                if (SetProperty(ref m_AlianceRankings, value))
                {
                    RefreshPlot();
                }
            }
        }

        private void RefreshPlot()
        {
            PlotModel m = new PlotModel();

            m.Title = Labels.WorldAlianceScore;

            //punktacja
            LinearAxis scoreY = new LinearAxis();
            scoreY.Key = "score";
            scoreY.Title = Labels.Score;
            scoreY.Position = AxisPosition.Left;
            scoreY.MajorGridlineStyle = LineStyle.Solid;
            scoreY.MinorGridlineStyle = LineStyle.Dot;
            m.Axes.Add(scoreY);

            //kontynenty
            var continents = new CategoryAxis();
            continents.GapWidth = 0.1;
            continents.Position = AxisPosition.Bottom;
            continents.Title = Labels.Continents;

            m.Axes.Add(continents);
            int categoryIx = -1;

            foreach (var aliance in AlianceRankings.GroupBy(x => x.AlianceName))
            {
                OxyPlot.Series.ColumnSeries cs = new OxyPlot.Series.ColumnSeries();
                cs.Title = aliance.Key;

                foreach(var continent in aliance.OrderBy(x => x.Continent))
                {
                    categoryIx = continents.Labels.IndexOf(continent.Continent.ToString());

                    if (categoryIx < 0)
                    {
                        continents.Labels.Add(continent.Continent.ToString());
                        categoryIx = continents.Labels.IndexOf(continent.Continent.ToString());
                    }

                    cs.Items.Add(new OxyPlot.Series.ColumnItem(continent.Score, categoryIx));
                }

                m.Series.Add(cs);
            }

            PlotM = m;
        }
    }
}
