using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static VescConnector.DataTypes;

namespace VescConnector
{
    public static class Commands
    {


        public static void ProcessPacket(ByteArray packet)
        {
            if (packet.data.Count <= 5) return;
            packet.data.RemoveRange(0, 2);
            packet.data.RemoveRange(packet.data.Count - 1 - 2, 3);
            COMM_PACKET_ID id = (COMM_PACKET_ID)packet.PopFrontUInt8();

            switch (id)
            {
                case COMM_PACKET_ID.COMM_FW_VERSION_REMOTION:
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
                        SerialPortConnector.SetStatusMessage(string.Format("Firmware Version:{0}.{1} UUID:{2} ", fw_major, fw_minor, hw));

                    }
                    break;

                case COMM_PACKET_ID.COMM_GET_VALUES:
                    {
                        VescModel values = new VescModel(); ;
                        values.Temp_mos = packet.PopFrontDouble16(1e1);
                        values.Temp_motor = packet.PopFrontDouble16(1e1);
                        values.Current_motor = packet.PopFrontDouble32(1e2);
                        values.Current_in = packet.PopFrontDouble32(1e2);
                        values.Encoder_in = (double)packet.PopFrontInt32() / 16384.0 * 360.0;
                        values.Encoder_out = (double)packet.PopFrontInt32() / 16384.0 * 360.0;
                        values.Duty_now = packet.PopFrontDouble16(1e3);
                        values.Rpm = packet.PopFrontDouble32(1e0);
                        values.V_in = packet.PopFrontDouble16(1e2);
                        values.Runout_value = packet.PopFrontDouble32(1e2);
                        values.Step_sensor = packet.PopFrontUInt8();
                        values.Fault_code = packet.PopFrontInt8();
                        values.Fault_str = values.Fault_code.ToString();
                        values.Pid_pos_set = packet.PopFrontDouble32(1e6);

                        if (packet.Size() >= 4)
                        {
                            values.Pid_pos_now = packet.PopFrontDouble32(1e6);
                        }
                        else
                        {
                            values.Pid_pos_now = -1.0;
                        }
                        values.Brake_count = packet.PopFrontInt32();
                        SerialPortConnector.UpdateVescValues(values);
                    }
                    break;
            }
        }

        private static void sendCommand(ByteArray packet)
        {
            SerialPortConnector.SendData(packet);
        }

        public static void SetDutyCycle(double dutyCycle)
        {
            ByteArray arr = new ByteArray();
            arr.AppendInt8((byte)COMM_PACKET_ID.COMM_SET_DUTY);
            arr.AppendDouble32(dutyCycle, 1e5);
            sendCommand(arr);
        }

        public static void GetFwVersion()
        {
            ByteArray arr = new ByteArray();
            arr.AppendInt8((byte)COMM_PACKET_ID.COMM_FW_VERSION_REMOTION);
            sendCommand(arr);
        }


    }
}
