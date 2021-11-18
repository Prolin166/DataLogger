using Business.Consumers.Contracts;
using Business.Consumers.Contracts.Fakes;
using Business.Hubs.Contracts;
using Business.Services.Contracts;
using Connection.Communication;
using Connection.Communication.Contracts;
using Connection.Protocols.Enums;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace DataLogger.Test.Business.Consumers
{
    [TestClass]
    public class OutputMeasurementHandlerTest
    {
        private IOutputMeasurementHandler _outputMeasurementHandler;
        private ISensorHub _hub;

        [TestInitialize]
        public void InitTest()
        {
            DeviceConnectionFactory connectionFactory = new DeviceConnectionFactory();
            connectionFactory.DeviceConnectionType = DeviceConnectionType.Test;
            _hub = new SensorHub(connectionFactory);
            _outputMeasurementHandler = new OutputMeasurementHandler(_hub, Mock.Of<IMeasurementHub>(), Mock.Of<IServiceProvider>(), Mock.Of<IManagementService>());
            _outputMeasurementHandler.StartHandler();
        }

        [TestMethod]
        public void StartHandler()
        {
            bool isActivated = false;
            string data = null;

            using (ShimsContext.Create())
            {
                ShimOutputMeasurementHandler.AllInstances.HandleSensorDataObjectSensorDataReceivedArgs = (hub, obj, args) =>
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
            bool isActivated = false;
            string data = null;

            _outputMeasurementHandler.StopHandler();

            using (ShimsContext.Create())
            {
                ShimOutputMeasurementHandler.AllInstances.HandleSensorDataObjectSensorDataReceivedArgs = (hub, obj, args) =>
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
