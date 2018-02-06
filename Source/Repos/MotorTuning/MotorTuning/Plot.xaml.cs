using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using static System.Math;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SystemIdentification;

namespace MotorTuningWPF
{
    /// <summary>
    /// Interaction logic for Plot.xaml
    /// </summary>
    public partial class Plot : UserControl, INotifyPropertyChanged
    {
        SeriesCollection series = new SeriesCollection();
        private LinearSingleDOF _sys = new LinearSingleDOF(.06, 6000, .081, .32);
        ChartValues<ObservablePoint> plotdata;

        #region Private Fields
        private double _m;
        private double _c;
        private double _k;
        private bool initialUpdate = true;
        #endregion

        #region Public Properties
        public double m { get { return _m; } set { _m = value; NotifyPropertyChanged(); } }
        public double c { get { return _c; } set { _c = value; NotifyPropertyChanged(); } }
        public double k { get { return _k; } set { _k = value; NotifyPropertyChanged(); } }
        #endregion

        #region NotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            if (!initialUpdate)
            {
                plotdata = GetPlotData(.01, 2, m, k, c);
                UpdatePlot();
            }
        }
        #endregion

        public Plot()
        {
            InitializeComponent();

            UpdateInitialParameters();

            UpdatePlot();
            this.LayoutRoot.DataContext = this;
        }

        public void UpdatePlot()
        {
            plotdata = GetPlotData(.01, 2, m, k, c);
            if (series.Chart != null) series.Clear();
            series.Add(new LineSeries
            {
                Values = plotdata,
                PointGeometrySize = 1
            });
        }

        public ChartValues<ObservablePoint> GetPlotData(double res, double range, double m, double k, double c)
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

        void UpdateInitialParameters()
        {
            c = _sys.c;
            k = _sys.k;
            m = _sys.m;
            initialUpdate = false;
        }

        private SeriesCollection seriesCollection;
        public SeriesCollection SeriesCollection { get { return series; } set { seriesCollection = series; } }

    }


}