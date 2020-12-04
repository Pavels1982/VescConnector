using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Windows;

namespace VescConnector
{
    public static class SerialPortConnector
    {
        public static  SerialPort port = new SerialPort();

        public delegate void StatusChanged(string message);
        public static event StatusChanged OnStatusChanged;

        public delegate void DataReceived(ByteArray dataPacket);
        public static event DataReceived OnDataReceived;

        public delegate void ValuesReceived(VescModel vesc);
        public static event ValuesReceived OnValuesReceived;
        public static System.Threading.SynchronizationContext Current { get; } = System.Threading.SynchronizationContext.Current;
        public static List<string> GetAvailablePortList()
        {
            return SerialPort.GetPortNames().ToList();
        }

        public static void UpdateVescValues(VescModel vesc)
        {
            OnValuesReceived?.Invoke(vesc);
        }

        public static void Disconnect()
        {
            if (port.IsOpen)
            {
                port.Close();
                SetStatusMessage("Порт закрыт.");
            }
        }

        public static void SetStatusMessage(string message)
        {
            Current.Post(SendToContext, message);
        }

        private static void SendToContext(object obj)
        {
            OnStatusChanged?.Invoke(obj as string);
        }

        public static void Connect(string portName)
        {
            if (!port.IsOpen && portName != null)
            {
                port.PortName = portName;
                port.BaudRate = 115200;
                port.DataBits = 8;
                port.Parity = Parity.None;
                port.StopBits = StopBits.One;
                port.RtsEnable = true;
                port.DataReceived += Port_DataReceived;
                port.ErrorReceived += Port_ErrorReceived;
                try
                {
                    SetStatusMessage("Порт открыт");
                    port.Open();
                }
                catch
                {
                    SetStatusMessage("Порт занят.");
                }
            }
        }

        public static void SendData(ByteArray packet)
        {
            byte[] arr = Packet.GetPacket(packet.data.ToArray());
            if (port.IsOpen)
            {
                port.Write(arr, 0, arr.Length);
            }
        }

        private static void Port_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            SetStatusMessage("Ошибка при получении данных.");
        }

        private static void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            OnDataReceived?.Invoke(new ByteArray(sp.ReadExisting()));
        }


    }
}
