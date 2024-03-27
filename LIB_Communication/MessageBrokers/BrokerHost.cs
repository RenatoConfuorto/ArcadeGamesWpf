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
using static LIB_Com.Constants.CommunicationCnst;
using LIB_Com.Interfaces.Communication;

namespace LIB_Com.MessageBrokers
{
    public class BrokerHost : BrokerBase
    {
        /// <summary>
        /// client list connected to the host
        /// </summary>
        public List<OnlineClient> clients = new List<OnlineClient>();
        /// <summary>
        /// List of received watchdogs
        /// </summary>
        List<Watchdog> receivedWatchdogs = new List<Watchdog>();
        Thread thread_checker;

        #region Events
        public event EventHandler<LobbyInfoRequestedEventArgs> LobbyInfoRequestedEvent;
        public event EventHandler<NewOnlineUserEventArgs> NewOnlineUserEvent;
        public event EventHandler<ClientConnectionLostEventArgs> ClientConnectionLost;
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
            User.UserSeq = 1;
            thread_checker = new Thread(HandleWatchDogs);
            thread_checker.IsBackground = true;
            logger = LoggerHelper.GetLogger("_brokerHost_log");
        }
        #endregion

        #region Start Server / Accept Connections
        public void RunServer()
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, DEFAULT_PORT);
            _socket.Bind(endPoint);
            _socket.Listen(1);
            logger.LogInfo($"Begin listen. Port: {DEFAULT_PORT}");
            logger.LogDebug("Begin Accept connections.");
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
                logger.LogDebug($"Received connection request. LocalEndPoint: {clientSocket.LocalEndPoint}, RemoteEndPoint: {clientSocket.RemoteEndPoint}");
                //creare oggetto OnlineClient e mandare la conferma
                int newUserSeq = GetLastUserSeq() + 1;
                OnlineClient client = new OnlineClient()
                {
                    socket = clientSocket,
                    user = new OnlineUser()
                    {
                        UserName = String.Empty, //da ricevere
                        UserSeq = newUserSeq,
                        UserId = Guid.NewGuid(),
                    },
                    lastWatchdog = DateTime.Now // Init last watchdog chekc
                };
                //mandare messaggio di conferma
                SendDataConfirmation message = new SendDataConfirmation(client.user.UserId, client.user.UserSeq);

                SendMessage(clientSocket, message);
                logger.LogDebug($"Confirmation sended. UserId: {client.user.UserId}, UserSeq: {client.user.UserSeq}");
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

            logger.LogDebug("Confirmation received from client. Client user name: {0}", receivedMessage.UserName.ToString());
            client.user.UserName = receivedMessage.UserName;

            clients.Add(client);
            //evento newUser => notificare al viewModel del nuovo utente
            NewOnlineUserEvent?.Invoke(this, new NewOnlineUserEventArgs(client.user));
            logger.LogDebug("Notify lobby of new user with NewOnlineUserEvent.");
            //recuperare le informazioni della lobby dal viewModel
            LobbyInfoRequestedEvent?.Invoke(this, new LobbyInfoRequestedEventArgs(client));

            logger.LogDebug("Start listening for a new connection.");
            _socket.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }

        public void SendLobbyInfo(OnlineClient client, IEnumerable<OnlineUser> users, LobbyStatus lobbyStatus) 
        {
            LobbyInfoMessage infoMessage = new LobbyInfoMessage(CommunicationHelper.GetLocalIpAddress().ToString(), users, lobbyStatus);
            SendMessage(client.socket, infoMessage);
            logger.LogDebug($"Sended lobby info to client. Lobby address: {infoMessage.HostIp}, user count: {infoMessage.Users.Length}");
            // Start checking watchdogs
            #if !DEBUG
            if (!thread_checker.IsAlive)
                thread_checker.Start();
            #endif
            logger.LogDebug("Start listening for messages from client.");
            //start listening for messager
            StartReceive(client.socket);
        }
        #endregion

        #region IDisposable
        public override void Dispose()
        {
            logger.LogDebug("Dispose of BrokerHost");
            NotifyDisconnectHost();
            thread_checker.Abort();
            foreach(OnlineClient client in clients)
            {
                client.socket.Shutdown(SocketShutdown.Both);
                client.socket.Close();
            }
            base.Dispose();
        }
        #endregion
        protected override bool CanRedirectMessage(MessageBase message)
        {
            if(message.MessageCode == (int)CommunicationCnst.Messages.Watchdog)
            {
                //logger.LogDebug($"WatchDog received from client: {message.SenderId}.");
                receivedWatchdogs.Add((Watchdog)message);
                return false;
            }
            return true;
        }
        private void HandleWatchDogs()
        {
            try
            {
                logger.LogDebug("Start checking watchdogs.");
                while(clients.Count > 0)
                {
                    if(receivedWatchdogs.Count > 0)
                    {
                        Watchdog wd = receivedWatchdogs[0];
                        OnlineClient client = clients.Where(c => c.user.UserId == wd.SenderId).FirstOrDefault();
                        client.lastWatchdog = DateTime.Now; // Update last check.
                        receivedWatchdogs.RemoveAt(0);
                    }
                    else
                    { // Avoid a loop in case there are no watchdogs yet
                        Thread.Sleep(50);
                    }
                    CheckClientsStatus();
                }
                thread_checker.Join();
            }catch(ThreadAbortException taEx)
            {

            }catch(Exception ex)
            {

            }
        }
        /// <summary>
        // Check if the client is active using the limit time <constant cref="CommunicationCnst.CLIENT_DELAY_LIMIT" />
        /// </summary>
        /// <param name="client"></param>
        /// <returns>True if client is considered connected</returns>
        private void CheckClientsStatus()
        {
            List<OnlineClient> clients = new List<OnlineClient>(this.clients);
            foreach(OnlineClient client in clients)
            {
                double milliseconds = (DateTime.Now - client.lastWatchdog).TotalMilliseconds;
                //Console.WriteLine($"check - {milliseconds}");
                if (milliseconds > CommunicationCnst.CLIENT_DELAY_LIMIT)
                    HandleClientConnectionLost(client);
            }
        }
        private void HandleClientConnectionLost(OnlineClient client)
        {
            logger.LogInfo($"Connection lost with client {client.user.UserName.ToString()}/{client.user.UserId}.");

            client.socket.Close();
            clients.Remove(client);
            ClientConnectionLost message = new ClientConnectionLost(client.user.UserId);
            SendToClients(message);
            ClientConnectionLost?.Invoke(this, new ClientConnectionLostEventArgs(client.user));
            //HandleClientDisconnected(clientDisconnectedMessage);
        }
        /// <summary>
        /// Sends a message to all clients
        /// </summary>
        /// <param name="message"></param>
        public void SendToClients<T>(T message)
            where T : IMessage
        {
            foreach(OnlineClient client in clients)
            {
                Socket cs = client.socket;
                if(cs != null &&
                    cs.Connected)
                {
                    SendMessage(cs, message);
                }
            }
        }
        /// <summary>
        /// Redirects a message from <class cref="OnlineUser"/> to all other clients
        /// </summary>
        /// <param name="message"></param>
        /// <param name="user"></param>
        public void RedirectToClients<T>(T message)
            where T : IMessage
        {
            Guid senderId = message.SenderId;
            foreach(OnlineClient client in clients)
            {
                if (client.user.UserId == senderId)
                    continue;
                SendMessage(client.socket, message);
            }
        }

        public void NotifyDisconnectHost()
        {
            HostDisconnectedMessage message = new HostDisconnectedMessage();
            SendToClients(message);
        }

        public bool HandleClientDisconnected(ClientDisconnectedMessage message)
        {
            bool retVal = false;
            OnlineClient client = clients.Where(c => c.user.UserId == message.UserId).FirstOrDefault();
            logger.LogInfo($"Client disconnection received from client {client.user.UserName.ToString()}/{client.user.UserId}");
            if(client != null)
            {
                // Remove client from clients list
                clients.Remove(client);
                // Notify the other clients of the disconnection
                RedirectToClients(message);
                retVal = true;
            }
            return retVal;
        }

        private int GetLastUserSeq()
        {
            if (clients.Count == 0)
                return 1;
            int iRetVal = clients.Max(c => c.user.UserSeq);
            return iRetVal == 0 ? 1 : iRetVal;
        }
    }
}
