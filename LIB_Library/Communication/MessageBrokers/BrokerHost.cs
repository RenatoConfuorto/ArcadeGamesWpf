using Core.Helpers;
using LIB.Communication.Constants;
using LIB.Communication.Events;
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
        public ClientDisconnectedEvent ClientDisconnectedEvent;
        public Socket ClientSocket;
        private Thread _chekingThread;
        public void RunServer(int port)
        {
            IPAddress IpAny = IPAddress.Any;
            _socket = CommunicationHelper.InitSocket(IpAny);
            _socket.Bind(new IPEndPoint(IPAddress.Any, port));
            _socket.Listen(10);

            StartListening(port);
        }

        public void StartListening(int port)
        {
            if(_chekingThread != null)_chekingThread.Abort();
            IsConnectionOpen = true;
            IsSearchingConnection = true;
            Task.Run(async () =>
            {
                await StartLookingForConnection(port);
            });
        }
        //public void StopServer()
        //{
        //    IsSearchingConnection = false;
        //}
        private async Task WaitConnection()
        {
            //_socket.BeginAccept(new AsyncCallback(HandleIncomingConnection), _socket);
            ClientSocket = await _socket.AcceptAsync();
            await HandleIncomingConnection(ClientSocket);
        }

        private async Task HandleIncomingConnection(Socket handler)
        {
            //if (!IsSearchingConnection) return;
            //Task.Run(() => GetNewConnection(asyncResult));
            //_socket.EndAccept(asyncResult);
            //if(IsSearchingConnection)WaitConnection();
            //aspettare il messaggio di conferma
            bool IsConnectionConfirmed = await GetConnectionConfirmationMessage(handler);
            TrackClientConnectionStatus(handler);
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
        private void TrackClientConnectionStatus(Socket socket)
        {
            try
            {
                _chekingThread = new Thread(() =>
                {
                    CheckClientConnectionStatus(socket);
                });
                _chekingThread.Start();
            }
            catch(ThreadAbortException TAex) { }
            catch(Exception ex)
            {
                MessageDialogHelper.ShowInfoMessage(ex.Message);
            }
        }
        private void CheckClientConnectionStatus(Socket socket)
        {
            bool threadActive = true;
            while (threadActive)
            {
                threadActive = CommunicationHelper.CheckIfClientIsConnected(socket);
                Thread.Sleep(CommunicationCnst.CONNECTION_CHECK_INTERVAL);
            }
            OnClientDisconnected();
        }
        private void OnClientDisconnected()
        {
            ClientSocket = null;
            ClientDisconnectedEvent?.Invoke();
            _chekingThread.Abort();
        }

        //private async Task ReceiveMessageServer(Socket connection)
        //{
        //    ReceiveMessage(connection);
        //    if (IsConnectionOpen) await ReceiveMessageServer(connection);
        //}
        private async Task StartLookingForConnection(int port)
        {
            try
            {
                await WaitConnection();
            }
            catch (Exception ex)
            {
                MessageDialogHelper.ShowInfoMessage(ex.Message);
            }
        }

        public override void Dispose()
        {
            _socket?.Dispose();
            base.Dispose();
        }
    }
}
