

using Connection.Protocols.Events;
using System;

namespace Connection.Protocols.Contracts
{
    public abstract class DeviceConnection
    {
        public abstract event EventHandler<ByteArrayReceivedEventArgs> ByteArrayReceived;
        public abstract event EventHandler<StringReceivedEventArgs> StringReceived;

        public virtual void Write(byte data)
        {
        }

        public virtual void Write(string data)
        {
        }

        /// <summary>   
        /// Initializes the connection to device, depending on options passed into the constructor.<para />
        /// Received data from connection are written to console if RawDataProcessment or StringProcessment not set.
        /// </summary>
        public virtual void Connect()
        {
        }
        public virtual void Disconnect()
        {
        }
    }
}
