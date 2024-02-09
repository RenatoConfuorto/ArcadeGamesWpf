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
        private string _newMessageText;
        private bool _isLobbyChatEnabled = true; // TODO manage lobby chat enabled
        #endregion

        #region Command
        public RelayCommand SendChatMessage { get; set; }
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
            }
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
                if (e.NewUser.UserId == client.user.UserId) continue; //the new user will receive the LobbyInfoMessage with the user list
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
        //private void OnClientDisconnected()
        //{
        //    //Task.Run(() =>  _brokerHost.StartListening(CommunicationCnst.DEFAULT_PORT));
        //}

        #endregion

        #region Commands methods
        private void SendChatMessageExecute(object paran)
        {

        }
        private bool SendChatMessageCanExecute(object paran) => IsLobbyChatEnabled;
        #endregion
    }
}
