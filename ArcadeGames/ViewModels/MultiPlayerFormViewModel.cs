using ArcadeGames.Views;
using Core.Attributes;
using Core.Commands;
using Core.Helpers;
using LIB_Com.Constants;
using LIB_Com.Events;
using LIB_Com.MessageBrokers;
using LIB_Com.Messages;
using LIB_Com.Messages.Base;
using LIB.Constants;
using LIB.Entities;
using LIB.Helpers;
using LIB.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ArcadeGames.ViewModels
{
    [ViewRef(typeof(MultiPlayerFormView))]
    public class MultiPlayerFormViewModel : ContentViewModel
    {
        #region Private Fields
        private string _localIp;
        private string _remoteIp;
        private string _userName;
        #endregion

        #region Command
        public RelayCommand CreateCommand { get; set; }
        public RelayCommand JoinCommand { get; set; }
        #endregion

        #region Public Properties
        public string LocalIp
        {
            get => _localIp;
            set => SetProperty(ref _localIp, value);
        }
        public string RemoteIp
        {
            get => _remoteIp;
            set
            {
                SetProperty(ref _remoteIp, value);
                SetCommandExecutionStatus();
            }
        }
        public string UserName
        {
            get => _userName;
            set
            {
                SetProperty(ref _userName, value);
                SetCommandExecutionStatus();
            }
        }
        #endregion

        #region Constructor
        public MultiPlayerFormViewModel() : base(ViewNames.MultiPlayerForm, ViewNames.Home) { }
        #endregion

        #region Override Methods
        protected override void OnInitialized()
        {
            base.OnInitialized();
            LocalIp = CommunicationHelper.GetLocalIpAddress().ToString();
        }
        protected override void InitCommands()
        {
            base.InitCommands();
            CreateCommand = new RelayCommand(CreateCommandExecute, CreateCommandExecuteCanExecute);
            JoinCommand = new RelayCommand(JoinCommandExeucte, JoinCommandCanExecute);
            NotifyPropertyChanged(nameof(JoinCommand));
        }
        protected override void SetCommandExecutionStatus()
        {
            base.SetCommandExecutionStatus();
            CreateCommand.RaiseCanExecuteChanged();
            JoinCommand.RaiseCanExecuteChanged();
        }
        #endregion

        #region Private Methods
        private bool IsValidUserName() => !String.IsNullOrEmpty(UserName);
        private void CreateCommandExecute(object param)
        {
            BrokerHost broker = new BrokerHost(_userName);
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "Mode", CommunicationCnst.Mode.Host },
                { "Broker", broker }
            };
            ChangeView(ViewNames.MultiPlayerLobby, parameters);
        }
        private bool CreateCommandExecuteCanExecute(object parma) => IsValidUserName();
        private void JoinCommandExeucte(object param)
        {
            try
            {
                Ping ping = new Ping();
                PingReply pr = ping.Send(RemoteIp);

                if (pr.Status == IPStatus.Success)
                {
                    BrokerClient client = new BrokerClient(_userName);
                    //TODO gestire il caso di connessione fallita
                    client.LobbyInfoReceivedEvent += OnLobbyInfoReceived;
                    client.RunClient(RemoteIp);
                    
                    //ChangeView(ViewNames.MultiPlayerLobby, parameters);
                }
                //else
                //{
                //    MessageDialogHelper.ShowInfoMessage("La connessione non è andata a buon fine");
                //    return;
                //}
                else
                {
                    MessageDialogHelper.ShowInfoMessage("Impossibile trovare l'host specificato");
                }
            }
            catch (Exception ex)
            {
                MessageDialogHelper.ShowInfoMessage("Impossibile trovare l'host specificato\r\n" + ex.Message);
            }
        }
        private bool JoinCommandCanExecute(object param) => !String.IsNullOrEmpty(RemoteIp) && IsValidUserName();

        private void OnLobbyInfoReceived(object sender, LobbyInfoReceivedEventArgs e)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "Mode", CommunicationCnst.Mode.Client },
                { "Broker", sender },
                { "HostIp", e.HostIp },
                { "Users", e.Users }
            };
            ChangeView(ViewNames.MultiPlayerLobby, parameters);
        }
        #endregion
    }
}
