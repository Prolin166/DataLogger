
using Connection.Communication;

namespace Business.Consumers.Contracts
{
    public interface IDatabaseMeasurementHandler 
    {
        void StartHandler();
        void StopHandler();
    }
}
