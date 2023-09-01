using Core.Helpers;
using LIB.Communication.Constants;
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

namespace LIB.Communication.MessageBrokers
{
    public class BrokerClient : BrokerBase
    {

        public async Task<bool> RunClient(int port, string IpAddress)
        {
            try
            {
                return await RunClient(port, IPAddress.Parse(IpAddress));
            }
            catch(Exception ex)
            {
                MessageDialogHelper.ShowInfoMessage(ex.Message);
                return false;
            }
        }
        public async Task<bool> RunClient(int port, IPAddress IpAddress)
        {
            int tries = 0;
            Exception lastEx = null;
            _socket = CommunicationHelper.InitSocket(IpAddress);
            while (tries < CommunicationCnst.CONNECTION_MAX_TRIES && !IsConnectionOpen)
            {
                try
                {
                    await _socket.ConnectAsync(new IPEndPoint(IpAddress, port));
                    if (_socket.Connected)
                    {
                        if (await ConfirmConnection())
                        {
                            IsConnectionOpen = true;
                            StartListening();
                        }
                    }
                    return IsConnectionOpen;
                }
                catch (Exception ex)
                {
                    //aspettare per provare a riconnettersi
                    tries++;
                    lastEx = ex;
                    Thread.Sleep(CommunicationCnst.CONNECTION_RETRY_WAIT);
                }
            }
            throw lastEx;
        }

        //public async Task<bool> RunClient(int port, IPAddress IpAddress)
        //{
        //    int tries = 0;
        //    _socket = CommunicationHelper.InitSocket(IpAddress);
        //    CommunicationHelper.ExecuteRecursiveTry(this.GetType().GetMethod("TryConnect"), _socket);
        //    return IsConnectionOpen;
        //}

        //public async Task TryConnect(int port, IPAddress IpAddress)
        //{
        //    await _socket.ConnectAsync(new IPEndPoint(IpAddress, port));
        //    if (_socket.Connected)
        //    {
        //        if (await ConfirmConnection())
        //        {
        //            IsConnectionOpen = true;
        //            StartListening();
        //        }
        //    }
        //}

        public void CloseConnection()
        {
            IsConnectionOpen = false;
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
        }

        public void StartListening()
        {
            //Task.Run(async () =>
            //{
            //    await ReceiveMessageClient();
            //});
            ReceiveMessage(_socket);
        }

        private async Task<bool> ConfirmConnection()
        {
            bool IsConnectionCofirmed = false;
            int tries = 0;
            Exception lastEx = null;
            ConnectionConfirmation confirmationMessage = new ConnectionConfirmation();
            while(tries < CommunicationCnst.CONNECTION_MAX_TRIES)
            {
                try
                {
                    //send confirmation request
                    await SendMessageAsync(_socket, confirmationMessage);
                    //wait for responde message
                    object responseMessage = await ReceiveMessageAsync(_socket);
                    if(responseMessage != null)
                    {
                        ConnectionConfirmed response = responseMessage as ConnectionConfirmed;
                        if (response.TestCode == confirmationMessage.TestCode)
                        {
                            IsConnectionCofirmed = true;
                        }
                    }
                    return IsConnectionCofirmed;
                }catch(Exception ex)
                {
                    tries++;
                    lastEx = ex;
                    MessageDialogHelper.ShowInfoMessage($"{ex.Message}");
                }
            }
            throw lastEx;
        }
        //private async Task ReceiveMessageClient()
        //{
        //    await ReceiveMessage(_socket);
        //    if(IsConnectionOpen) await ReceiveMessageClient();
        //}

        public override void Dispose()
        {
            _socket?.Dispose();
            base.Dispose();
        }
    }
}
