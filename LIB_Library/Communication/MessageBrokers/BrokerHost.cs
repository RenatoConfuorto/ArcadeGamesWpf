using Core.Helpers;
using LIB.Communication.Constants;
using LIB.Communication.Events;
using LIB.Communication.Messages;
using LIB.Communication.Messages.Base;
using LIB.Entities;
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
using static LIB.Communication.Constants.CommunicationCnst;

namespace LIB.Communication.MessageBrokers
{
    public class BrokerHost : BrokerBase
    {
        /// <summary>
        /// client list connected to the host
        /// </summary>
        public List<OnlineClient> clients = new List<OnlineClient>();

        #region Events
        public event EventHandler<LobbyInfoRequestedEventArgs> LobbyInfoRequestedEvent;
        public event EventHandler<NewOnlineUserEventArgs> NewOnlineUserEvent;
        #endregion

        #region Constructor
        /// <summary>
        /// Create a new instance of the BrokerHost and assign a GUID to the host
        /// </summary>
        /// <param name="UserName"></param>
        public BrokerHost(string UserName)
            :base(UserName) 
        {
            LocalUserId = Guid.NewGuid();
        }
        #endregion

        #region Start Server / Accept Connections
        public void RunServer()
        {
            _socket.Bind(new IPEndPoint(IPAddress.Any, DEFAULT_PORT));
            _socket.Listen(1);
            _socket.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }
        /// <summary>
        /// Callback of the method BeginAccept in <method cref="RunServer"/> that gets the client socket
        /// </summary>
        /// <param name="ar"></param>
        private void AcceptCallback(IAsyncResult ar)
        {
            Socket clientSocket;
            try
            {
                clientSocket = _socket.EndAccept(ar);
                //creare oggetto OnlineClient e mandare la conferma

                OnlineClient client = new OnlineClient()
                {
                    socket = clientSocket,
                    user = new OnlineUser()
                    {
                        UserName = String.Empty, //da ricevere
                        UserId = Guid.NewGuid(),
                    }
                };
                //mandare messaggio di conferma
                SendDataConfirmation message = new SendDataConfirmation(client.user.UserId);

                SendMessage(clientSocket, message);
                clientSocket.BeginReceive(buffer, 0, DEFAULT_BUFFER_SIZE, SocketFlags.None, new AsyncCallback(OnConfirmationReceived), client);
            }catch(ObjectDisposedException odEx)
            {
                return;
            }catch(Exception ex)
            {
                return;
            }
        }
        /// <summary>
        /// Callback of the method BeginReceive in <method cref="AcceptCallback(IAsyncResult)"/> that gets the <class cref="SendUserNameToHost"/> message
        /// </summary>
        /// <param name="ar"></param>
        private void OnConfirmationReceived(IAsyncResult ar)
        {
            OnlineClient client = (OnlineClient)ar.AsyncState;
            Socket clientSocket = client.socket;
            int received = clientSocket.EndReceive(ar);
            SendUserNameToHost receivedMessage = (SendUserNameToHost)GetMessageFromBytes(received);

            client.user.UserName = receivedMessage.UserName;

            clients.Add(client);
            //evento newUser => notificare al viewModel del nuovo utente
            NewOnlineUserEvent?.Invoke(this, new NewOnlineUserEventArgs(client.user));
            //recuperare le informazioni della lobby dal viewModel
            LobbyInfoRequestedEvent?.Invoke(this, new LobbyInfoRequestedEventArgs(client));

            _socket.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }

        public void SendLobbyInfo(OnlineClient client, IEnumerable<OnlineUser> users)
        {
            LobbyInfoMessage infoMessage = new LobbyInfoMessage(CommunicationHelper.GetLocalIpAddress().ToString(), users);
            SendMessage(client.socket, infoMessage);
            //TODO Check client connection status

            //start listening for messager
            StartReceive(client.socket);
        }
        #endregion

        #region IDisposable
        public override void Dispose()
        {
            foreach(OnlineClient client in clients)
            {
                client.socket.Dispose();
            }
            base.Dispose();
        }
        #endregion

