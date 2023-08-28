using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
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

        public static byte[] SerializeObject(object obj)
        {
            if(obj == null)
            {
                return null;
            }
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            };
        }

        public static object DeserializeObject(byte[] data)
        {
            if (data.Length == 0) return null;
            BinaryFormatter bf = new BinaryFormatter();
            using(MemoryStream ms = new MemoryStream(data))
            {
                return bf.Deserialize(ms);
            }
        }
    }
}
