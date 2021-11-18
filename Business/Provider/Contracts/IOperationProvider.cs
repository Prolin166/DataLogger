using Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Provider.Contracts
{
    interface IOperationProvider
    {
        void SetGpioState(int pin, bool state);
        int GetGpioState(int pin);
        void WriteStringData(string data);
        void WriteByteData(byte data);
        void ActivateHandler(HandlerTypeModel type);
        void DeactivateHandler(HandlerTypeModel type);
    }
}
