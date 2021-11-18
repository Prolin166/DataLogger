using System;
using System.Collections.Generic;
using System.Text;

namespace Connection.Protocols.Events
{
    public class StringReceivedEventArgs
    {
        public StringReceivedEventArgs(string input)
        {
            Data = input;
        }

        public string Data { get; set; }
    }
}
