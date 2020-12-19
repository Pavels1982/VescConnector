using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace VescConnector.ViewModels
{
    public class MainWindowViewModel:INotifyPropertyChanged
    {
        public ObservableCollection<Vesc> VescList { get; set; }= new ObservableCollection<Vesc>();

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
            //double duty1 = -0.1d * 1e3;


            //byte[] str = BitConverter.GetBytes(duty1);


            //Int64 res = (((Int64)str[7] << 56 ) | ((Int64)str[6]) << 48);
           
            //byte[] y = BitConverter.GetBytes(res);

            //double total = (double)(BitConverter.Int64BitsToDouble(res) / 1e3);

            //byte[] str3 = BitConverter.GetBytes(total);
           
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
