using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using VescConnector.Services;

namespace VescConnector.ViewModels
{
    public class MainWindowViewModel:INotifyPropertyChanged
    {
        public ObservableCollection<Vesc> VescList { get; set; }= new ObservableCollection<Vesc>();
        private DispatcherTimer ChartTimer = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(20) };

        public Chart Chart { get; set; } = new Chart();

        private int x { get; set; } = 0;

        public ICommand SetDuty
        {
            get
            {
                return new RelayCommand<Vesc>((vesc) =>
                {
                  
                    vesc.SetDutyCycle(vesc.Duty);
                });
            }
        }

        public ICommand StartForwardDuty
        {
            get
            {
                return new RelayCommand<Vesc>((vesc) =>
                {
                    vesc.StartForwardDutyCycle();
                });
            }
        }
        public ICommand StartReverseDuty
        {
            get
            {
                return new RelayCommand<Vesc>((vesc) =>
                {
                    vesc.StartReverseDutyCycle();
                });
            }
        }
        public ICommand StopDuty
        {
            get
            {
                return new RelayCommand<Vesc>((vesc) =>
                {
                    vesc.StopDutyCycle();
                });
            }
        }

        public ICommand SetRPM
        {
            get
            {
                return new RelayCommand<Vesc>((vesc) => vesc.SetRpm(vesc.RPM));
            }
        }

        public ICommand SetCurrent
        {
            get
            {
                return new RelayCommand<Vesc>((vesc) =>
                {
               
                    vesc.SetCurrent(vesc.Current);
                    });
            }
        }

        public ICommand AddVesc
        {
            get
            {
                return new RelayCommand((o) =>
                {
                    int newId = 0;
                    if (VescList.Count > 0)
                        newId = VescList.OrderBy(vesc => vesc.ID).Last().ID + 1;
                    VescList.Add(new Vesc(newId));

                    Chart.ReInitSeries(VescList);

                });
            }
        }
        public ICommand RemoveVesc
        {
            get
            {
                return new RelayCommand<Vesc>((vesc) =>
                {
                    VescList.Remove(vesc);
                    Chart.ReInitSeries(VescList);
                });
            }
        }

        public ICommand RemoveSyncVesc
        {
            get
            {
                return new RelayCommand<Vesc>((vesc) =>
                {
                    vesc.SynchVesc = null;
                });
            }
        }
        public ICommand RefreshPorts
        {
            get
            {
                return new RelayCommand((o) =>
                {
                    foreach (var vesc in VescList)
                    {
                        vesc.GetAvailablePortList();
                    }
                });
            }
        }

        public ICommand StopVescs
        {
            get
            {
                return new RelayCommand((o) =>
                {

                    foreach (var vesc in VescList)
                    {
                        vesc.Brake();
                    }
                });
            }
        }


        public ICommand BrakeVesc
        {
            get
            {
                return new RelayCommand<Vesc>((vesc) =>
                {

                        vesc.Brake();
                });
            }
        }


        public ICommand ConnectPort
        {
            get
            {
                return new RelayCommand<Vesc>((vesc) =>
                {
                    vesc.Connect();
                });
            }
        }
        public ICommand DisconnectPort
        {
            get
            {
                return new RelayCommand<Vesc>((vesc) =>
                {
                    vesc.Disconnect();
                });
            }
        }

        public MainWindowViewModel()
        {
            ChartTimer.Tick += ChartTimer_Tick;
            ChartTimer.Start();

            //double duty1 = -0.1d * 1e3;

            //byte[] str = BitConverter.GetBytes(duty1);

            //Int64 res = (((Int64)str[7] << 56 ) | ((Int64)str[6]) << 48);

            //byte[] y = BitConverter.GetBytes(res);

            //double total = (double)(BitConverter.Int64BitsToDouble(res) / 1e3);

            //byte[] str3 = BitConverter.GetBytes(total);

        }

        private void ChartTimer_Tick(object sender, EventArgs e)
        {
            if (Chart.Model.Series.Count == 0) return;

            for (int i = 0; i < VescList.Count  ; i++)
            {
                Chart.LineSeries[i].Points.Add(new OxyPlot.DataPoint(x, VescList[i].RealTimeData.Rpm));
            }

            if (Chart.LineSeries[0].Points.Count > 200)
            {

                for (int i = 0; i < VescList.Count ; i++)
                {                  
                    Chart.LineSeries[i].Points.RemoveAt(0);
                }

            }
            Chart.Model.InvalidatePlot(true);

            x++;
          
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
