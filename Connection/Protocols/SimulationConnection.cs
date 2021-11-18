using Connection.Protocols.Contracts;
using Connection.Protocols.Events;
using Connection.Protocols.Options;
using System;
using System.Text;
using System.Timers;

namespace Connection.Protocols
{
    public class SimulationConnection : DeviceConnection
    {
        private readonly SimulationConnectionOptions _simulationConnectionOptions;
        private readonly Timer _timer;

        public SimulationConnection(SimulationConnectionOptions simulationConnectionOptions)
        {
            _simulationConnectionOptions = simulationConnectionOptions;

            _timer = new Timer();
            _timer.Interval = _simulationConnectionOptions.ReadCycle;
            _timer.Elapsed += Timer_Elapsed;
        }

        public override event EventHandler<ByteArrayReceivedEventArgs> ByteArrayReceived;
        public override event EventHandler<StringReceivedEventArgs> StringReceived;

        public override void Connect()
        {
            _timer.Start();
        }

        public override void Disconnect()
        {
            _timer.Stop();
        }

        public override void Write(byte data)
        {
            throw new NotImplementedException();
        }

        public override void Write(string data)
        {
            throw new NotImplementedException();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            string randomJson = "{\"SessionId\":245,\"Measurements\":[{\"SensorId\":\"1\",\"Value\":0.000313},{\"SensorId\":\"2\",\"Value\":-0.000016},{\"SensorId\":\"3\",\"Value\":-0.000047},{\"SensorId\":\"4\",\"Value\":-0.000016},{\"SensorId\":\"5\",\"Value\":0},{\"SensorId\":\"6\",\"Value\":0},{\"SensorId\":\"7\",\"Value\":0},{\"SensorId\":\"8\",\"Value\":0},{\"SensorId\":\"9\",\"Value\":24.01},{\"SensorId\":\"10\",\"Value\":1011.656},{\"SensorId\":\"11\",\"Value\":32.71582}]}\r";
            StringReceived(this, new StringReceivedEventArgs(randomJson));
            ByteArrayReceived(this, new ByteArrayReceivedEventArgs(new ASCIIEncoding().GetBytes(randomJson)));
        }
    }
}

