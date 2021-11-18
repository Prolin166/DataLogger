using Business.Consumers;
using Business.Consumers.Contracts;
using Business.Consumers.Contracts.Fakes;
using Business.Consumers.Fakes;
using Connection.Communication;
using Connection.Communication.Contracts;
using Connection.Protocols.Enums;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataLogger.Test.Business.Consumers
{
    [TestClass]
    public class ConsoleMeasurementHandlerTest
    {
        private IConsoleMeasurementHandler _consoleMeasurementHandler;
        private ISensorHub _hub;

        [TestInitialize]
        public void InitTest()
        {
            DeviceConnectionFactory connectionFactory = new DeviceConnectionFactory();
            connectionFactory.DeviceConnectionType = DeviceConnectionType.Test;
            _hub = new SensorHub(connectionFactory);
            _consoleMeasurementHandler = new ConsoleMeasurementHandler(_hub);
        }

        [TestMethod]
        public void StartHandler()
        {
            _consoleMeasurementHandler.StartHandler();

            bool isActivated = true;
            string data = null;

            using (ShimsContext.Create())
            {
                ShimConsoleMeasurementHandler.AllInstances.HandleSensorDataObjectSensorDataReceivedArgs = (hub, obj, args) =>
                {
                    isActivated = true;
                    data = args.StringData;
                };

                _hub.Initialize();
            }

            Assert.IsTrue(isActivated);
            Assert.IsNotNull(data);
        }


        [TestMethod]
        public void StopHandler()
        {
            _consoleMeasurementHandler.StartHandler();

            bool isActivated = false;
            string data = null;

            _consoleMeasurementHandler.StopHandler();

            using (ShimsContext.Create())
            {
                ShimConsoleMeasurementHandler.AllInstances.HandleSensorDataObjectSensorDataReceivedArgs = (hub, obj, args) =>
                {
                    isActivated = true;
                    data = args.StringData;
                };

                _hub.Initialize();
            }

            Assert.IsFalse(isActivated);
            Assert.IsNull(data);
        }

    }
}
