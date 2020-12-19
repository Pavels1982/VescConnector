using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace VescConnector.Services
{
    public class Chart : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        public PlotModel Model { get; set; }

        public List<LineSeries> LineSeries { get; set; } = new List<LineSeries>();

        public Chart()
        {
            this.Model = new PlotModel();
           
            LinearAxis xAxis = new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                MajorGridlineStyle = LineStyle.Solid,

            };
            Model.Axes.Add(xAxis);

            LinearAxis YAxis = new LinearAxis()
            {
                IsAxisVisible = true,
                Position = AxisPosition.Left,
                IsPanEnabled = false,
                //MaximumPadding = 1,
                //Minimum = -1,
                //Maximum = 1,
                MajorStep = 5000,
                MajorGridlineStyle = LineStyle.Solid,
            };
            Model.Axes.Add(YAxis);
            Model.InvalidatePlot(true);

        }


        public void ReInitSeries(ObservableCollection<Vesc> listVesc)
        {
            this.Model.Series.Clear();
            LineSeries.Clear();

            foreach (var vesc in listVesc)
            {
                LineSeries line = new LineSeries();
                Color col = ((SolidColorBrush)vesc.ChartColor).Color;
                line.Color = OxyColor.FromRgb(col.R, col.G, col.B);
                this.LineSeries.Add(line);
                this.Model.Series.Add(this.LineSeries.Last());

            }

            this.Model.InvalidatePlot(true);


        }


    }
}
