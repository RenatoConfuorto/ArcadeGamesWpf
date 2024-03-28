using Core.Helpers;
using LIB.Constants;
using LIB.Entities;
using LIB.Sounds;
using LIB.Views;
using LIB_Com.Constants;
using LIB_Com.Entities;
using LIB_Com.Events;
using LIB_Com.Messages;
using LIB_Com.Messages.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static LIB_Com.Constants.CommunicationCnst;

namespace LIB_Com.ViewModels
{
    public abstract class OnlineGameViewModelBase<C, S> : OnlineViewModelBase
        where C : CellEntityBase
        where S : OnlineSettingsBase
    {
        private MessageBox _gameStatusMessage;
        private List<Guid> _confirmationReceived = new List<Guid>();
        #region Private Fields
        private bool _isGameEnabled = true;
        private bool _isGameOver = false;
        private string _gameOverMessage = String.Empty;

        private ObservableCollection<C> _cells;
        private S _gameSettings;
        #endregion

        #region Commands
        #endregion

        #region Public Properties
        public bool IsGameEnabled
        {
            get => _isGameEnabled;
            set => SetProperty(ref _isGameEnabled, value);
        }
        public bool IsGameOver
        {
            get => _isGameOver;
            set => SetProperty(ref _isGameOver, value);
        }
        public string GameOverMessage
        {
            get => _gameOverMessage;
            set => SetProperty(ref _gameOverMessage, value);
        }
        public ObservableCollection<C> Cells
        {
            get => _cells;
            set => SetProperty(ref _cells, value);
        }
        public S GameSettings
        {
            get => _gameSettings;
            set => SetProperty(ref _gameSettings, value);
        }
        #endregion

        #region Constructor
        public OnlineGameViewModelBase(string viewName, string parentView = null, object param = null)
            : base(viewName, parentView, param)
        {
        }
        #endregion

        #region Override Methods
        protected override void SetCommandExecutionStatus()
        {
            base.SetCommandExecutionStatus();
        }
        protected override void InitCommands()
        {
            base.InitCommands();
        }
        protected override void OnInitialized()
        {
            base.OnInitialized();
            Cells = GenerateGrid();
            //InitGame();
            WaitClientsConfirmationAndGameStart();
            if(IsUserClient)
                SendGameJoinConfirmation();
        }
        protected override void GetViewParameter()
        {
            base.GetViewParameter();
            Dictionary<string, object> parameters = (Dictionary<string, object>)ViewParam;
            if(parameters.TryGetValue(GAME_SETTINGS, out object tempObj))
            {
                if(tempObj is S settings)
                {
                    GameSettings = settings;
                    logger.LogDebug($"Ricevute impostazioni di tipo <{GameSettings.GetType().Name}>, PlayersTime <{GameSettings.PlayersTime}>");
                }
            }
            if(GameSettings == null)
            {
                MessageDialogHelper.ShowInfoMessage("Errore nella ricezione delle impostazioni.");
                ChangeView(ViewNames.Home);
            }
        }
        public override void Dispose()
        {
            base.Dispose();
        }
        public override void OnMessageReceivedEvent(object sender, MessageReceivedEventArgs e)
        {
            base.OnMessageReceivedEvent(sender, e);
            MessageBase message = e.MessageReceived as MessageBase;
            switch((CommunicationCnst.Messages)message.MessageCode)
            {
                case CommunicationCnst.Messages.GameJoinConfirmationMessage:
                    HandleGameJoinConfirmationMessage(message as GameJoinConfirmationMessage);
                    break;
                case CommunicationCnst.Messages.GameInitCommandMessage:
                    HandleGameInitCommandMessage(message as GameInitCommandMessage);
                    break;
            }
        }

        #endregion

        #region Private Methods
        private void WaitClientsConfirmationAndGameStart()
        {
            string statusMessage;
            if (IsUserClient)
                statusMessage = "In attesa della risposta dell'host";
            else statusMessage = "In attesa dei client";

            _gameStatusMessage = MessageDialogHelper.ShowStatusMessage(statusMessage);
            IsGameEnabled = false;
        }

        #region Host Methods
        private void HandleGameJoinConfirmationMessage(GameJoinConfirmationMessage message)
        {
            if (IsUserHost)
            {
                logger.LogDebug($"Ricevuto GameJoinConfirmationMessage da UserId <{message.SenderId}>");
                _confirmationReceived.Add(message.SenderId);
                if(CheckClientsConfirmations())
                {
                    logger.LogDebug("Tutti i client sono entrati nella partita.");
                    GameInitCommandMessage gameInitMessage = new GameInitCommandMessage();
                    _brokerHost.SendToClients(gameInitMessage);
                    Thread.Sleep(250); //If the operation is too fast _gameStatusMessage will be null
                    MessageDialogHelper.CloseMessageBox(_gameStatusMessage);
                    InitGame();
                }
            }
        }
        /// <summary>
        /// Check if has been received confirmation from all client
        /// </summary>
        /// <returns></returns>
        private bool CheckClientsConfirmations()
        {
            bool bRetVal = true;
            foreach(OnlineUser user in Users)
            {
                if (user.UserId != _brokerHost.LocalUserId && !_confirmationReceived.Contains(user.UserId))
                {
                    bRetVal = false;
                    break;
                }
            }
            return bRetVal;
        }
        #endregion

        #region Client Methods
        private void HandleGameInitCommandMessage(GameInitCommandMessage message)
        {
            if (IsUserClient)
            {
                logger.LogDebug("Ricevuto GameInitCommandMessage dall'host.");
                MessageDialogHelper.CloseMessageBox(_gameStatusMessage);
                InitGame();
            }
        }

        private void SendGameJoinConfirmation()
        {
            GameJoinConfirmationMessage message = new GameJoinConfirmationMessage(ViewName);
            _brokerClient.SendToHost(message);
            logger.LogDebug($"Inviata conferma di ingresso alla partita, ViewName <{message.GameViewName}>");
        }
        #endregion

        #endregion

        #region Protected Methods
        protected void PlaySound(string sound)
        {
            SoundsManagment.PlaySoundSingle(sound);
        }
        protected abstract ObservableCollection<C> GenerateGrid();
        protected virtual void InitGame()
        {
            IsGameEnabled = true;
            IsGameOver = false;
        }
        protected virtual void EndGame()
        {
            IsGameEnabled = false;
            IsGameOver = true;
        }
        #endregion
    }
}
