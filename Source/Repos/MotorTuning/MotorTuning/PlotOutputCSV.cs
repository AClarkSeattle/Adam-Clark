using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace MotorTuningWPF
{
    public class PlotOutputCSV
    {
        string outputpath = @"C:\Users\Adam Clark\Source\Repos\MotorTuning\Log";
        public bool LogPlotCSV(ChartValues<ObservablePoint> plotdata)
        {
            System.IO.File.WriteAllText(@outputpath + @"\step.csv", "");
            StreamWriter thedatastream = new StreamWriter(@outputpath + @"\step.csv", true);
            {
                thedatastream.WriteLine("\n");
                foreach(ObservablePoint p in plotdata)
                {
                    thedatastream.WriteLine(String.Format("{0:0.##}", p.X) + "," + String.Format("{0:0.##}", p.Y));
                }
            }
            return true;
        }
    }
}
