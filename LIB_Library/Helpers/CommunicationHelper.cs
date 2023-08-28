using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LIB.Helpers
{
    public class CommunicationHelper
    {
        public static IPAddress GetLocalIpAddress()
        {
            IPAddress result = IPAddress.Any;
            IPHostEntry entry = Dns.GetHostEntry(Dns.GetHostName());
            foreach(IPAddress ip in entry.AddressList)
            {
                if(ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    result = ip;
                    break;
                }
            }
            return result;
        }
    }
}
