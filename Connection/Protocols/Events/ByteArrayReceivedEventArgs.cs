using System;
using System.Collections.Generic;
using System.Text;

namespace Connection.Protocols.Events
{
    public class ByteArrayReceivedEventArgs
    {
        public ByteArrayReceivedEventArgs(byte[] data)
        {
            Data = data;
        }

        public byte[] Data { get; set; }
    }
}
