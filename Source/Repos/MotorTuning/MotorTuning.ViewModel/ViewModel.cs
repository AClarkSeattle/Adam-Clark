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


namespace MotorTuning.ViewModel
{
    public class ViewModel:ViewModelBase
    {
        private StepPlot _step = new StepPlot();
        private SeriesCollection _seriesCollection;
        public SeriesCollection SeriesCollection { get { return _seriesCollection; } set { _seriesCollection = value; NotifyPropertyChanged(); } }
        private LinearSingleDOF _sys = new LinearSingleDOF();

        public ViewModel()
        {          
            SubscribeNotifyPropertyChanged(ChartPlotterPropertyChanged);
            SubscribeNotifyPropertyChanged(SystemIdentificationPropertyChanged);
            _sys.SetLinearSingleDOF(.06, 6000, .081, .32);
            SeriesCollection = _step.GetPlotSeries(_sys);
        }

        #region Properties and Fields
        private double _c_parameter, _k_parameter, _m_parameter;

        public double c_parameter
        {
            get { return _c_parameter; }
            set { _c_parameter = value;  NotifyPropertyChanged(); }
        }

        public double k_parameter
        {
            get { return _k_parameter; }
            set { _k_parameter = value; NotifyPropertyChanged(); }
        }

        public double m_parameter
        {
            get { return _m_parameter; }
            set { _m_parameter = value; NotifyPropertyChanged(); }
        }
        #endregion

        public virtual void ChartPlotterPropertyChanged( object sender, PropertyChangedEventArgs e)
        {
            var lcl = sender as StepPlot;
            if (lcl == null || e == null) return;
            switch(e.PropertyName)
            {
                case "SeriesCollection":
                    SeriesCollection = lcl.SeriesCollection;
                    break;
                default:
                    break;
            }
        }

        public virtual void SystemIdentificationPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var lcl = sender as LinearSingleDOF;
            if (lcl == null || e == null) return;
            switch (e.PropertyName)
            {
                case "m":
                    m_parameter = lcl.m;
                    c_parameter = lcl.c;
                    k_parameter = lcl.k;
                    break;
                case "c":
                    m_parameter = lcl.m;
                    c_parameter = lcl.c;
                    k_parameter = lcl.k;
                    break;
                case "k":
                    m_parameter = lcl.m;
                    c_parameter = lcl.c;
                    k_parameter = lcl.k;
                    break;
                default:
                    break;
            }
        }

        private void SubscribeNotifyPropertyChanged(PropertyChangedEventHandler SubSysPropertyChanged)
        {
            _step.PropertyChanged += SubSysPropertyChanged;
            _sys.PropertyChanged += SubSysPropertyChanged;
        }

    }
}
