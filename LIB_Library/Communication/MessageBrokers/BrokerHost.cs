using Core.Helpers;
using LIB.Communication.Messages;
using LIB.Communication.Messages.Base;
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
using System.Windows.Documents;

namespace LIB.Communication.MessageBrokers
{
    public class BrokerHost : BrokerBase
    {
        public bool IsSearchingConnection = false;

        public void RunServer(int port)
        {
            IPAddress IpAny = IPAddress.Any;
                    _socket = CommunicationHelper.InitSocket(IpAny);
            StartListening(port);
        }

        public void StartListening(int port)
        {
            IsConnectionOpen = true;
            IsSearchingConnection = true;
            Task.Run(async () =>
            {
                try
                {
                    _socket.Bind(new IPEndPoint(IPAddress.Any, port));
                    _socket.Listen(10);

                    await WaitConnection();
                }
                catch (Exception ex)
                {
                    MessageDialogHelper.ShowInfoMessage(ex.Message);
                }
            });
        }
        public void StopServer()
        {
            IsSearchingConnection = false;
        }
        private async Task WaitConnection()
        {
            //_socket.BeginAccept(new AsyncCallback(HandleIncomingConnection), _socket);
            Socket handler = await _socket.AcceptAsync();
            await HandleIncomingConnection(handler);
        }

        private async Task HandleIncomingConnection(Socket handler)
        {
            //if (!IsSearchingConnection) return;
            //Task.Run(() => GetNewConnection(asyncResult));
            //_socket.EndAccept(asyncResult);
            //if(IsSearchingConnection)WaitConnection();
            //aspettare il messaggio di conferma
            bool IsConnectionConfirmed = await GetConnectionConfirmationMessage(handler);
            if (!IsConnectionConfirmed) return;
            //mettere in ascolto dei messaggi del client in un thread parallelo per far continuare l'esecuzione del programma
            ReceiveMessage(handler);
        }
        //private async Task GetNewConnection(IAsyncResult asyncResult)
        //{
        //}
        private async Task<bool> GetConnectionConfirmationMessage(Socket connection)
        {
            bool IsConnectionConfirmed = false;
            byte[] data = new byte[4096];
            int receivedBytes = await connection.ReceiveAsync(new ArraySegment<byte>(data), SocketFlags.None);
            if(receivedBytes == 0) return false;
            ConnectionConfirmation message = (ConnectionConfirmation)CommunicationHelper.DeserializeObject(data);
            if(message != null)
            {
                IsConnectionConfirmed = true;
                ConnectionConfirmed confirmationMessage = new ConnectionConfirmed(message.TestCode);
                await SendMessageAsync(connection, confirmationMessage);
            }
            return IsConnectionConfirmed;
            
        }

        //private async Task ReceiveMessageServer(Socket connection)
        //{
        //    ReceiveMessage(connection);
        //    if (IsConnectionOpen) await ReceiveMessageServer(connection);
        //}

        public override void Dispose()
        {
            _socket?.Dispose();
            base.Dispose();
        }
    }
}
