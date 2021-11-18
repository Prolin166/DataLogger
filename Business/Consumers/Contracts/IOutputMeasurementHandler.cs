
using Connection.Communication;

namespace Business.Consumers.Contracts
{
    public interface IOutputMeasurementHandler
    {
        void StartHandler();
        void StopHandler();
    }
}
