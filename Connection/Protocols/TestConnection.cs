using Connection.Protocols.Contracts;
using Connection.Protocols.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Connection.Protocols
{
    public class TestConnection : DeviceConnection
    {
        public TestConnection()
        {

        }

        public override event EventHandler<ByteArrayReceivedEventArgs> ByteArrayReceived;
        public override event EventHandler<StringReceivedEventArgs> StringReceived;

        public override void Write(byte data)
        {
        }

        public override void Write(string data)
        {
        }

        public override void Connect()
        {
            StringReceived(this, new StringReceivedEventArgs("TEST1234"));
            ByteArrayReceived(this, new ByteArrayReceivedEventArgs(new ASCIIEncoding().GetBytes("TEST1234")));
        }
        public override void Disconnect()
        {
        }
    }
}