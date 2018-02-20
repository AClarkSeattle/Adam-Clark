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
using MathNet.Numerics.Interpolation;
using HelperLibrary;


namespace ChartPlotter
{
    public class SplinePlot:AbstractChartPlotterClass,INotifyPropertyChanged
    {

        public double[] xdata = { 0, 1, 1.5, 2.25 };
        public double[] ydata = { 2, 4.4366, 6.7134, 13.9130 };

        private double[] dataptArray = new double[4];
        private double[] c0 = new double[3];
        private double[] c1 = new double[3];
        private double[] c2 = new double[3];
        private double[] c3 = new double[3];

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

        public SeriesCollection GetPlotSeries()
        {
            plotdata = GetPlotData();
            if (Series.Chart != null) Series.Clear();
            Series.Add(new LineSeries
            {
                Values = plotdata,
                PointGeometrySize = 1
            });
            return Series;
        }

        private ChartValues<ObservablePoint> GetPlotData()
        {
            ChartValues<ObservablePoint> chartdata = new ChartValues<ObservablePoint>();
            CubicSpline cs = new CubicSpline(dataptArray, c0, c1, c2, c3);
            cs = CubicSpline.InterpolateNatural(xdata, ydata);
            c0 = HelperLibrary.PropertyHelper.GetPrivateFieldValue<double[]>(cs, "_c0");
            c1 = HelperLibrary.PropertyHelper.GetPrivateFieldValue<double[]>(cs, "_c1");
            c2 = HelperLibrary.PropertyHelper.GetPrivateFieldValue<double[]>(cs, "_c2");
            c3 = HelperLibrary.PropertyHelper.GetPrivateFieldValue<double[]>(cs, "_c3");

            for(int i=0;i<c0.Length;i++)
            {
                List<double[]> tmp = new List<double[]>();
                tmp = CubicIntervalData(xdata[i], xdata[i+1], c3[i], c2[i], c1[i], c0[i]);
                foreach(double[] pt in tmp)
                {
                    chartdata.Add(new ObservablePoint(pt[0], pt[1]));
                }
            }

            return chartdata;
        }

        private List<double[]> CubicIntervalData(double x0, double x1, double a, double b, double c, double d, double res=.01)
        {
            List<double[]> output = new List<double[]>();

            for(double i=x0;i<x1;i=i+res)
            {
                double[] element = new double[2];
                element[0] = i;
                element[1] = a * Pow((i - x0), 3) + b * Pow((i - x0), 2) + c * Pow((i - x0), 1) + d;
                output.Add(element);
            }

            return output;
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
