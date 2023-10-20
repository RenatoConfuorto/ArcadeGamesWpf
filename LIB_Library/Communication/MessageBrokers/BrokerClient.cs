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
using static LIB.Communication.Constants.CommunicationCnst;

namespace LIB.Communication.MessageBrokers
{
    public class BrokerClient : BrokerBase
    {
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
                MessageDialogHelper.ShowInfoMessage(iEx.Message);
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
            while (!_socket.Connected && attempts < CONNECTION_MAX_TRIES)
            {
                try
                {
                    attempts++;
                    _socket.Connect(new IPEndPoint(address, DEFAULT_PORT));
                }
                catch (SocketException sEx)
                {

                }
                catch (Exception ex)
                {

                }
            }
            if (_socket.Connected)
            {
                //connessione avvenuta => ricevere la GUID
                _socket.BeginReceive(buffer, 0, DEFAULT_BUFFER_SIZE, SocketFlags.None, new AsyncCallback(ReceiveConfirmationCallBack), _socket);
            }
            else
            {
                MessageDialogHelper.ShowInfoMessage("Connessione con l'host fallita");
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

            LocalUserId = receivedMessage.UserId;

            //sned userName
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

            LobbyInfoReceivedEvent?.Invoke(this, new LobbyInfoReceivedEventArgs(infoMessage));
            StartReceive(socket);
        }
        #endregion
        //public async Task<bool> RunClient(int port, string IpAddress)
        //{
        //    try
        //    {
        //        return await RunClient(port, IPAddress.Parse(IpAddress));
        //    }
        //    catch(Exception ex)
        //    {
        //        MessageDialogHelper.ShowInfoMessage(ex.Message);
        //        return false;
        //    }
        //}
        //public async Task<bool> RunClient(int port, IPAddress IpAddress)
        //{
        //    int tries = 0;
        //    Exception lastEx = null;
        //    _socket = CommunicationHelper.InitSocket(IpAddress);
        //    while (tries < CommunicationCnst.CONNECTION_MAX_TRIES && !IsConnectionOpen)
        //    {
        //        try
        //        {
        //            await _socket.ConnectAsync(new IPEndPoint(IpAddress, port));
        //            if (_socket.Connected)
        //            {
        //                if (await ConfirmConnection())
        //                {
        //                    IsConnectionOpen = true;
        //                    StartListening();
        //                }
        //            }
        //            return IsConnectionOpen;
        //        }
        //        catch (Exception ex)
        //        {
        //            //aspettare per provare a riconnettersi
        //            tries++;
        //            lastEx = ex;
        //            Thread.Sleep(CommunicationCnst.CONNECTION_RETRY_WAIT);
        //        }
        //    }
        //    throw lastEx;
        //}

        ////public async Task<bool> RunClient(int port, IPAddress IpAddress)
        ////{
        ////    int tries = 0;
        ////    _socket = CommunicationHelper.InitSocket(IpAddress);
        ////    CommunicationHelper.ExecuteRecursiveTry(this.GetType().GetMethod("TryConnect"), _socket);
        ////    return IsConnectionOpen;
        ////}

        ////public async Task TryConnect(int port, IPAddress IpAddress)
        ////{
        ////    await _socket.ConnectAsync(new IPEndPoint(IpAddress, port));
        ////    if (_socket.Connected)
        ////    {
        ////        if (await ConfirmConnection())
        ////        {
        ////            IsConnectionOpen = true;
        ////            StartListening();
        ////        }
        ////    }
        ////}

        //public void CloseConnection()
        //{
        //    IsConnectionOpen = false;
        //    _socket.Shutdown(SocketShutdown.Both);
        //    _socket.Close();
        //}

        //public void StartListening()
        //{
        //    //Task.Run(async () =>
        //    //{
        //    //    await ReceiveMessageClient();
        //    //});
        //    ReceiveMessage(_socket);
        //}

        //private async Task<bool> ConfirmConnection()
        //{
        //    bool IsConnectionCofirmed = false;
        //    int tries = 0;
        //    Exception lastEx = null;
        //    ConnectionConfirmation confirmationMessage = new ConnectionConfirmation();
        //    while(tries < CommunicationCnst.CONNECTION_MAX_TRIES)
        //    {
        //        try
        //        {
        //            //send confirmation request
        //            await SendMessageAsync(_socket, confirmationMessage);
        //            //wait for responde message
        //            object responseMessage = await ReceiveMessageAsync(_socket);
        //            if(responseMessage != null)
        //            {
        //                ConnectionConfirmed response = responseMessage as ConnectionConfirmed;
        //                if (response.TestCode == confirmationMessage.TestCode)
        //                {
        //                    IsConnectionCofirmed = true;
        //                }
        //            }
        //            return IsConnectionCofirmed;
        //        }catch(Exception ex)
        //        {
        //            tries++;
        //            lastEx = ex;
        //            MessageDialogHelper.ShowInfoMessage($"{ex.Message}");
        //        }
        //    }
        //    throw lastEx;
        //}
        ////private async Task ReceiveMessageClient()
        ////{
        ////    await ReceiveMessage(_socket);
        ////    if(IsConnectionOpen) await ReceiveMessageClient();
        ////}

        //public override void Dispose()
        //{
        //    _socket?.Shutdown(SocketShutdown.Both);
        //    _socket?.Dispose();
        //    base.Dispose();
        //}
    }
}
