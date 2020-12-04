using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VescConnector
{
    public class ByteArray
    {
        public List<byte> data;

        private double roundDouble(double x)
        {
            return x < 0.0 ? Math.Ceiling(x - 0.5) : Math.Floor(x + 0.5);
        }

        private void append(List<byte> data)
        {
            this.data.AddRange(data);
        }

        #region Constructor
        /// <summary>
        /// Конструктор по-умолчанию.
        /// </summary>
        public ByteArray()
        {
            this.data = new List<byte>();
        }

        public ByteArray(string data)
        {
            this.data = Encoding.ASCII.GetBytes(data).ToList();
        }
        #endregion


        #region Append Methods
        public void AppendInt32(Int32 number)
        {
            var data = new List<byte>();
            int res = number;
            data.Add((byte)((res >> 24) & 0xFF));
            data.Add((byte)((res >> 16) & 0xFF));
            data.Add((byte)((res >> 8) & 0xFF));
            data.Add((byte)(res & 0xFF));
            append(data);
        }

        public void AppendUInt32(UInt32 number)
        {
            var data = new List<byte>();
            UInt32 res = number;
            data.Add((byte)((res >> 24) & 0xFF));
            data.Add((byte)((res >> 16) & 0xFF));
            data.Add((byte)((res >> 8) & 0xFF));
            data.Add((byte)(res & 0xFF));
            append(data);
        }

        public void AppendInt16(Int16 number)
        {
            var data = new List<byte>();
            Int16 res = number;
            data.Add((byte)((res >> 8) & 0xFF));
            data.Add((byte)(res & 0xFF));
            append(data);
        }

        public void AppendUInt16(UInt16 number)
        {
            var data = new List<byte>();
            UInt16 res = number;
            data.Add((byte)((res >> 8) & 0xFF));
            data.Add((byte)(res & 0xFF));
            append(data);
        }

        public void AppendInt8(byte number)
        {
            var data = new List<byte>();
            data.Add(number);
            append(data);
        }

        public void AppendUInt8(byte number)
        {
            var data = new List<byte>();
            data.Add(number);
            append(data);
        }

        public void AppendDouble32(double number, double scale)
        {
            AppendInt32((Int32)roundDouble(number * scale));
        }

        public void AppendDouble16(double number, double scale)
        {
            AppendInt32((Int32)roundDouble(number * scale));
        }

        #endregion


        #region Pop Methods
        public Int32 PopFrontInt32()
        {
            if (this.data.Count < 4)
            {
                return 0;
            }

            Int32 res = (data[0] << 24 | data[1] << 16 | data[2] << 8 | data[3]);
            this.data.RemoveRange(0, 4);
            return res;
        }
        public UInt32 PopFrontUInt32()
        {
            if (this.data.Count < 4)
            {
                return 0;
            }

            UInt32 res = (UInt32)(data[0] << 24 | data[1] << 16 | data[2] << 8 | data[3]);
            this.data.RemoveRange(0, 4);
            return res;
        }

        public Int16 PopFrontInt16()
        {
            if (this.data.Count < 2)
            {
                return 0;
            }

            Int16 res = (Int16)(data[0] << 8 | data[1]);
            this.data.RemoveRange(0, 2);
            return res;
        }


        public UInt16 PopFrontUInt16()
        {
            if (this.data.Count < 2)
            {
                return 0;
            }

            UInt16 res = (UInt16)(data[0] << 8 | data[1]);
            this.data.RemoveRange(0, 2);
            return res;
        }

        public byte PopFrontInt8()
        {
            if (this.data.Count < 1)
            {
                return 0;
            }

            byte res = data[0];
            this.data.RemoveRange(0, 1);
            return res;
        }

        public byte PopFrontUInt8()
        {
            if (this.data.Count < 1)
            {
                return 0;
            }

            byte res = data[0];
            this.data.RemoveRange(0, 1);
            return res;
        }
        public double PopFrontDouble32(double scale)
        {
            return (double)PopFrontInt32() / scale;
        }

        public double PopFrontDouble16(double scale)
        {
            return (double)PopFrontInt16() / scale;
        }

        public string PopFrontString()
        {
            if (data.Count < 1) return string.Empty;

            string res =  BitConverter.ToString(data.ToArray(), 0, data.Count);
            data.Clear();
            return res;
        }
        #endregion


        public byte[] GetDataArray() => data.ToArray();
        public int Size() => data.Count;


    }
}
