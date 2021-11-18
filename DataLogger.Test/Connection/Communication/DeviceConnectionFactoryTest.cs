using Connection.Communication;
using Connection.Protocols;
using Connection.Protocols.Contracts;
using Connection.Protocols.Enums;
using Connection.Protocols.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DataLogger.Test.Connection.Communication
{
    [TestClass]
    public class DeviceConnectionFactoryTest
    {
        [TestMethod]
        public void CreateConnection_ChooseRightDeviceConnectionType_IsTestConnection()
        {
            DeviceConnectionFactory connectionFactory = new DeviceConnectionFactory();
            connectionFactory.DeviceConnectionType = DeviceConnectionType.Test;

            var result = connectionFactory.CreateConnection();

            Assert.IsInstanceOfType(result, typeof(TestConnection));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CreateConnection_ChooseRightDeviceConnectionType_IsSimulationConnection()
        {
            DeviceConnectionFactory connectionFactory = new DeviceConnectionFactory();
            connectionFactory.DeviceConnectionType = DeviceConnectionType.Simulation;

            DeviceConnection result;

            using (ShimsContext.Create())
            {
                ShimSimulationConnection.ConstructorSimulationConnectionOptions = (conn, opt) =>
                {

                };

                result = connectionFactory.CreateConnection();
            }


            Assert.IsInstanceOfType(result, typeof(SimulationConnection));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CreateConnection_ChooseRightDeviceConnectionType_IsSerialConnection()
        {
            DeviceConnectionFactory connectionFactory = new DeviceConnectionFactory();
            connectionFactory.DeviceConnectionType = DeviceConnectionType.Serial;

            var result = connectionFactory.CreateConnection();

            Assert.IsInstanceOfType(result, typeof(SerialConnection));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CreateConnection_ChooseRightDeviceConnectionType_IsNoneConnection()
        {
            DeviceConnectionFactory connectionFactory = new DeviceConnectionFactory();
            connectionFactory.DeviceConnectionType = DeviceConnectionType.None;

            Assert.ThrowsException<NotImplementedException>(connectionFactory.CreateConnection, "DeviceConnectionType unknown");
        }
    }
}
