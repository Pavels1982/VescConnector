namespace VescConnector
{
    public class VescValues
    {
        public double Temp_mos { get; set; } = 0;
        public double Temp_motor { get; set; } = 0;
        public double Current_motor { get; set; } = 0;
        public double Current_in { get; set; } = 0;
        public double Encoder_in { get; set; } = 0;
        public double Encoder_out { get; set; } = 0;
        public double Duty_now { get; set; } = 0;
        public double Rpm { get; set; } = 0;
        public double V_in { get; set; } = 0;
        public double Runout_value { get; set; } = 0;
        public byte Step_sensor { get; set; } = 0;
        public byte Fault_code { get; set; } = 0;
        public string Fault_str { get; set; } = string.Empty;
        public double Pid_pos_set { get; set; } = 0;
        public double Pid_pos_now { get; set; } = 0;
        public int Brake_count { get; set; } = 0;
    }
}
