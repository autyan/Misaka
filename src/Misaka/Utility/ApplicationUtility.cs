using System.Net;
using System.Net.Sockets;
using System.Reflection;

namespace Misaka.Utility
{
    public static class ApplicationUtility
    {
        public static string GetHostIpAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }

            return string.Empty;
        }

        public static string GetCurrentApplicationName()
            => Assembly.GetCallingAssembly().GetName().Name;
    }
}
