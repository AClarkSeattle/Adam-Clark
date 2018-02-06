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
        public LinearSingleDOF(double SteadyStateResponse, double MagnitudeOfStepInput, double PeakOverShoot, double TimeAtPlotPeak)
        {
            xss = SteadyStateResponse;
            fss = MagnitudeOfStepInput;
            PlotPeakValue = PeakOverShoot;
            tp = TimeAtPlotPeak;
        }

        private double _xss;
        /// <summary>
        /// Steady State Response
        /// </summary>
        public double xss
        {
            get { return _xss; }
            set { _xss = value; }
        }

        private double _fss;
        /// <summary>
        /// Magnitude of Step Input
        /// </summary>
        public double fss
        {
            get { return _fss; }
            set { _fss = value; }
        }

        /// <summary>
        /// Model Parameter Corresponding to Integral Gain
        /// </summary>
        public double k
        {
            get { return fss / xss; }
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

        /// <summary>
        /// Model Parameter Corresponding to Derivative Gain
        /// </summary>
        public double m
        { get { return k / Pow(wn, 2); } }
        /// <summary>
        /// Model Parameter Corresponding to Proportional Gain
        /// </summary>
        public double c
        { get { return 2 * DampingRatio * Sqrt(m * k); } }

        #region NotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
