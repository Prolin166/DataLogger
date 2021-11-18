using Common.Models;

namespace Business.Services.Contracts
{
    public interface IManagementService
    {
        bool SetDeviceId(string id);
        string GetDeviceId();
        string GetIPAdress();
        string GetHostName();
        DeviceModel GetDeviceProperties();
        bool InitDeviceProperties();
        string GetMacAddr();
        bool OperationTimerExists();
        OperationTimerModel GetOperationTimerValue();
        bool UpdateOperationTimerFromSensors(OperationTimerModel optModel);
        bool ResetOperationTimer();
        int? GetDaysToSave();
        bool SetDaysToSave(int days);
    }
}
