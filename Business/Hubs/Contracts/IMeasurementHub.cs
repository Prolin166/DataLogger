using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Hubs.Contracts
{
    public interface IMeasurementHub
    {
        Task SendMeasurements(string measurementString);
        Task SendOperationTimer(string operationTimer);
    }
}
