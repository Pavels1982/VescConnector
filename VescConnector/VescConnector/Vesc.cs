﻿using System;
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
using System.Diagnostics;
using System.Windows.Media;
using System.IO;
using System.Threading;
using Newtonsoft.Json;

namespace VescConnector
{
    public class Vesc : INotifyPropertyChanged
    {
        public int ID { get; set; }
        public bool NegativeRotation { get; set; }
        public string Name => string.Format("VESC ID: {0}", ID);
        private double lastRpm { get; set; }
        public bool IsConnected;
        public bool isIncrease { get; set; }

        private SerialPort port = new SerialPort();
        private System.Threading.SynchronizationContext CurrentContext { get; } = System.Threading.SynchronizationContext.Current;
        public string StatusText { get; set; } = String.Empty;

        private DispatcherTimer realDataTimer = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(5) };

        private DispatcherTimer DutyTimer = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(5) };

        public event PropertyChangedEventHandler PropertyChanged;

        public double Delta { get; set; }


        private List<Brush> Color = new List<Brush>()
        {
            Brushes.Red,
            Brushes.Blue,
            Brushes.Green,
            Brushes.Brown,
            Brushes.Black
        };

        public Brush ChartColor { get; private set; }
        public string SelectedPort { get; set; }

        private double currentDuty;

        public RealTimeData RealTimeData { get; set; }

        public VescInfo Info { get; set; }

        public List<string> PortList { get; set; }

        public double Duty { get; set; }

        public int RPM { get; set; }

        public double Current { get; set; }

        public bool IsRealTimeData { get; set; } = true;

        private ByteArray lastPacket;

        public void GetAvailablePortList()
        {
            this.PortList.Clear();
            this.PortList = SerialPort.GetPortNames().ToList();
        }
        private Stopwatch timeWatcher = new Stopwatch();
        private double deltaTime = 0;
        public Vesc SynchVesc { get; set; }

        public Vesc(int id)
        {
            this.ID = id;
            this.RealTimeData = new RealTimeData();
            this.Info = new VescInfo();
            this.PortList = new List<string>();
            GetAvailablePortList();
            realDataTimer.Tick += RealDataTimer_Tick;
            realDataTimer.Start();
            timeWatcher.Start();
            DutyTimer.Tick += DutyTimer_Tick;
            if (id > Color.Count-1) id = 0;
            ChartColor = Color[id];
        }

        private bool isConnectionMode;

        public void ConnectingMode()
        {
            if (!this.isConnectionMode && Math.Abs(RealTimeData.Duty_now) < 0.04d)
            {
                this.isConnectionMode = true;
                Task.Factory.StartNew(() =>
                {

                    for (int i = 0; i < 6; i++)
                    {
                        if (RealTimeData.Duty_now <= 0) SetDutyCycle(0.03d); else SetDutyCycle(-0.03d);
                        sendCommand(lastPacket);
                        Thread.Sleep(280);
                    }
                    SetDutyCycle(0.0d);
                    sendCommand(lastPacket);
                    this.isConnectionMode = false;
                });
            }
        }


        private void RealDataTimer_Tick(object sender, EventArgs e)
        {
             deltaTime = 0.005d / realDataTimer.Interval.TotalMilliseconds;
            if (IsRealTimeData) GetValues();

            if (lastPacket != null && SynchVesc == null)
            {


                if (Duty != 0 && !isConnectionMode)
                {
                    currentDuty += currentDuty < 0.0 ? deltaTime : -deltaTime;

                    if (Math.Abs(RealTimeData.Duty_now) == 0.001d) currentDuty = 0;

                    SetDutyCycle(currentDuty);
                    sendCommand(lastPacket);
                }


            }
            else if (SynchVesc != null)
            {

                if (SynchVesc.IsRealTimeData && SynchVesc.IsConnected)
                {
                    if (SynchVesc.RealTimeData.Duty_now != 0)
                    {
                      // double oneRateInDuty = 0.000545d;
                        Delta = Math.Abs(Math.Abs(SynchVesc.RealTimeData.Rpm) - Math.Abs(RealTimeData.Rpm));

                        // double duty = -(SynchVesc.RealTimeData.Rpm) * rp;

                        double offSet = isIncrease ? 0 : (SynchVesc.RealTimeData.Duty_now / 100d * 1.3d);

                       // double duty = -(SynchVesc.RealTimeData.Duty_now - (SynchVesc.RealTimeData.Duty_now / 100d * 1.3d));
                     //   double duty = -(SynchVesc.RealTimeData.Duty_now - offSet);
                      
                           double duty = -(SynchVesc.RealTimeData.Duty_now);

                        SetDutyCycle(duty);
                        sendCommand(lastPacket);
                       
                    }
                }
            }
            Duty = currentDuty;
        }


        public void Disconnect()
        {
            if (port.IsOpen)
            {
                port.Close();
                SetStatusMessage("Порт закрыт.");
                IsConnected = false;
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
               // port.RtsEnable = true;
                port.Encoding = Encoding.UTF8;
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
                    IsConnected = false;
                }
            }
        }

        public void SendData(ByteArray packet)
        {
            try
            {
                if (packet != null)
                {
                    byte[] arr = Packet.Create(packet.data.ToArray());
                    if (port.IsOpen)
                    {
                        port.Write(arr, 0, arr.Length);
                    }
                }
            }
            catch { }
        }

        private void Port_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            SetStatusMessage("Ошибка при получении данных.");
        }

        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            // ProcessPacket(new ByteArray(sp.));
         //   ProcessPacket(new ByteArray(sp.ReadExisting()));

            int bytes = port.BytesToRead;
            byte[] buffer = new byte[bytes];
            port.Read(buffer, 0, bytes);

            ProcessPacket(new ByteArray(buffer));
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
                            hw = packet.PopFrontUInt16().ToString();
                            // hw = packet.PopFrontString();
                            IsConnected = true;
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

                        values.Rpm = packet.PopFrontDouble32(1e0) / 11;

                        double h = Math.Abs(values.Rpm);

                        if (h > lastRpm)  isIncrease = true;  else  isIncrease = false;
                        lastRpm = h;

                        //if (SynchVesc == null && values.Duty_now > 0.02)

                        //    using (StreamWriter sw = new StreamWriter("main.csv", true, System.Text.Encoding.Default))
                        //    {
                        //        sw.WriteLine("{0};", values.Duty_now);
                        //    };

                        //if (SynchVesc != null && values.Duty_now > 0.02)

                        //    using (StreamWriter sw = new StreamWriter("second.csv", true, System.Text.Encoding.Default))
                        //    {
                        //        sw.WriteLine("{0};", values.Duty_now);
                        //    };

                        // Debug.WriteLine(lastRpm);

                        //  if (values.Rpm > 10000) values.Rpm = 0;
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
                      //  lastRpm = RealTimeData.Rpm;
                        RealTimeData = values;
                    }
                    break;
            }
        }

        private void sendCommand(ByteArray packet)
        {
            SendData(packet);
        }
        public void StartForwardDutyCycle()
        {
            this.NegativeRotation = false;
            DutyTimer.Start();
        }
        public void StartReverseDutyCycle()
        {
            this.NegativeRotation = true;
            DutyTimer.Start();
        }

        public void StopDutyCycle()
        {
            DutyTimer.Stop();
        }

        private void DutyTimer_Tick(object sender, EventArgs e)
        {
              currentDuty = NegativeRotation && Math.Abs(currentDuty) < 0.7d  ? currentDuty - (0.02d / DutyTimer.Interval.TotalMilliseconds) : currentDuty + (0.02d / DutyTimer.Interval.TotalMilliseconds);
           // currentDuty = NegativeRotation?  -0.04d : 0.04d;


            ByteArray arr = new ByteArray();
            arr.AppendInt8((byte)COMM_PACKET_ID.COMM_SET_DUTY);
            arr.AppendDouble32(currentDuty, 1e5);
            lastPacket = arr;
            Duty = currentDuty;
        }



        public void SetDutyCycle(double dutyCycle)
        {
            if (Math.Abs(dutyCycle) > 0.7d)   return; 
           //  currentDuty = dutyCycle < 0.0 ? currentDuty - (1d / deltaTime) : currentDuty + (1d / deltaTime);
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
            sendCommand(arr);
            lastPacket = null;
        }

        public void Brake()
        {
            SetCurrent(0);
            currentDuty = 0;
            //SetDutyCycle(0);
        }


    }
}
