using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Windows;
using static VescConnector.DataTypes;
using System.Windows.Threading;
using VescConnector;
using System.ComponentModel;

namespace VescConnector
{
    public class Vesc: INotifyPropertyChanged
    {
        private SerialPort port = new SerialPort();
        private System.Threading.SynchronizationContext CurrentContext { get; } = System.Threading.SynchronizationContext.Current;
        public string StatusText { get; set; } = String.Empty;
        private DispatcherTimer realDataTimer = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(1) };

        public event PropertyChangedEventHandler PropertyChanged;

        public string SelectedPort { get; set; }

        public RealTimeData RealTimeData { get; set; }

        public VescInfo Info { get; set; }

        public List<string> PortList { get; set; }

        public double Duty { get; set; }

        public int RPM { get; set; }

        public double Current { get; set; }
        public bool IsRealTimeData { get; set; }

        private ByteArray lastPacket;

        public void GetAvailablePortList()
        {
            this.PortList.Clear();
            this.PortList = SerialPort.GetPortNames().ToList();
        }

        public Vesc()
        {
            this.RealTimeData = new RealTimeData();
            this.Info = new VescInfo();
            this.PortList = new List<string>();
            GetAvailablePortList();
            realDataTimer.Tick += RealDataTimer_Tick;
            realDataTimer.Start();
        }

        private void RealDataTimer_Tick(object sender, EventArgs e)
        {
            if (IsRealTimeData) GetValues();
            if (lastPacket != null) sendCommand(lastPacket);
        }

        //private void RealtimeDataOn()
        //{
        //    if (!realDataTimer.IsEnabled) realDataTimer.Start();
        //}

        //private void RealtimeDataOff()
        //{
        //    if (realDataTimer.IsEnabled) realDataTimer.Stop();
        // }

        public void Disconnect()
        {
            if (port.IsOpen)
            {
                port.Close();
                SetStatusMessage("Порт закрыт.");
            }
        }

        public void SetStatusMessage(string message)
        {
            CurrentContext.Post(SendToContext, message);
        }

        private void SendToContext(object obj)
        {
            StatusText = obj as string;
        }

        public void Connect()
        {
            string portName = this.SelectedPort;
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
                    if (port.IsOpen) GetFwVersion();
                }
                catch
                {
                    SetStatusMessage("Порт занят.");
                }
            }
        }

        public void SendData(ByteArray packet)
        {
            byte[] arr = Packet.Create(packet.data.ToArray());
            if (port.IsOpen)
            {
                port.Write(arr, 0, arr.Length);
            }
        }

        private void Port_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            SetStatusMessage("Ошибка при получении данных.");
        }

        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            ProcessPacket(new ByteArray(sp.ReadExisting()));
        }


        public void ProcessPacket(ByteArray packet)
        {
            if (packet.data.Count <= 5) return;
            packet.data.RemoveRange(0, 2);
            packet.data.RemoveRange(packet.data.Count - 1 - 2, 3);
            COMM_PACKET_ID id = (COMM_PACKET_ID)packet.PopFrontUInt8();

            switch (id)
            {
                case COMM_PACKET_ID.COMM_FW_VERSION:
                    {
                        int fw_major = 0;
                        int fw_minor = 0;
                        string hw = String.Empty;

                        if (packet.data.Count >= 2)
                        {
                            fw_major = packet.PopFrontInt8();
                            fw_minor = packet.PopFrontInt8();
                            hw = packet.PopFrontString();
                        }
                        SetStatusMessage(string.Format("Firmware Version:{0}.{1} UUID:{2} ", fw_major, fw_minor, hw));
                        this.Info.Hw = hw;
                    }
                    break;

                case COMM_PACKET_ID.COMM_GET_VALUES:
                    {
                        RealTimeData values = new RealTimeData(); ;
                       // UInt32 mask = (uint)(0xFFFFFFFF);
                                       
                        values.Temp_mos = packet.PopFrontDouble16(1e1);
                        values.Temp_motor = packet.PopFrontDouble16(1e1);
                        values.Current_motor = packet.PopFrontDouble32(1e2);
                        values.Current_in = packet.PopFrontDouble32(1e2);
                        values.id = packet.PopFrontDouble32(1e2);
                        values.iq = packet.PopFrontDouble32(1e2);
                        values.Duty_now = packet.PopFrontDouble16(1e3);
                        values.Rpm = packet.PopFrontDouble32(1e0);
                        values.V_in = packet.PopFrontDouble16(1e1) * values.Current_in;
                        values.Amp_hours = packet.PopFrontDouble32(1e4);
                        values.Amp_hours_charged = packet.PopFrontDouble32(1e4);
                        values.Watt_hours = packet.PopFrontDouble32(1e4);
                        values.Watt_hours_charged = packet.PopFrontDouble32(1e4);
                        values.Tachometer = packet.PopFrontInt32();
                        values.Tachometer_abs = packet.PopFrontInt32();

                        values.Fault_code = packet.PopFrontInt8();
                        values.Fault_str = values.Fault_code.ToString();

                        if (packet.Size() >= 4)
                        {
                            values.Position = packet.PopFrontDouble32(1e6);
                        }
                        else
                        {
                            values.Position = -1.0;
                        }

                        if (packet.Size() >= 1)
                        {
                            values.Vesc_id = packet.PopFrontInt8();
                        }
                        else
                        {
                            values.Vesc_id = 255;
                        }

                        if (packet.Size() >= 6)
                        {
                            values.Temp_mos_1 = packet.PopFrontDouble16(1e1);
                            values.Temp_mos_2 = packet.PopFrontDouble16(1e1);
                            values.Temp_mos_3 = packet.PopFrontDouble16(1e1);
                        }

                        if (packet.Size() >= 8)
                        {
                            values.vd = packet.PopFrontDouble16(1e3);
                            values.vq = packet.PopFrontDouble16(1e3);

                        }
                        RealTimeData = values;
                    }
                    break;
            }
        }

        private void sendCommand(ByteArray packet)
        {
            SendData(packet);
        }

        public void SetDutyCycle(double dutyCycle)
        {
            ByteArray arr = new ByteArray();
            arr.AppendInt8((byte)COMM_PACKET_ID.COMM_SET_DUTY);
            arr.AppendDouble32(dutyCycle, 1e5);
            lastPacket = arr;
            // sendCommand(arr);
        }

        public void SetRpm(int rpm)
        {
            ByteArray arr = new ByteArray();
            arr.AppendInt8((byte)COMM_PACKET_ID.COMM_SET_RPM);
            arr.AppendInt32(rpm);
            lastPacket = arr;
          //  sendCommand(arr);
        }

        public void GetFwVersion()
        {
            ByteArray arr = new ByteArray();
            arr.AppendInt8((byte)COMM_PACKET_ID.COMM_FW_VERSION);
            sendCommand(arr);
        }

        private void GetValues()
        {
            ByteArray arr = new ByteArray();
            arr.AppendInt8((byte)COMM_PACKET_ID.COMM_GET_VALUES);
            sendCommand(arr);
        }


        public void SetCurrent(double current)
        {
            ByteArray arr = new ByteArray();
            arr.AppendInt8((byte)COMM_PACKET_ID.COMM_SET_CURRENT);
            arr.AppendDouble32(current,1e3);
            lastPacket = arr;
            // sendCommand(arr);
        }

        public void Brake()
        {
            SetCurrent(0);
            //SetDutyCycle(0);
        }
    }
}
