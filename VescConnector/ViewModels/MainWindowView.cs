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
