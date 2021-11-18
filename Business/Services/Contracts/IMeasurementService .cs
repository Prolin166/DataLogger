using Common.Models;
using System.Collections.Generic;

namespace Business.Services.Contracts
{
    public interface IMeasurementService
    {
        List<MeasurementModel> GetAllMeasurements();
        MeasurementModel GetMeasurementById(int id);
        MeasurementModel GetLastMeasurement();
        bool AddMeasurementFromSensors(ICollection<MeasurementModel> measurement, bool detachFromAddedElementsAutomatically = true);
        bool DeleteMeasurementById(int id);
        bool DeleteMeasurementsOlderThanDays(int days);
        void WriteMeasurementsToFile(string name);
        byte[] SendMeasurmentsToClient();
    }
}
