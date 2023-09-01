using Core.Helpers;
using LIB.Communication.Constants;
using LIB.Communication.Events;
using LIB.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace LIB.Communication.MessageBrokers
{
    public class BrokerBase : IDisposable
    {
        public Socket _socket;
        public event MessageReceivedEvent MessageReceived;
        public bool IsConnectionOpen = false;

        #region Send/Receive Message
        public async Task SendMessageAsync(Socket socket, object Message)
        {
            try
            {
                byte[] data = new byte[4096];
                data = CommunicationHelper.SerializeObject(Message);
                int result = await socket.SendAsync(new ArraySegment<byte>(data), SocketFlags.None);
                if (result == 0)
                {
                    //error    
                }
            }
            catch (Exception ex)
            {
                MessageDialogHelper.ShowInfoMessage(ex.Message);
            }
        }
        public void SendMessage(Socket socket, object Message)
        {
            byte[] data = CommunicationHelper.SerializeObject(Message);
            socket.BeginSend(data, 0, 4096, SocketFlags.None, null, socket);
        }

        public async Task<object> ReceiveMessageAsync(Socket socket)
        {
            object result = null;
            byte[] receivedData = new byte[4096]; //Modificare
            int receivedBytes = await socket.ReceiveAsync(new ArraySegment<byte>(receivedData), SocketFlags.None);
            if (receivedBytes == 0) return result;
            result = CommunicationHelper.DeserializeObject(receivedData);
            return result;
        }

        public void ReceiveMessage(Socket socket)
        {
            StateObject so = new StateObject();
            so.workSocket = socket;
            socket.BeginReceive(so.buffer, 0, StateObject.BUFFER_SIZE, SocketFlags.None, new AsyncCallback(ReceiveMessageHandle), so);
        }

        private void ReceiveMessageHandle(IAsyncResult ar)
        {
            try
            {
                StateObject so = (StateObject)ar.AsyncState;
                int read = so.workSocket.EndReceive(ar);
                if(read > 0)
                {
                    if(IsConnectionOpen)so.workSocket.BeginReceive(so.buffer, 0, StateObject.BUFFER_SIZE, SocketFlags.None, new AsyncCallback(ReceiveMessageHandle), so);
                    GetObjectReceived(so.buffer);
                }
            }catch(Exception ex) { }
        }
        
        private void GetObjectReceived(byte[] data)
        {
            if(data.Length == 0) return;
            object result = CommunicationHelper.DeserializeObject(data);
            OnMessageReceived(result);
        }
        #endregion

        #region Messages Events
        private void OnMessageReceived(object Message)
        {
            MessageReceived?.Invoke(Message);
        }
        #endregion

        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
