using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using static System.Math;
using SystemIdentification;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ChartPlotter
{
    /// <summary>
    /// Methods for plotting a step function in the time domain.
    /// </summary>
    public class StepPlot: AbstractChartPlotterClass, INotifyPropertyChanged
    {
        #region Properties
        private SeriesCollection _seriesCollection;

        public SeriesCollection SeriesCollection 
        {
            get { return _seriesCollection; }
            private set { _seriesCollection = value; }
        }

        private SeriesCollection Series = new SeriesCollection();
        /// <summary>
        /// Plot Data is Available in Pt Form for Easier Export to .txt
        /// </summary>
        public ChartValues<ObservablePoint> plotdata { get; private set; }
  
        #endregion

        public SeriesCollection GetPlotSeries(LinearSingleDOF System, double res = .01, double range = 2)
        {

            plotdata = GetPlotData(System.m, System.k, System.c);
            if (Series.Chart != null) Series.Clear();
            Series.Add(new LineSeries
            {
                Values = plotdata,
                PointGeometrySize = 1
            });
            return Series;
        }
        
        private ChartValues<ObservablePoint> GetPlotData(double m, double k, double c, double res=.01, double range=2)
        {
            double w = Sqrt(k / m);//natural frequency
            double j = c / (2 * Sqrt(m * k)); //damping ratio
            double z = Sqrt(1 - Pow(j, 2));
            double q = w * j;
            double M = 6;

            ChartValues<ObservablePoint> chartdata = new ChartValues<ObservablePoint>();

            chartdata.Add(new ObservablePoint(0, 0));
            for (double x = .01; x < range - res; x += .01)
            {
                http://lpsa.swarthmore.edu/LaplaceZTable/LaplaceZFuncTable.html
                double y = M * (1 - (1 / z) * Pow(E, -w * j * x) * (Sin(w * z * x + Acos(j))));

                chartdata.Add(new ObservablePoint(x, y));
            }
            return chartdata;
        }

        #region NotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
