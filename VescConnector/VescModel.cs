using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VescConnector
{
    public class VescModel: INotifyPropertyChanged
    {
        public double Temp_mos { get; set; }
        public double Temp_motor { get; set; }
        public double Current_motor { get; set; }
        public double Current_in { get; set; }
        public double Encoder_in { get; set; }
        public double Encoder_out { get; set; }
        public double Duty_now { get; set; }
        public double Rpm { get; set; }
        public double V_in { get; set; }
        public double Runout_value { get; set; }
        public byte Step_sensor { get; set; }
        public byte Fault_code { get; set; }
        public string Fault_str { get; set; }
        public double Pid_pos_set { get; set; }
        public double Pid_pos_now { get; set; }
        public int Brake_count { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
