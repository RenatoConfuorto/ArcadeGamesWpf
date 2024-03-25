using Core.Helpers;
using LIB_Com.Constants;
using LIB_Com.Events;
using LIB_Com.Messages;
using LIB_Com.Messages.Base;
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
using System.Windows.Forms;

namespace LIB_Com.MessageBrokers
{
    public class BrokerClient : BrokerBase
    {
        Thread watchdogThread;
        //bool _isRunning = false;
        bool _isConnected = false;
        #region Events
        public event EventHandler<LobbyInfoReceivedEventArgs> LobbyInfoReceivedEvent;
        #endregion

        #region Constructor
        /// <summary>
        /// Create a new instance of the Broker Client
        /// </summary>
        /// <param name="UserName"></param>
        public BrokerClient(string UserName)
            :base(UserName) 
        {
            logger = LoggerHelper.GetLogger("_brokerClient_log");
        }
        #endregion

        #region Start Client / Connect to Host
        /// <summary>
        /// Connects to the host converting the string into an <type cref="IPAddress"/> type
        /// </summary>
        /// <param name="address"></param>
        public void RunClient(string address)
        {
            IPAddress iPAddress;
            try
            {
                IPAddress.TryParse(address, out iPAddress);
                if (address == null) throw new InvalidCastException("Indirizzo Ip non è nel formato corretto");
                RunClient(iPAddress);
            }
            catch (InvalidCastException iEx)
            {
                logger.LogError(iEx.Message);
                MessageBox.Show(iEx.Message);
            }
        }
        /// <summary>
        /// Connects to the host with the given address
        /// </summary>
        /// <param name="address"></param>
        public void RunClient(IPAddress address)
        {
            ConnectToServer(address);
        }
        /// <summary>
        /// Does a limited amount of tries to connect to the host, see <const cref="CONNECTION_MAX_TRIES"/>
        /// </summary>
        /// <param name="address"></param>
        private void ConnectToServer(IPAddress address)
        {
            int attempts = 0;
            logger.LogInfo("Attempt to connect to host: {0}", address.ToString());
            while (!_socket.Connected && attempts < CONNECTION_MAX_TRIES)
            {
                try
                {
                    attempts++;
                    _socket.Connect(new IPEndPoint(address, DEFAULT_PORT));
                }
                catch (SocketException sEx)
                {
                    logger.LogWarn(sEx.Message);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.Message);
                }
            }
            if (_socket.Connected)
            {
                //connessione avvenuta => ricevere la GUID
                logger.LogDebug("Connection to host successful. Waiting confirmation.");
                _socket.BeginReceive(buffer, 0, DEFAULT_BUFFER_SIZE, SocketFlags.None, new AsyncCallback(ReceiveConfirmationCallBack), _socket);
            }
            else
            {
                logger.LogWarn("Connection to host failed.");
                MessageBox.Show("Connessione con l'host fallita");
            }
        }
        /// <summary>
        /// Callback of the method BeginReceive in <method cref="ConnectToServer(IPAddress)"/>.
        /// Receive the message the the LocalId(GUID) and send to the host the UserName
        /// </summary>
        /// <param name="ar"></param>
        private void ReceiveConfirmationCallBack(IAsyncResult ar)
        {
            Socket socket = (Socket)ar.AsyncState;
            int received = socket.EndReceive(ar);

            SendDataConfirmation receivedMessage = (SendDataConfirmation)GetMessageFromBytes(received);
            logger.LogDebug("Confirmation received from host. UserId/UserSeq {0}/{1}.", receivedMessage.UserId, receivedMessage.UserSeq);
            LocalUserId = receivedMessage.UserId;
            User.UserSeq = receivedMessage.UserSeq;

            //send userName
            SendUserNameToHost responseMessage = new SendUserNameToHost(this.UserName);
            byte[] responseData = CommunicationHelper.SerializeObject(responseMessage);
            SendMessage(socket, responseMessage);
            //socket.Send(responseData);
            //wait lobby info
            socket.BeginReceive(buffer, 0, DEFAULT_BUFFER_SIZE, SocketFlags.None, new AsyncCallback(OnLobbyInfoReceived), socket);
        }

        private void OnLobbyInfoReceived(IAsyncResult ar)
        {
            Socket socket = (Socket )ar.AsyncState;
            int received = socket.EndReceive(ar);
            LobbyInfoMessage infoMessage = (LobbyInfoMessage)GetMessageFromBytes(received);
            logger.LogDebug("Lobby info received from host.");
            LobbyInfoReceivedEvent?.Invoke(this, new LobbyInfoReceivedEventArgs(infoMessage));
            _isConnected = true;
            StartWatchdog();
            StartReceive(socket);
        }

        private void SendDisconnectionToHost()
        {
            ClientDisconnectedMessage message = new ClientDisconnectedMessage(LocalUserId);
            SendToHost(message);
        }
        private void StartWatchdog()
        {
            watchdogThread = new Thread(WatchDogThread);
            watchdogThread.Start();
        }
        private void WatchDogThread()
        {
            logger.LogDebug("Starting watchdog thread.");
            try
            {
                while(_isConnected)
                {
                    SendWatchdog();
                    Thread.Sleep(250);
                }
            }catch(SocketException sEx)
            {
                logger.LogWarn("Error while sending the watchdog.");
                _isConnected = false;
            }catch(Exception ex)
            {
                //logger.LogError(ex.Message);
                logger.LogException(ex);
            }
            finally
            {
                //_isConnected = false;
                logger.LogDebug("Thread watchdog concluded.");
                watchdogThread.Join();
            }
        }
        private void SendWatchdog()
        {
            Watchdog wd = new Watchdog();
            logger.LogDebug("Watchdog sended to host.");
            SendToHost(wd);
        }
        #endregion

        #region Send/Receive from host
        public void SendToHost(object message)
        {
            SendMessage(_socket, message);
        }
        #endregion
        public override void Dispose()
        {
            _isConnected = false;
            SendDisconnectionToHost();
            base.Dispose();
        }
    }
}
