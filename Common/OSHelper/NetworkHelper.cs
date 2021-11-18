using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Common.OSHelper
{
    public static class NetworkHelper
    {
        public static string IPAdress { get; } = ReadIPAdress();

        public static string HostName { get; } = ReadHostName();

        public static string MacAdress { get; } = ReadMacAdress();

        private static string ReadIPAdress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        private static string ReadHostName()
        {
            string host = string.Empty;

            host = Dns.GetHostName();

            return host;
        }

        private static string ReadMacAdress()
        {
            string macAddresses = string.Empty;

            foreach (NetworkInterface adapter in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (adapter.OperationalStatus == OperationalStatus.Up)
                {
                    macAddresses += adapter.GetPhysicalAddress().ToString();
                    break;
                }
            }

            return macAddresses;
        }
    }
}
