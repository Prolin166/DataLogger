using Common.Models;
using System.Collections.Generic;

namespace Business.Services.Contracts
{
    public interface ISensorService
    {
        List<SensorModel> GetAllSensors();
        List<SensorModel> GetAllSensors(bool isActive);
        SensorModel GetSensorById(int id);
        bool AddSensorFromSensors(SensorModel sensor);
        bool EditSensorProperties(SensorModel sensor);
        bool DeleteSensor(int id);
    }
}
