using Connection.Communication;
using Connection.Communication.Fakes;
using Connection.Protocols.Enums;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

namespace DataLogger.Test.Connection.Communication
{
    [TestClass]
    public class SensorHubTest
    {

        [TestMethod]
        public void Initialize()
        {

            using (ShimsContext.Create())
            {
                DeviceConnectionFactory connectionFactory = new DeviceConnectionFactory();
                connectionFactory.DeviceConnectionType = DeviceConnectionType.Test;
                SensorHub hub = new SensorHub(connectionFactory);

                bool isStringReceived = false;
                bool isByteReceived = false;
                string stringData = "";
                byte[] byteData = null;

                using (ShimsContext.Create())
                {
                    ShimSensorHub.AllInstances.Connection_StringReceivedObjectStringReceivedEventArgs = (hub, obj, args) =>
                    {
                        isStringReceived = true;
                        stringData = args.Data;
                    };

                    ShimSensorHub.AllInstances.Connection_ByteArrayReceivedObjectByteArrayReceivedEventArgs = (hub, obj, args) =>
                    {
                        isByteReceived = true;
                        byteData = args.Data;   
                    };

                    hub.Initialize();
                }

                Assert.IsTrue(isByteReceived);
                Assert.IsTrue(isStringReceived);
                Assert.AreEqual("TEST1234", stringData);
                Assert.AreEqual(Encoding.ASCII.GetBytes("TEST1234").Length, byteData.Length);
            }
        }
    }
}
