using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Runtime.InteropServices;
using System.Text;

namespace Common.OSHelper
{
    public static class GpioProvider
    {
        private static GpioController _gpioController = new GpioController(GpioScheme);
        private static bool _isUnixSystem => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        public static PinNumberingScheme GpioScheme { get; set; } = PinNumberingScheme.Logical;
        public static void SetGpioState(int pin, bool state)
        {
            if (_isUnixSystem)
            {
                try
                {
                    if (state)
                        _gpioController.Write(pin, PinValue.High);
                    else
                        _gpioController.Write(pin, PinValue.Low);

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message + "\r\n" + e.InnerException?.Message);

                }
            }

        }

        public static int GetGpioState(int pin)
        {
            int state = 0000;
            if (_isUnixSystem)
            {
                try
                {
                    var value = _gpioController.Read(pin);
                    if (value == PinValue.High)
                        state = 1;
                    else
                        state = 0;

                }
                catch (Exception e)
                {

                    Console.WriteLine(e.Message + "\r\n" + e.InnerException?.Message);

                }
            }

            return state;
        }
    }
}
