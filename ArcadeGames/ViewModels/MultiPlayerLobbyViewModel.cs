﻿using ArcadeGames.Views;
using Core.Attributes;
using Core.Commands;
using Core.Helpers;
using LIB.Communication.Constants;
using LIB.Communication.Events;
using LIB.Communication.Messages;
using LIB.Communication.MessageBrokers;
using LIB.Constants;
using LIB.Entities;
using LIB.Helpers;
using LIB.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace ArcadeGames.ViewModels
{
    [ViewRef(typeof(MultiPlayerLobbyView))]
    public class MultiPlayerLobbyViewModel : ContentViewModel
    {
        #region Private Fields
        private CommunicationCnst.Mode _userMode;
        private BrokerHost _brokerHost;
        private BrokerClient _brokerClient;
        private ObservableCollection<OnlineUser> _users = new ObservableCollection<OnlineUser>();
        private string _hostIp;
        #endregion

        #region Command
        #endregion

        #region Public Properties
        public string HostIp
        {
            get => _hostIp;
            set => SetProperty(ref _hostIp, value);
        }
        public ObservableCollection<OnlineUser> Users
        {
            get => _users;
            set => SetProperty(ref _users, value);
        }
        #endregion

        #region Constructor
        public MultiPlayerLobbyViewModel(object param) : base(ViewNames.MultiPlayerLobby, ViewNames.MultiPlayerForm, param) { }
        #endregion

        #region Override Methods
        protected override void OnInitialized()
        {
            base.OnInitialized();
        }
        protected override void InitCommands()
        {
            base.InitCommands();
        }
        protected override void GetViewParameter()
        {
            if(ViewParam != null)
            {
                if (ViewParam is Dictionary<string, object>)
                {
                    Dictionary<string, object> parameters = (Dictionary<string, object>)ViewParam;
                    string parameterName = "Mode";
                    object tempObj;
                    if (parameters.TryGetValue(parameterName, out tempObj))
                    {
                        if (tempObj is CommunicationCnst.Mode)
                        {
                            _userMode = (CommunicationCnst.Mode)tempObj;
                        }
                    }
                    parameterName = "Broker";
                    if (parameters.TryGetValue(parameterName, out tempObj))
                    {
                        if (_userMode == CommunicationCnst.Mode.Host)
                        {
                            HostIp = CommunicationHelper.GetLocalIpAddress().ToString();
                            if (tempObj is BrokerHost)
                            {
                                _brokerHost = (BrokerHost)tempObj;
                                _brokerHost.LobbyInfoRequestedEvent += OnLobbyInfoRequestedEvent;
                                _brokerHost.NewOnlineUserEvent += OnNewOnlineUserEvent;
                                _brokerHost.MessageReceivedEvent += OnMessageReceivedEvent;
                                AddUser(_brokerHost.User);
                                _brokerHost.RunServer();
                                
                            }
                        }
                        else if (_userMode == CommunicationCnst.Mode.Client)
                        {
                            if (tempObj is BrokerClient)
                            {
                                _brokerClient = (BrokerClient)tempObj;
                                _brokerClient.MessageReceivedEvent += OnMessageReceivedEvent;
                                //get client parameters
                                parameterName = "HostIp";
                                if(parameters.TryGetValue(parameterName, out tempObj))
                                {
                                    if(tempObj is string hostIp)
                                    {
                                        HostIp = hostIp;
                                    }
                                }
                                parameterName = "Users";
                                if(parameters.TryGetValue(parameterName, out tempObj))
                                {
                                    if(tempObj is IEnumerable<OnlineUser> users)
                                    {
                                        Users = new ObservableCollection<OnlineUser>(users);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    MessageDialogHelper.ShowInfoMessage("I parametri in ingresso non sono nel formato corretto");
                }
            }
        }
        protected override void SetCommandExecutionStatus()
        {
            base.SetCommandExecutionStatus();
        }
        public override void Dispose()
        {
            _brokerHost?.Dispose();
            _brokerClient?.Dispose();
            base.Dispose();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Add a user to the Users ObservableCollection (avoiding the error if ObservableCollection is changed from diffrent thread)
        /// </summary>
        /// <param name="user"></param>
        private void AddUser(OnlineUser user)
        {
            List<OnlineUser> users = Users.ToList();
            users.Add(user);
            Users = new ObservableCollection<OnlineUser>(users);
        }
        #region Host Methods
        private void OnLobbyInfoRequestedEvent(object sender, LobbyInfoRequestedEventArgs e)
        {
            _brokerHost.SendLobbyInfo(e.client, Users);
        }

        private void OnNewOnlineUserEvent(object sender, NewOnlineUserEventArgs e)
        {
            List<OnlineUser> users = Users.ToList();
            users.Add(e.NewUser);
            Users = new ObservableCollection<OnlineUser>(users);
            //send new users list to clients
            foreach(OnlineClient client in _brokerHost.clients)
            {
                if (e.NewUser.UserId == client.user.UserId) continue; //the new user will receive the LobbyInfoMessage whit the user list
                SendUpdatedUserList message = new SendUpdatedUserList(Users);
                _brokerHost.SendMessage(client.socket, message);
            }
        }
        #endregion

        #region Client Methods

        private void OnMessageReceivedEvent(object sender, MessageReceivedEventArgs e)
        {

        }
        #endregion
        private void OnClientDisconnected()
        {
            //Task.Run(() =>  _brokerHost.StartListening(CommunicationCnst.DEFAULT_PORT));
        }
        #endregion
    }
}
