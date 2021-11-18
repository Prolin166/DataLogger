using Connection.Protocols.Contracts;
using Connection.Protocols.Events;
using Connection.Protocols.Options;
using System;
using System.IO.Ports;

namespace Connection.Protocols
{
    public class SerialConnection : DeviceConnection
    {
        private readonly SerialPort _serialPort = new SerialPort();
        private readonly SerialConnectionOptions _serialConnectionOptions;

        public override event EventHandler<ByteArrayReceivedEventArgs> ByteArrayReceived;
        public override event EventHandler<StringReceivedEventArgs> StringReceived;

        public SerialConnection(SerialConnectionOptions serialConnectionOptions)
        {
            _serialConnectionOptions = serialConnectionOptions;
        }

        public override void Connect()
        {

            while(_serialPort.IsOpen == false)
            { 
                try
                 {
                        _serialPort.PortName = _serialConnectionOptions.PortName;
                        _serialPort.BaudRate = _serialConnectionOptions.BaudRate;
                        _serialPort.Parity = _serialConnectionOptions.Parity;
                        _serialPort.DataBits = _serialConnectionOptions.DataBits;
                        _serialPort.StopBits = _serialConnectionOptions.StopBits;
                        _serialPort.DataReceived += SerialPort_DataReceived;

                        _serialPort.Open();
                 }
                 catch (Exception ex)
                 {

                        Console.WriteLine("Serielle Verbindung konnte nicht hergestellt werden!");
                        Console.WriteLine("Fehlermeldung: " + ex);
                    
                 }


            }
        }

        public override void Disconnect()
        {
            _serialPort.Close();
        }

        public override void Write(byte data)
        {
            throw new NotImplementedException();
        }

        public override void Write(string data)
        {
            throw new NotImplementedException();
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string serialInput = _serialPort.ReadLine();
            StringReceived(this, new StringReceivedEventArgs(serialInput));
        }
    }
}
