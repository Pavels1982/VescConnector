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
using Newtonsoft.Json;
using System.IO;

namespace VescConnector.ViewModels
{
    public class MainWindowViewModel:INotifyPropertyChanged
    {
        public ObservableCollection<Vesc> VescList { get; set; }= new ObservableCollection<Vesc>();
        private DispatcherTimer ChartTimer = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(20) };

        private string dataFileName = "data.bin";

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

        public ICommand ConnectingCommand
        {
            get
            {
                return new RelayCommand<Vesc>((vesc) =>
                {

                        vesc.ConnectingMode();
        
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


        public void SaveData()
        {
            if (VescList != null)
            {
                foreach (var item in VescList)
                {
                    item.Brake();
                    item.SelectedPort = null;
                    item.StatusText = String.Empty;
                    item.SynchVesc = null;
                    item.Disconnect();
                }

                SaveDataTo(VescList, this.dataFileName);


            }
        }



        /// <summary>
        /// Метод сериализации и сохранения объекта в файл.
        /// </summary>
        /// <param name="obj">Сериализуемый объект.</param>
        /// <param name="fileName">Имя файла.</param>
        public static void SaveDataTo(object obj, string fileName)
        {
            try
            {
                using (StreamWriter file = File.CreateText(fileName))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, obj);
                    file.Close();
                }

            }
            catch
            { }

        }

        /// <summary>
        /// Метод десериализации объекта из файла.
        /// </summary>
        /// <param name="fileName">Имя файла.</param>
        public static object ReadDataFrom<T>(string fileName)
        {
            if (File.Exists(fileName))
            {
                JsonSerializer serializer = new JsonSerializer();
                using (StreamReader file = File.OpenText(fileName))
                {
                    return serializer.Deserialize(file, typeof(T));
                }

            }
            return null;
        }


        public MainWindowViewModel()
        {
            ChartTimer.Tick += ChartTimer_Tick;
            ChartTimer.Start();
            if (File.Exists(this.dataFileName))
            {
                VescList = (ObservableCollection<Vesc>)ReadDataFrom<ObservableCollection<Vesc>>(this.dataFileName);
                Chart.ReInitSeries(VescList);
            }

          // var g =  ((ushort)(16191) ^ -1) + 1;


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
                    Chart.LineSeries[i].Points.Add(new OxyPlot.DataPoint(x, Math.Abs( VescList[i].RealTimeData.Rpm)));
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
