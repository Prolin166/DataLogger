using Business.Consumers.Contracts;
using Business.Services;
using Business.Services.Contracts;
using Common.Enums;
using Common.Extensions;
using Common.Models;
using Connection.Communication;
using Connection.Communication.Contracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Business.Consumers
{
    public class DatabaseMeasurementHandler : IDatabaseMeasurementHandler
    {

        private ISensorHub _sensorHub;
        private IMeasurementService _measurmentService;
        private IServiceProvider _serviceProvider;
        private IManagementService _managementService;

        public DatabaseMeasurementHandler(ISensorHub sensorHub,
                                          IMeasurementService measurementService,
                                          IManagementService managementService, 
                                          IServiceProvider serviceProvider)
        {
            _sensorHub = sensorHub;
            _managementService = managementService;
            _measurmentService = measurementService;
            _serviceProvider = serviceProvider;
        }

        private void HandleSensorData(object sender, SensorDataReceivedArgs e)
        {
            if (_managementService.GetDaysToSave() != null)
                _measurmentService.DeleteMeasurementsOlderThanDays((int)_managementService.GetDaysToSave());

            if (e.RawDataReceived || !e.StringData.IsValidJson())
                return; // logging ...

            var notification = JsonConvert.DeserializeObject<ControllerNotificationModel>(e.StringData);

            var sensorService = _serviceProvider.GetService(typeof(ISensorService)) as ISensorService; 

            var sensorsdb = sensorService.GetAllSensors();
            var activeSensors = sensorService.GetAllSensors(true);

            var writingData = new ControllerNotificationModel
            {
                SessionId = notification.SessionId,
            };

            writingData.Measurements = new List<MeasurementModel>();

            foreach (var measure in notification.Measurements)
            {
                if (!sensorsdb.Exists(s => s.Id == measure.SensorId))
                {
                    sensorService.AddSensorFromSensors(new SensorModel(measure.SensorId));
                }

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

            _managementService.UpdateOperationTimerFromSensors(opTimer);
            _measurmentService.AddMeasurementFromSensors(writingData.Measurements);
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