        //public bool IsSearchingConnection = false;
        //public ClientDisconnectedEvent ClientDisconnectedEvent;
        //public Socket ClientSocket;
        //private Thread _chekingThread;
        //public void RunServer(int port)
        //{
        //    IPAddress IpAny = IPAddress.Any;
        //    _socket = CommunicationHelper.InitSocket(IpAny);
        //    _socket.Bind(new IPEndPoint(IPAddress.Any, port));
        //    _socket.Listen(10);

        //    StartListening(port);
        //}

        //public void StartListening(int port)
        //{
        //    if(_chekingThread != null)_chekingThread.Abort();
        //    IsConnectionOpen = true;
        //    IsSearchingConnection = true;
        //    Task.Run(async () =>
        //    {
        //        await StartLookingForConnection(port);
        //    });
        //}
        ////public void StopServer()
        ////{
        ////    IsSearchingConnection = false;
        ////}
        //private async Task WaitConnection()
        //{
        //    //_socket.BeginAccept(new AsyncCallback(HandleIncomingConnection), _socket);
        //    ClientSocket = await _socket.AcceptAsync();
        //    await HandleIncomingConnection(ClientSocket);
        //}

        //private async Task HandleIncomingConnection(Socket handler)
        //{
        //    //if (!IsSearchingConnection) return;
        //    //Task.Run(() => GetNewConnection(asyncResult));
        //    //_socket.EndAccept(asyncResult);
        //    //if(IsSearchingConnection)WaitConnection();
        //    //aspettare il messaggio di conferma
        //    bool IsConnectionConfirmed = await GetConnectionConfirmationMessage(handler);
        //    TrackClientConnectionStatus(handler);
        //    if (!IsConnectionConfirmed) return;
        //    //mettere in ascolto dei messaggi del client in un thread parallelo per far continuare l'esecuzione del programma
        //    ReceiveMessage(handler);
        //}
        ////private async Task GetNewConnection(IAsyncResult asyncResult)
        ////{
        ////}
        //private async Task<bool> GetConnectionConfirmationMessage(Socket connection)
        //{
        //    bool IsConnectionConfirmed = false;
        //    byte[] data = new byte[4096];
        //    int receivedBytes = await connection.ReceiveAsync(new ArraySegment<byte>(data), SocketFlags.None);
        //    if(receivedBytes == 0) return false;
        //    ConnectionConfirmation message = (ConnectionConfirmation)CommunicationHelper.DeserializeObject(data);
        //    if(message != null)
        //    {
        //        IsConnectionConfirmed = true;
        //        ConnectionConfirmed confirmationMessage = new ConnectionConfirmed(message.TestCode);
        //        await SendMessageAsync(connection, confirmationMessage);
        //    }
        //    return IsConnectionConfirmed;

        //}
        //private void TrackClientConnectionStatus(Socket socket)
        //{
        //    try
        //    {
        //        _chekingThread = new Thread(() =>
        //        {
        //            CheckClientConnectionStatus(socket);
        //        });
        //        _chekingThread.Start();
        //    }
        //    catch(ThreadAbortException TAex) { }
        //    catch(Exception ex)
        //    {
        //        MessageDialogHelper.ShowInfoMessage(ex.Message);
        //    }
        //}
        //private void CheckClientConnectionStatus(Socket socket)
        //{
        //    bool threadActive = true;
        //    while (threadActive)
        //    {
        //        threadActive = CommunicationHelper.CheckIfClientIsConnected(socket);
        //        Thread.Sleep(CommunicationCnst.CONNECTION_CHECK_INTERVAL);
        //    }
        //    OnClientDisconnected();
        //}
        //private void OnClientDisconnected()
        //{
        //    ClientSocket = null;
        //    ClientDisconnectedEvent?.Invoke();
        //    _chekingThread.Abort();
        //}

        ////private async Task ReceiveMessageServer(Socket connection)
        ////{
        ////    ReceiveMessage(connection);
        ////    if (IsConnectionOpen) await ReceiveMessageServer(connection);
        ////}
        //private async Task StartLookingForConnection(int port)
        //{
        //    try
        //    {
        //        await WaitConnection();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageDialogHelper.ShowInfoMessage(ex.Message);
        //    }
        //}

        //public override void Dispose()
        //{
        //    _socket?.Dispose();
        //    base.Dispose();
        //}
    }
}
