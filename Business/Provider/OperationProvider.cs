using Business.Consumers.Contracts;
using Business.Provider.Contracts;
using Common.Enums;
using Common.OSHelper;
using Connection.Communication.Contracts;
using System;
using System.Device.Gpio;
using System.Runtime.InteropServices;

namespace Business.Provider
{
    public class OperationProvider : IOperationProvider
    {
        private bool _isUnixSystem => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

        private ISensorHub _sensorHub;
        private IConsoleMeasurementHandler _consoleMeasurmentHandler;
        private IDatabaseMeasurementHandler _databaseMeasurementHandler;
        private IOutputMeasurementHandler _outputMeasurementHandler;

        public OperationProvider(ISensorHub sensorHub,
                                 IDatabaseMeasurementHandler databaseMeasurementHandler,
                                 IOutputMeasurementHandler outputMeasurementHandler,
                                 IConsoleMeasurementHandler consoleMeasurementHandler)
        {
            _sensorHub = sensorHub;
            _databaseMeasurementHandler = databaseMeasurementHandler;
            _consoleMeasurmentHandler = consoleMeasurementHandler;
            _outputMeasurementHandler = outputMeasurementHandler;
        }

        public void SetGpioState(int pin, bool state)
        {
            GpioProvider.GpioScheme = PinNumberingScheme.Logical;
            if (_isUnixSystem)
            {
                try
                {
                    GpioProvider.SetGpioState(pin, state);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message + "\r\n" + e.InnerException?.Message);
                }
            }

        }

        public int GetGpioState(int pin)
        {
            int state = 0000;
            if (_isUnixSystem)
            {
                try
                {
                    var value = GpioProvider.GetGpioState(pin);
                }
                catch (Exception e)
                {

                    Console.WriteLine(e.Message + "\r\n" + e.InnerException?.Message);

                }
            }

            return state;
        }

        public void WriteStringData(string data)
        {
            _sensorHub.Connection_WriteStringData(data);
        }

        public void WriteByteData(byte data)
        {
            _sensorHub.Connection_WriteByteData(data);
        }

        public void ActivateHandler(HandlerTypeModel type)
        {
            switch (type)
            {
                case HandlerTypeModel.Database:
                    _databaseMeasurementHandler.StartHandler();
                    break;
                case HandlerTypeModel.Console:
                    _consoleMeasurmentHandler.StartHandler();
                    break;
                case HandlerTypeModel.Output:
                    _outputMeasurementHandler.StartHandler();
                    break;
            }
        }

        public void DeactivateHandler(HandlerTypeModel type)
        {
            switch (type)
            {
                case HandlerTypeModel.Database:
                    _databaseMeasurementHandler.StopHandler();
                    break;
                case HandlerTypeModel.Console:
                    _consoleMeasurmentHandler.StopHandler();
                    break;
                case HandlerTypeModel.Output:
                    _outputMeasurementHandler.StopHandler();
                    break;
            }
        }
    }
}
