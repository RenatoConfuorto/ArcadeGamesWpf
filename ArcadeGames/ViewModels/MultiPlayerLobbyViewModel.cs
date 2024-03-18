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
using LIB_Com.ViewModels;
using static LIB_Com.Constants.OnlineGamesDefs;
using System.Windows.Controls;

namespace ArcadeGames.ViewModels
{
    [ViewRef(typeof(MultiPlayerLobbyView))]
    public class MultiPlayerLobbyViewModel : OnlineViewModelBase
    {
        #region Private Fields
        private string _newMessageText;
        private bool _isLobbyChatEnabled = true;
        private string _chatButtonText = "Chat Abilitata";
        private BindingList<LobbyChatMessage> _chatMessages = new BindingList<LobbyChatMessage>();
        private OnlineGame _selectedGame;
        public UserControl _settingsControl;
        #endregion

        #region Command
        public RelayCommand SendChatMessage { get; set; }
        public RelayCommand RemovePlayerTime { get; set; }
        public RelayCommand AddPlayerTime { get; set; }
        #endregion

        #region Public Properties
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
                    SendLobbyStatusToClients();
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
        public List<OnlineGame> Games
        {
            get => OnlineGamesDefs.Games;
        }
        public OnlineGame SelectedGame
        {
            get => _selectedGame;
            set
            {
                SetProperty(ref _selectedGame, value);
                NotifyPropertyChanged(nameof(GameSettings));
                NotifyPropertyChanged(nameof(PlayersTime));
                SetCommandExecutionStatus();
                OnSelectedGameChanged();
            }
        }
        public OnlineSettingsBase GameSettings
        {
            get => SelectedGame?.GameSettings;
        }
        public int PlayersTime
        {
            get => (int)GameSettings?.PlayersTime;
            set
            {
                if(GameSettings != null)
                {
                    GameSettings.PlayersTime = value;
                    NotifyPropertyChanged();
                    SetCommandExecutionStatus();
                    SendLobbyStatusToClients();
                }
            }
        }
        public UserControl SettingsControl
        {
            get => _settingsControl;
            set => SetProperty(ref _settingsControl, value);
        }
        #endregion

        #region Constructor
        public MultiPlayerLobbyViewModel(object param) 
            : base(ViewNames.MultiPlayerLobby, ViewNames.MultiPlayerForm, param) { }
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
            RemovePlayerTime = new RelayCommand(RemovePlayerTimeExecute, RemovePlayerTimeCanExecute);
            AddPlayerTime = new RelayCommand(AddPlayerTimeExecute, AddPlayerTimeCanExecute);
        }
        protected override void SetCommandExecutionStatus()
        {
            base.SetCommandExecutionStatus();
            SendChatMessage.RaiseCanExecuteChanged();
            RemovePlayerTime.RaiseCanExecuteChanged();
            AddPlayerTime.RaiseCanExecuteChanged();
        }
        protected override void GetViewParameter()
        {
            base.GetViewParameter();
            if(ViewParam != null)
            {
                if (ViewParam is Dictionary<string, object>)
                {
                    Dictionary<string, object> parameters = (Dictionary<string, object>)ViewParam;
                    string parameterName = String.Empty;
                    object tempObj;
                    if (IsUserHost)
                    {
                        _brokerHost.NewOnlineUserEvent += OnNewOnlineUserEvent;
                        _brokerHost.LobbyInfoRequestedEvent += OnLobbyInfoRequestedEvent;
                        _brokerHost.RunServer();
                                
                    }
                    else if (IsUserClient)
                    {
                        parameterName = "LobbyStatus";
                        if(parameters.TryGetValue(parameterName, out tempObj))
                        {
                            if(tempObj is LobbyStatus lobbyStatus)
                            {
                                SetLobbyStatusForClient(lobbyStatus);
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
        public override void OnMessageReceivedEvent(object sender, MessageReceivedEventArgs e)
        {
            MessageBase message = (MessageBase)e.MessageReceived;
            switch (message.MessageCode)
            {
                case (int)CommunicationCnst.Messages.SendUpdatedUserList:
                    HandleUpdateUserListMessage(message as SendUpdatedUserList);
                    break;
                case (int)CommunicationCnst.Messages.LobbyChatMessage:
                    HandleNewChatMessage(message as LobbyChatMessage);
                    break;
                case (int)CommunicationCnst.Messages.LobbyStatusAndSettings:
                    HandleLobbyStatusMessage(message as LobbyStatusAndSettings);
                    break;
            }
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
            _brokerHost.SendLobbyInfo(e.client, Users, GetLobbyStatus());
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

        private void SendLobbyStatusToClients()
        {
            LobbyStatusAndSettings message = new LobbyStatusAndSettings(GetLobbyStatus());
            _brokerHost.SendToClients(message);
        }
        private LobbyStatus GetLobbyStatus()
        {
            return new LobbyStatus(
                Convert.ToInt16(this.IsLobbyChatEnabled),
                SelectedGame == null ? (short)0 : SelectedGame.GameId,
                this.GameSettings
                );
        }
        #endregion

        #region Client Methods
        private void SetLobbyStatusForClient(LobbyStatus lobbyStatus)
        {
            IsLobbyChatEnabled = lobbyStatus.bChatStatus;
            SelectedGame = Games.Where(g => g.GameId == lobbyStatus.GameId).FirstOrDefault();
            if(SelectedGame != null )
            {
                SelectedGame.GameSettings = lobbyStatus.GameSettings;
            }
            NotifyPropertyChanged(nameof(GameSettings));
            NotifyPropertyChanged(nameof(PlayersTime));
        }
        #endregion
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
        private void HandleLobbyStatusMessage(LobbyStatusAndSettings message)
        {
            if (IsUserClient)
            {
                SetLobbyStatusForClient(message.lobbyStatus);
            }
        }
        private void OnSelectedGameChanged()
        {
            if(IsUserHost)
            {
                SendLobbyStatusToClients();
            }
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

        private void RemovePlayerTimeExecute(object param)
        {
            if (PlayersTime > 5)
                PlayersTime -= 5;
            else PlayersTime = 1;
        }
        private bool RemovePlayerTimeCanExecute(object param) => SelectedGame != null && PlayersTime > 1;
        private void AddPlayerTimeExecute(object param)
        {
            if (PlayersTime >= 5)
                PlayersTime += 5;
            else PlayersTime = 5;
            AddPlayerTime.RaiseCanExecuteChanged();
        }
        private bool AddPlayerTimeCanExecute(object param) => SelectedGame != null;
        #endregion
    }
}
