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
                return new RelayCommand<Vesc>((vesc) => vesc.SetDutyCycle(vesc.Duty));
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
                return new RelayCommand<Vesc>((vesc) => vesc.SetCurrent(vesc.Current));
            }
        }

        public ICommand AddVesc
        {
            get
            {
                return new RelayCommand((o) =>
                {
                    VescList.Add(new Vesc());

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
            
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
