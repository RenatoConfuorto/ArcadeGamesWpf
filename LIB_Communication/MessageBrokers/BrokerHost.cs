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

namespace LIB_Com.MessageBrokers
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
            User.UserSeq = 1;
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
                int newUserSeq = GetLastUserSeq() + 1;
                OnlineClient client = new OnlineClient()
                {
                    socket = clientSocket,
                    user = new OnlineUser()
                    {
                        UserName = String.Empty, //da ricevere
                        UserSeq = newUserSeq,
                        UserId = Guid.NewGuid(),
                    }
                };
                //mandare messaggio di conferma
                SendDataConfirmation message = new SendDataConfirmation(client.user.UserId, client.user.UserSeq);

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

        public void SendToClients(object message)
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

        private int GetLastUserSeq()
        {
            if (clients.Count == 0)
                return 1;
            int iRetVal = clients.Max(c => c.user.UserSeq);
            return iRetVal == 0 ? 1 : iRetVal;
        }
    }
}
