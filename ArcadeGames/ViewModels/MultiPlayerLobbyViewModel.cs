using ArcadeGames.Views;
using Core.Attributes;
using Core.Commands;
using Core.Helpers;
using LIB_Com.Constants;
using LIB_Com.Events;
using LIB_Com.Messages;
using LIB_Com.MessageBrokers;
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
using LIB_Com.Entities;
using LIB_Com.Messages.Base;
using System.ComponentModel;
using System.Windows.Threading;
using System.Runtime.Remoting.Contexts;

namespace ArcadeGames.ViewModels
{
    [ViewRef(typeof(MultiPlayerLobbyView))]
    public class MultiPlayerLobbyViewModel : ContentViewModel
    {
        #region Private Fields
        private CommunicationCnst.Mode _userMode;
        private BrokerHost _brokerHost;
        private BrokerClient _brokerClient;
        private BindingList<OnlineUser> _users = new BindingList<OnlineUser>();
        private string _hostIp;
        private string _newMessageText;
        private bool _isLobbyChatEnabled = true;
        private string _chatButtonText = "Chat Abilitata";
        private BindingList<LobbyChatMessage> _chatMessages = new BindingList<LobbyChatMessage>();
        #endregion

        #region Command
        public RelayCommand SendChatMessage { get; set; }
        #endregion

        #region Public Properties
        public bool IsUserHost
        {
            get => _userMode == CommunicationCnst.Mode.Host;
        }
        public bool IsUserClient
        {
            get => _userMode == CommunicationCnst.Mode.Client;
        }

        public string HostIp
        {
            get => _hostIp;
            set => SetProperty(ref _hostIp, value);
        }
        public BindingList<OnlineUser> Users
        {
            get => _users;
            set => SetProperty(ref _users, value);
        }
        public string NewMessageText
        {
            get => _newMessageText;
            set
            {
                SetProperty(ref _newMessageText, value);
                SetCommandExecutionStatus();
            }
        }
        public bool IsLobbyChatEnabled
        {
            get => _isLobbyChatEnabled;
            set
            {
                SetProperty(ref _isLobbyChatEnabled, value);
                SetCommandExecutionStatus();
                if(IsUserHost)
                    SendChatChangedStatus();
                SwitchChatButtonText(value);
                if (!value)
                    NewMessageText = String.Empty;
            }
        }
        public string ChatButtonText
        {
            get => _chatButtonText;
            set => SetProperty(ref _chatButtonText, value);
        }
        public BindingList<LobbyChatMessage> ChatMessages
        {
            get => _chatMessages;
            set => SetProperty(ref _chatMessages, value);
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
            SendChatMessage = new RelayCommand(SendChatMessageExecute, SendChatMessageCanExecute);
        }
        protected override void SetCommandExecutionStatus()
        {
            base.SetCommandExecutionStatus();
            SendChatMessage.RaiseCanExecuteChanged();
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
                            NotifyPropertyChanged(nameof(IsUserHost));
                        }
                    }
                    parameterName = "Broker";
                    if (parameters.TryGetValue(parameterName, out tempObj))
                    {
                        if (IsUserHost)
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
                        else if (IsUserClient)
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
                                        Users = new BindingList<OnlineUser>(users.ToList());
                                    }
                                }
                                parameterName = "ChatStatus";
                                if(parameters.TryGetValue(parameterName, out tempObj))
                                {
                                    if(tempObj is bool chatStatus)
                                    {
                                        IsLobbyChatEnabled = chatStatus;
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
        public override void Dispose()
        {
            _brokerHost?.Dispose();
            _brokerClient?.Dispose();
            base.Dispose();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Add a user to the Users BindingList (avoiding the error if BindingList is changed from diffrent thread)
        /// </summary>
        /// <param name="user"></param>
        private void AddUser(OnlineUser user)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                Users.Add(user);
            });
        }
        /// <summary>
        /// Add a message to the ChatMessages BindingList (avoiding the error if BindingList is changed from diffrent thread)
        /// </summary>
        /// <param name="message"></param>
        private void AddMessage(LobbyChatMessage message)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                ChatMessages.Add(message);
            });
        }

        private void SwitchChatButtonText(bool value)
        {
            if (value)
                ChatButtonText = "Chat Abilitata";
            else
                ChatButtonText = "Chat Disabilitata";
        }
        #region Host Methods
        private void OnLobbyInfoRequestedEvent(object sender, LobbyInfoRequestedEventArgs e)
        {
            _brokerHost.SendLobbyInfo(e.client, Users, IsLobbyChatEnabled);
        }

        private void OnNewOnlineUserEvent(object sender, NewOnlineUserEventArgs e)
        {
            AddUser(e.NewUser);
            //send new users list to clients
            foreach(OnlineClient client in _brokerHost.clients)
            {
                if (e.NewUser.UserId == client.user.UserId) continue; //the new user will receive the LobbyInfoMessage with the user list
                SendUpdatedUserList message = new SendUpdatedUserList(Users.OrderBy(u => u.UserSeq));
                _brokerHost.SendMessage(client.socket, message);
            }
        }

        private void SendChatChangedStatus()
        {
            ChangeLobbyChatStatus message = new ChangeLobbyChatStatus(IsLobbyChatEnabled);
            _brokerHost.SendToClients(message);
        }
        #endregion

        #region Client Methods

        #endregion
        private void OnMessageReceivedEvent(object sender, MessageReceivedEventArgs e)
        {
            MessageBase message = (MessageBase)e.MessageReceived;
            switch (message.MessageCode)
            {
                case (int)CommunicationCnst.Messages.SendUpdatedUserList:
                    HandleUpdateUserListMessage(message as  SendUpdatedUserList);
                    break;
                case (int)CommunicationCnst.Messages.LobbyChatMessage:
                    HandleNewChatMessage(message as LobbyChatMessage);
                    break;
                case (int)CommunicationCnst.Messages.ChangeLobbyChatStatus:
                    HandleNewChatStatus(message as ChangeLobbyChatStatus);
                    break;
            }
        }
        private void HandleNewChatMessage(LobbyChatMessage message)
        {
            AddMessage(message);
            if (IsUserHost)
                _brokerHost.RedirectToClients(message);
        }
        private void HandleUpdateUserListMessage(SendUpdatedUserList message)
        {
            Users = new BindingList<OnlineUser>(message.Users);
        }
        private void HandleNewChatStatus(ChangeLobbyChatStatus message)
        {
            if (IsUserClient)
                IsLobbyChatEnabled = message.bChatStatus;
        }

        #endregion

        #region Commands methods
        private void SendChatMessageExecute(object paran)
        {
            LobbyChatMessage message = new LobbyChatMessage(null, NewMessageText);
            //message.TextMessage = NewMessageText;
            if (IsUserClient)
            {
                message.User = _brokerClient.User;
                AddMessage(message);
                _brokerClient.SendToHost(message);
            }
            else
            {
                message.User = _brokerHost.User;
                AddMessage(message);
                _brokerHost.SendToClients(message);
            }
            // Clean Message box
            NewMessageText = String.Empty;
        }
        private bool SendChatMessageCanExecute(object paran) => IsLobbyChatEnabled && !String.IsNullOrWhiteSpace(NewMessageText);
        #endregion
    }
}
