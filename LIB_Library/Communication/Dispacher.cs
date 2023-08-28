using Core.Helpers;
using LIB.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LIB.Communication
{
    public class DispacherBase
    {
        protected Socket _socket;
        public event MessageReceivedEvent MessageReceived;

        #region Send/Receive Message
        public async Task SendMessage(Socket socket, object Message, int messageLength)
        {
            try
            {
                byte[] data = new byte[messageLength];
                data = CommunicationHelper.SerializeObject(Message);
                int result = await socket.SendAsync(new ArraySegment<byte>(data), SocketFlags.None);
                if (result != messageLength)
                {
                    //error    
                }
            }catch(Exception ex)
            {
                MessageDialogHelper.ShowInfoMessage(ex.Message);
            }
        }

        public async Task ReceiveMessage(Socket socket)
        {
            byte[] receivedData = new byte[65536]; //Modificare
            int receivedBytes = await socket.ReceiveAsync(new ArraySegment<byte>(receivedData), SocketFlags.None);
            if (receivedBytes == 0) return;
            object result = CommunicationHelper.DeserializeObject(receivedData);
            OnMessageReceived(result);
        }
        #endregion

        #region Messages Events
        private void OnMessageReceived(object Message)
        {
            MessageReceived?.Invoke(Message);

        } 
        #endregion
    }
}
