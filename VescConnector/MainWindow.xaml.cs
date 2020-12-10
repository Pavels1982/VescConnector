using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO.Ports;
using System.Threading;
using System.Diagnostics;

namespace VescConnector
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public float Duty { get; set; }

        Vesc vesc1 { get; set; } = new Vesc();

        public MainWindow()
        {
           InitializeComponent();
            this.DataContext = this;
        }

        private void SerialPortConnector_OnStatusChanged(string message)
        {
            Status.Text += message + System.Environment.NewLine;
        }

        private void addReceiveTextToTextBlock(object text)
        {
            Status.Text += (text as string) + System.Environment.NewLine;
        }



        private void PortList_DropDownOpened(object sender, EventArgs e)
        {
            PortList.Items.Clear();
            foreach (var item in vesc1.GetAvailablePortList())
            {
                PortList.Items.Add(item);
            }
        }


        private void ConnectPort_Click(object sender, RoutedEventArgs e)
        {
            vesc1.Connect(PortList.SelectedValue as string);
        }

        private void SendDutyButton_Click(object sender, RoutedEventArgs e)
        {
            //   SerialPortConnector.SendData(Commands.SetDutyCycle(Duty));
        }

        private void GetVersion_Click(object sender, RoutedEventArgs e)
        {
            vesc1.GetFwVersion();
        }

        private void DisconnectPort_Click(object sender, RoutedEventArgs e)
        {
            vesc1.Disconnect();
        }
    }




}
