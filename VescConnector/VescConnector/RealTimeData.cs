using System.ComponentModel;

namespace VescConnector
{
    public class RealTimeData: INotifyPropertyChanged
    {
        public double Temp_mos { get; set; } = 0;
        public double Temp_motor { get; set; } = 0;
        public double Current_motor { get; set; } = 0;
        public double Current_in { get; set; } = 0;
        public double id { get; set; } = 0;
        public double iq { get; set; } = 0;
        public double Duty_now { get; set; } = 0;
        public double Rpm { get; set; } = 0;
        public double V_in { get; set; } = 0;
        public double Amp_hours { get; set; } = 0;
        public double Amp_hours_charged { get; set; } = 0;
        public double Watt_hours { get; set; } = 0;
        public double Watt_hours_charged { get; set; } = 0;
        public double Tachometer { get; set; } = 0;
        public double Tachometer_abs { get; set; } = 0;
        public byte Fault_code { get; set; } = 0;
        public string Fault_str { get; set; } = string.Empty;
        public double Position { get; set; } = 0;
        public byte Vesc_id { get; set; } = 0;
        public double Temp_mos_1 { get; set; } = 0;
        public double Temp_mos_2 { get; set; } = 0;
        public double Temp_mos_3 { get; set; } = 0;
        public double vd { get; set; } = 0;
        public double vq { get; set; } = 0;

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
