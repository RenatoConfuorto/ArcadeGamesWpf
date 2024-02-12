using Core.Commands;
using Core.Helpers;
using LIB.Constants;
using LIB.Helpers;
using LIB.ViewModels;
using LIB_Com.Constants;
using LIB_Com.Entities;
using LIB_Com.Events;
using LIB_Com.MessageBrokers;
using LIB_Com.Messages.Base;
using LIB_Com.Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LIB_Com.Constants.CommunicationCnst;

namespace LIB_Com.ViewModels
{
    public class OnlineViewModelBase : ContentViewModel
    {
        #region Private Fields
        private BindingList<OnlineUser> _users = new BindingList<OnlineUser>();
        private string _hostIp;
        #endregion

        #region Protected fields
        protected CommunicationCnst.Mode _userMode;
        protected BrokerHost _brokerHost;
        protected BrokerClient _brokerClient;
        #endregion

        #region Command
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
        #endregion

        #region Constructor
        public OnlineViewModelBase(string viewName, string parentView = null, object param = null) 
            : base(viewName, parentView, param) 
        { 
        }
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
        protected override void SetCommandExecutionStatus()
        {
            base.SetCommandExecutionStatus();
        }
        protected override void GetViewParameter()
        {
            if (ViewParam != null)
            {
                if (ViewParam is Dictionary<string, object>)
                {
                    Dictionary<string, object> parameters = (Dictionary<string, object>)ViewParam;
                    string parameterName = USER_MODE;
                    object tempObj;
                    if (parameters.TryGetValue(parameterName, out tempObj))
                    {
                        if (tempObj is CommunicationCnst.Mode)
                        {
                            _userMode = (CommunicationCnst.Mode)tempObj;
                            NotifyPropertyChanged(nameof(IsUserHost));
                            NotifyPropertyChanged(nameof(IsUserClient));
                        }
                    }
                    parameterName = USERS_LIST;
                    if (parameters.TryGetValue(parameterName, out tempObj))
                    {
                        if (tempObj is IEnumerable<OnlineUser> users)
                        {
                            Users = new BindingList<OnlineUser>(users.ToList());
                        }
                    }
                    parameterName = HOST_IP;
                    if (parameters.TryGetValue(parameterName, out tempObj))
                    {
                        if (tempObj is string hostIp)
                        {
                            HostIp = hostIp;
                        }
                    }
                    parameterName = USER_BROKER;
                    if (parameters.TryGetValue(parameterName, out tempObj))
                    {
                        if (IsUserHost)
                        {
                            if (tempObj is BrokerHost)
                            {
                                _brokerHost = (BrokerHost)tempObj;
                                _brokerHost.MessageReceivedEvent += OnMessageReceivedEvent;
                            }
                        }
                        else if (IsUserClient)
                        {
                            if (tempObj is BrokerClient)
                            {
                                _brokerClient = (BrokerClient)tempObj;
                                _brokerClient.MessageReceivedEvent += OnMessageReceivedEvent;
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
        protected Dictionary<string, object> GenerateBaseViewParameters()
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { USER_MODE, _userMode },
                { HOST_IP, HostIp },
                { USERS_LIST, Users },
            };
            if (IsUserHost)
                parameters.Add(USER_BROKER, _brokerHost);
            else
                parameters.Add(USER_BROKER, _brokerClient);

            return parameters;
        }
        public override void Dispose()
        {
            _brokerHost?.Dispose();
            _brokerClient?.Dispose();
            base.Dispose();
        }
        #endregion

        #region Private Methods

        #region Host Methods

        #endregion

        #region Client Methods

        #endregion
        protected virtual void OnMessageReceivedEvent(object sender, MessageReceivedEventArgs e)
        {
        }

        #endregion

        #region Commands methods
        #endregion
    }
}
