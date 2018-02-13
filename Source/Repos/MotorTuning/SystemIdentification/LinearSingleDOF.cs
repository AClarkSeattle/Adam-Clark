using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace SystemIdentification
{
    public class LinearSingleDOF : INotifyPropertyChanged
    {
        public LinearSingleDOF()
        {

        }

        public void SetLinearSingleDOF(double SteadyStateResponse, double MagnitudeOfStepInput, double PeakOverShoot, double TimeAtPlotPeak)
        {
            xss = SteadyStateResponse;
            fss = MagnitudeOfStepInput;
            PlotPeakValue = PeakOverShoot;
            tp = TimeAtPlotPeak;
            UpdateValues();
        }

        private void UpdateValues()
        {
            k = xss!=0?fss /xss:0;
            m = wn!=Double.NaN? k / Pow(wn, 2):0;
            c = 2 * DampingRatio * Sqrt(m * k);
        }

        private double _xss;
        /// <summary>
        /// Steady State Response
        /// </summary>
        public double xss
        {
            get { return _xss; }
            set { _xss = value;}
        }

        private double _fss;
        /// <summary>
        /// Magnitude of Step Input
        /// </summary>
        public double fss
        {
            get { return _fss; }
            set { _fss = value;}
        }

        private double _k;
        /// <summary>
        /// Model Parameter Corresponding to Integral Gain
        /// </summary>
        public double k
        {
            get { return _k; }
            set { _k= value; NotifyPropertyChanged(); }
        }

        private double _plotPeakValue;
        /// <summary>
        /// Maximum Value of Step Response Overshoot
        /// </summary>
        public double PlotPeakValue
        {
            get { return _plotPeakValue; }
            set { _plotPeakValue = value; }
        }

        public double MaxPercentOvershoot
        {
            get { return 100 * ((PlotPeakValue - xss) / xss); }
        }

        private double A
        { get { return Log(100 / MaxPercentOvershoot); } }

        public double DampingRatio
        { get { return (A / Sqrt(Pow(PI, 2) + Pow(A, 2))); } }

        private double _tp;
        /// <summary>
        /// Time at Plot Peak
        /// </summary>
        public double tp
        {
            get { return _tp; }
            set { _tp = value; }
        }

        /// <summary>
        /// Natural Frequency
        /// </summary>
        public double wn
        { get { return PI / (tp * Sqrt(1 - Pow(DampingRatio, 2))); } }

        private double _m;
        /// <summary>
        /// Model Parameter Corresponding to Derivative Gain
        /// </summary>
        public double m
        {
            get { return _m; }
            set{_m =value; NotifyPropertyChanged();}
        }

        private double _c;
        /// <summary>
        /// Model Parameter Corresponding to Proportional Gain
        /// </summary>
        public double c
        {
            get { return _c; }
            set{ _c=value; NotifyPropertyChanged(); }
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
