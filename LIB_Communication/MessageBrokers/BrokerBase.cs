using Core.Helpers;
using LIB_Com.Constants;
using LIB_Com.Events;
using LIB_Com.Messages;
using LIB_Com.Messages.Base;
using LIB_Com.Entities;
using LIB.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using static LIB_Com.Constants.CommunicationCnst;
using LIB_Com.Helpers;

namespace LIB_Com.MessageBrokers
{
    public class BrokerBase : IDisposable
    {
        protected Socket _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        protected byte[] buffer = new byte[DEFAULT_BUFFER_SIZE];

        #region Events
        public event EventHandler<MessageReceivedEventArgs> MessageReceivedEvent;
        #endregion

        #region User Managment
        private OnlineUser _user;
        /// <summary>
        /// Define the user that is using the broker
        /// </summary>
        public OnlineUser User
        {
            get => _user != null ? _user : new OnlineUser();
            set => _user = value;
        }
        /// <summary>
        /// User Name visualized in the client
        /// </summary>
        public string UserName
        {
            get => _user != null ? _user.UserName : string.Empty;
            set
            {
                if (_user != null) { _user.UserName = value; }
                else throw new ArgumentNullException($"{nameof(User)} è null");
            }
        }
        /// <summary>
        /// User GUID
        /// </summary>
        public Guid LocalUserId
        {
            get => (Guid)_user.UserId;
            set
            {
                if (_user != null) { _user.UserId = value; }
                else throw new ArgumentNullException($"{nameof(User)} è null");
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Create a new instance of the broker
        /// </summary>
        /// <param name="userName"></param>
        public BrokerBase(string userName)
        {
            User = new OnlineUser()
            {
                UserName = userName,
            };
        }
        #endregion

        #region Send/Receive Messages
        /// <summary>
        /// socket starts listening to incoming messages
        /// </summary>
        /// <param name="socket"></param>
        protected void StartReceive(Socket socket)
        {
            socket.BeginReceive(buffer, 0, DEFAULT_BUFFER_SIZE, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
        }
        /// <summary>
        /// converts the main buffer into a byte array and then into an object
        /// </summary>
        /// <param name="receivedBytes"></param>
        /// <returns></returns>
        protected object GetMessageFromBytes(int receivedBytes)
        {
            byte[] data = new byte[receivedBytes];
            Array.Copy(buffer, data, receivedBytes);
            object message = DeserializeObject(data);
            //object message = CommunicationHelper.DeserializeObject(data);
            return message;
        }
        /// <summary>
        /// generic callback of method BeginReceive in <method cref="StartReceive"/>
        /// </summary>
        /// <param name="ar"></param>
        private void ReceiveCallback(IAsyncResult ar)
        {
            Socket socket = (Socket)ar.AsyncState;
            int receivedData;
            try
            {
                receivedData = socket.EndReceive(ar);
                object message = GetMessageFromBytes(receivedData);
                OnMessageReceived(message);
                socket.BeginReceive(buffer, 0, DEFAULT_BUFFER_SIZE, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);

            }
            catch(ObjectDisposedException odEx)
            {
                return;
            }
            catch(SocketException sEx)
            {
                //Connessione persa;
                return;
            }
            catch(Exception ex)
            {
                return;
            }
        }
        /// <summary>
        /// triggers the event <event cref="MessageReceived"/> using the message object and the message code
        /// </summary>
        /// <param name="message"></param>
        private void OnMessageReceived(object message)
        {
            MessageReceivedEvent?.Invoke(this, new MessageReceivedEventArgs(message));
        }
        /// <summary>
        /// Send a single message
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="message"></param>
        public void SendMessage(Socket socket, object message)
        {
            //inserire GUID del mittente
            MessageBase messageBase = message as MessageBase;
            messageBase.SenderId = LocalUserId;

            byte[] data = CommunicationHelper.SerializeObject(messageBase);
            socket.Send(data, 0, data.Length, SocketFlags.None);
        }
        #endregion

        #region IDisposable
        public virtual void Dispose()
        {
            if(_socket != null) _socket.Close();
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Deserialize Message
        /// <summary>
        /// Deserialize byte array into a message object
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected MessageBase DeserializeObject(byte[] data)
        {
            return MessageDeserializer.DeserializeMessage(data);
        }
        #endregion
    }
}
