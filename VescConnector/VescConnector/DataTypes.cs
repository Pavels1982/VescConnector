using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VescConnector
{
    public class DataTypes
    {
        public enum COMM_PACKET_ID
        {
            COMM_FW_VERSION = 0,
            COMM_JUMP_TO_BOOTLOADER,
            COMM_ERASE_NEW_APP,
            COMM_WRITE_NEW_APP_DATA,
            COMM_GET_VALUES,
            COMM_SET_DUTY,
            COMM_SET_CURRENT,
            COMM_SET_CURRENT_BRAKE,
            COMM_SET_RPM,
            COMM_SET_POS,
            COMM_SET_HANDBRAKE,
            COMM_SET_DETECT,
            COMM_SET_SERVO_POS,
            COMM_SET_MCCONF,
            COMM_GET_MCCONF,
            COMM_GET_MCCONF_DEFAULT,
            COMM_SET_APPCONF,
            COMM_GET_APPCONF,
            COMM_GET_APPCONF_DEFAULT,
            COMM_SAMPLE_PRINT,
            COMM_TERMINAL_CMD,
            COMM_PRINT,
            COMM_ROTOR_POSITION,
            COMM_EXPERIMENT_SAMPLE,
            COMM_DETECT_MOTOR_PARAM,
            COMM_DETECT_MOTOR_R_L,
            COMM_DETECT_MOTOR_FLUX_LINKAGE,
            COMM_DETECT_ENCODER,
            COMM_DETECT_HALL_FOC,
            COMM_REBOOT,
            COMM_ALIVE,
            COMM_GET_DECODED_PPM,
            COMM_GET_DECODED_ADC,
            COMM_GET_DECODED_CHUK,
            COMM_FORWARD_CAN,
            COMM_SET_CHUCK_DATA,
            COMM_CUSTOM_APP_DATA,
            COMM_NRF_START_PAIRING,
            COMM_FW_VERSION_REMOTION,
            COMM_SET_BRAKEON,
            COMM_SET_BRAKEOFF,
            COMM_SET_BRAKETEST

        }



    }
}
