using Business.Hubs;
using Business.Hubs.Contracts;
using Business.Services.Contracts;
using Common.Enums;
using Common.Extensions;
using Common.Models;
using Connection.Communication;
using Connection.Communication.Contracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Consumers.Contracts
{
    public class OutputMeasurementHandler : IOutputMeasurementHandler
    {

        private ISensorHub _sensorHub;
        private IMeasurementHub _measurementHub;
        private IServiceProvider _serviceProvider;
        private IManagementService _managementService;

        public OutputMeasurementHandler(ISensorHub sensorHub, 
                                        IMeasurementHub measurementHub, 
                                        IServiceProvider serviceProvider, 
                                        IManagementService managementService)
        {
            _sensorHub = sensorHub;
            _measurementHub = measurementHub;
            _serviceProvider = serviceProvider;
            _managementService = managementService;
        }

        private void HandleSensorData(object sender, SensorDataReceivedArgs e)
        {
            if (e.RawDataReceived || !e.StringData.IsValidJson())
                return; // logging ...

            var notification = JsonConvert.DeserializeObject<ControllerNotificationModel>(e.StringData);
            var sensorService = _serviceProvider.GetService(typeof(ISensorService)) as ISensorService;

            var activeSensors = sensorService.GetAllSensors(true);

            var writingData = new ControllerNotificationModel
            {
                SessionId = notification.SessionId,
            };

            writingData.Measurements = new List<MeasurementModel>();

            foreach (var measure in notification.Measurements)
            {
                if (activeSensors.Exists(s => s.Id == measure.SensorId))
                {
                    writingData.Measurements.Add(measure);
                }

            }
            var opTimer = new OperationTimerModel();

            if (_managementService.OperationTimerExists())
                opTimer = _managementService.GetOperationTimerValue();

            if (opTimer.SessionId == notification.SessionId)
            {
                if (opTimer.LastTime.ToString() == "01.01.01 00:00:00")
                {
                    opTimer.LastTime = DateTime.Now;
                }
                else
                {
                    var time = DateTime.Now - opTimer.LastTime;
                    opTimer.CumulatedCount = opTimer.CumulatedCount + time;
                }
            }

            opTimer.LastTime = DateTime.Now;
            opTimer.SessionId = notification.SessionId;

            _measurementHub.SendOperationTimer(JsonConvert.SerializeObject(opTimer)).Wait();

            _measurementHub.SendMeasurements(JsonConvert.SerializeObject(writingData)).Wait();
        }

        public void StartHandler()
        {
            _sensorHub.SensorDataReceived += HandleSensorData;
        }

        public void StopHandler()
        {
            _sensorHub.SensorDataReceived -= HandleSensorData;
        }
    }
}
