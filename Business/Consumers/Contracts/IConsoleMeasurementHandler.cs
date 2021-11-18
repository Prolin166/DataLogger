
using Connection.Communication;

namespace Business.Consumers.Contracts
{
    public interface IConsoleMeasurementHandler 
    {
        void StartHandler();
        void StopHandler();
    }
}
