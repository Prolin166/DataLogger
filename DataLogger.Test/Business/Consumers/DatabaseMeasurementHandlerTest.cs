using Business.Consumers;
using Business.Consumers.Contracts;
using Business.Consumers.Fakes;
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
    public class DatabaseMeasurementHandlerTest
    {
        private IDatabaseMeasurementHandler _databaseMeasurementHandler;
        private ISensorHub _hub;

        [TestInitialize]
        public void InitTest()
        {
            DeviceConnectionFactory connectionFactory = new DeviceConnectionFactory();
            connectionFactory.DeviceConnectionType = DeviceConnectionType.Test;
            _hub = new SensorHub(connectionFactory);
            _databaseMeasurementHandler = new DatabaseMeasurementHandler(_hub, Mock.Of<IMeasurementService>(), Mock.Of<IManagementService>(), Mock.Of<IServiceProvider>());
            _databaseMeasurementHandler.StartHandler();
        }

        [TestMethod]
        public void StartHandler()
        {

            bool isActivated = true;
            string data = null;

            using (ShimsContext.Create())
            {
                ShimDatabaseMeasurementHandler.AllInstances.HandleSensorDataObjectSensorDataReceivedArgs = (hub, obj, args) =>
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

            _databaseMeasurementHandler.StopHandler();

            using (ShimsContext.Create())
            {
                ShimDatabaseMeasurementHandler.AllInstances.HandleSensorDataObjectSensorDataReceivedArgs = (hub, obj, args) =>
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
