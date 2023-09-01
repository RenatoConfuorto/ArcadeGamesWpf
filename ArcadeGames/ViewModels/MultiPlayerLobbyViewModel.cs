using ArcadeGames.Views;
using Core.Attributes;
using Core.Commands;
using Core.Helpers;
using LIB.Communication.Constants;
using LIB.Communication.MessageBrokers;
using LIB.Constants;
using LIB.Helpers;
using LIB.ViewModels;
using System;
using System.Collections.Generic;
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
        #endregion

        #region Command
        #endregion

        #region Public Properties
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
                            if (tempObj is BrokerHost)
                            {
                                _brokerHost = (BrokerHost)tempObj;
                                _brokerHost.MessageReceived += OnMessageReceived;
                            }
                        }
                        else if (_userMode == CommunicationCnst.Mode.Client)
                        {
                            if (tempObj is BrokerClient)
                            {
                                _brokerClient = (BrokerClient)tempObj;
                                _brokerClient.MessageReceived += OnMessageReceived;
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
        private void OnMessageReceived(object message)
        {

        }
        #endregion
    }
}
