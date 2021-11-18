using Business.Hubs.Contracts;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Hubs
{
    public class MeasurementHub : Hub, IMeasurementHub
    {
        IHubContext<MeasurementHub> _hubContext;

        public MeasurementHub(IHubContext<MeasurementHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public Task SendMeasurements(string measurementString)
        {
            return _hubContext.Clients.All.SendAsync("ReceiveMeasurementString", measurementString);
        }

        public Task SendOperationTimer(string operationTimer)
        {
            return _hubContext.Clients.All.SendAsync("ReceiveOperationTimer", operationTimer);
        }
    }
}

