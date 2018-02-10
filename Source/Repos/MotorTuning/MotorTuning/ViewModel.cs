using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using ChartPlotter;
using SystemIdentification;
using static System.Math;


namespace MotorTuningWPF
{
    //This class is not used.
    public class ViewModel:INotifyPropertyChanged
    {
        private StepPlot step = new StepPlot();
        private SeriesCollection _seriesCollection;
        public SeriesCollection SeriesCollection { get { return _seriesCollection; } set { _seriesCollection = value; NotifyPropertyChanged(); } }
        private LinearSingleDOF _sys = new LinearSingleDOF(.06, 6000, .081, .32 );
        
        public ViewModel()
        {
            
            UpdateParameters();
            SeriesCollection = step.GetPlotSeries(_sys);
        }
         
        #region NotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName="")
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Properties and Fields
        private double _c_parameter, _k_parameter, _m_parameter;

        public double c_parameter
        {
            get { return _c_parameter; }
            set { _c_parameter = value; c = String.Format("{0:0.##}", _c_parameter); }
        }

        public double k_parameter
        {
            get { return _k_parameter; }
            set { _k_parameter = value; k = String.Format("{0:0.##}", _k_parameter); }
        }

        public double m_parameter
        {
            get { return _m_parameter; }
            set { _m_parameter = value; m = String.Format("{0:0.##}", _m_parameter); }
        }

        private string _c = "c";
        public string c
        {
            get { return _c; }
            set { _c = "c = "+ value; NotifyPropertyChanged(); }
        }

        private string _k="k";
        public string k
        {
            get { return _k; }
            set { _k = "k = " + value; NotifyPropertyChanged(); }
        }

        private string _m="m";
        public string m
        {
            get { return _m; }
            set { _m = "m = " + value; NotifyPropertyChanged(); }
        }
        #endregion

        void UpdateParameters()
        {
            c_parameter = _sys.c;
            k_parameter = _sys.k;
            m_parameter = _sys.m;
        }

    }
}
