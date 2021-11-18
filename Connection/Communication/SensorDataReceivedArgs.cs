using Common.Extensions;
using System;

namespace Connection.Communication
{
    public class SensorDataReceivedArgs : EventArgs
    {
        public SensorDataReceivedArgs(string stringData = "", byte[] rawData = null)
        {
            StringData = stringData;
            RawData = rawData;
        }

        public string StringData { get; set; }
        public byte[] RawData { get; set; }

        public bool StringReceived => StringData.IsNotNullOrEmpty();
        public bool RawDataReceived => RawData != null && RawData.Length > 0;
    }
}
