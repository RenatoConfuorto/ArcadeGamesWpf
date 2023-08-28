using ArcadeGames.Views;
using Core.Attributes;
using Core.Commands;
using Core.Helpers;
using LIB.Constants;
using LIB.Helpers;
using LIB.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace ArcadeGames.ViewModels
{
    [ViewRef(typeof(MultiPlayerFormView))]
    public class MultiPlayerFormViewModel : ContentViewModel
    {
        #region Private Fields
        private string _localIp;
        private string _remoteIp;
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
            CreateCommand = new RelayCommand(CreateCommandExecute);
            JoinCommand = new RelayCommand(JoinCommandExeucte, JoinCommandCanExecute);
            NotifyPropertyChanged(nameof(JoinCommand));
        }
        protected override void SetCommandExecutionStatus()
        {
            base.SetCommandExecutionStatus();
            JoinCommand.RaiseCanExecuteChanged();
        }
        #endregion

        #region Private Methods
        private void CreateCommandExecute(object param)
        {
        }
        private void JoinCommandExeucte(object param)
        {
            try
            {
                Ping ping = new Ping();
                PingReply pr = ping.Send(RemoteIp);
                if(pr.Status == IPStatus.Success)
                {
                    //lobby
                }
                else
                {
                    MessageDialogHelper.ShowInfoMessage("Impossibile trovare l'host specificato");
                }
            }catch(Exception ex)
            {
                MessageDialogHelper.ShowInfoMessage("Impossibile trovare l'host specificato\r\n" + ex.Message);
            }
        }
        private bool JoinCommandCanExecute(object param) => !String.IsNullOrEmpty(RemoteIp);
        #endregion
    }
}
