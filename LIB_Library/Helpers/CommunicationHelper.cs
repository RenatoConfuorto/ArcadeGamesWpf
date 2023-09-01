using LIB.Communication.Constants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LIB.Helpers
{
    public class CommunicationHelper
    {
        /// <summary>
        /// Gets the IP address of the current device
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// Serialize an object into byte array
        /// </summary>
        /// <param name="obj">object to serialize</param>
        /// <returns></returns>
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
        /// <summary>
        /// Deserialize byte array into object
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static object DeserializeObject(byte[] data)
        {
            if (data.Length == 0) return null;
            BinaryFormatter bf = new BinaryFormatter();
            using(MemoryStream ms = new MemoryStream(data))
            {
                return bf.Deserialize(ms);
            }
        }
        /// <summary>
        /// Create a new socket
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static Socket InitSocket(IPAddress address)
        {
            return new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        }
        /// <summary>
        /// Tries and execute a method multiple time.
        /// </summary>
        /// <param name="method">The method to execute</param>
        /// <param name="obj">The object on which to call the method</param>
        /// <param name="parameters">The methods parameters</param>
        public static void ExecuteRecursiveTry(MethodInfo method, object obj, params object[] parameters)
        {
            int tries = 0;
            //bool auxCheck = EvaluateAuxChecks(AuxChecks);
            Exception lastEx = null;
            while (tries < CommunicationCnst.CONNECTION_MAX_TRIES)
            {
                try
                {
                    method?.Invoke(obj, parameters);
                    return;
                }
                catch (Exception ex)
                {
                    tries++;
                    Thread.Sleep(CommunicationCnst.CONNECTION_RETRY_WAIT);
                    lastEx = ex;
                }
            }
            if (lastEx != null) throw lastEx;
        }
        /// <summary>
        /// Get the MethodInfo of a method
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public static MethodInfo GetMethodInfo(object obj, Action method)
        {
            return obj.GetType().GetMethod(method.Method.Name);
        }
    }
}
