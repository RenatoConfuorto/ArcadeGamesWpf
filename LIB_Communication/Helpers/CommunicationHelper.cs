using LIB_Com.Constants;
using LIB_Com.Entities;
using LIB_Com.Entities.OnlineGameSettings;
using LIB_Com.Events;
using LIB_Com.Extensions;
using LIB_Com.Messages;
using LIB_Com.Messages.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static LIB_Com.Constants.CommunicationCnst;
using static LIB_Com.Constants.OnlineGamesDefs;

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
        #region Serialize / Deserialize
        #region OLD METHOD
        /// <summary>
        /// Serialize an object into byte array
        /// </summary>
        /// <param name="obj">object to serialize</param>
        /// <returns></returns>
        /*public static byte[] SerializeObject(object obj)
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
        }*/
        #endregion

        #region New Method
        /// <summary>
        /// Serialize a message into byte array
        /// </summary>
        /// <param name="obj">object to serialize</param>
        /// <returns></returns>
        public static byte[] SerializeObject<T>(T obj)
            where T : MessageBase
        {
            return obj.Serialize();
        }
        /// <summary>
        /// Deserialize byte array into a message
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T DeserializeObject<T>(byte[] data)
            where T : MessageBase , new()
        {
            T result = new T();
            result.Deserialize(data);
            return result;
        }
        #endregion
        #endregion
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
        /// Check if the client is still connected
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        public static bool CheckIfClientIsConnected(Socket socket)
        {
            try
            {
                return !(socket.Poll(1, SelectMode.SelectRead) && socket.Available == 0);
            }
            catch (SocketException) { return false; }
        }
        /// <summary>
        /// set a string into a char array
        /// </summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="fieldLength"></param>
        public static void SetString(ref char[] field, string value, int fieldLength)
        {
            SetArray(ref field, value, fieldLength, ' ');
        }
        /// <summary>
        /// Set an array keeping the same length
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="fieldLength"></param>
        public static void SetArray<T>(ref T[] field, IEnumerable<T> value, int fieldLength)
            where T : new()
        {
            SetArray(ref field, value, fieldLength, new T());
        }
        /// <summary>
        /// Set an array keeping the same length
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="fieldLength"></param>
        /// <param name="voidValue"></param>
        public static void SetArray<T>(ref T[] field, IEnumerable<T> value, int fieldLength, T voidValue)
            where T : new()
        {
            field = new T[fieldLength];
            for(int i = 0; i < fieldLength; i++)
            {
                T element;
                if(i < value.Count())
                {
                    element = value.ElementAt(i);
                }
                else
                {
                    element = voidValue;
                }
                field[i] = element;
            }
        }
        /// <summary>
        /// Reads an online settings object from the Binary Reader given the GameId
        /// </summary>
        /// <param name="br"></param>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public static OnlineSettingsBase DeserializeOnlineSettings(BinaryReader br, int gameId)
        {
            OnlineSettingsBase result = null;
            switch(gameId)
            {
                case TRIS_ID:
                    result = br.ReadObject<OnlineTrisSettings>();
                    break;
            }
            return result;
        }
    }
}
