using Core.Helpers;
using LIB.Constants;
using LIB.Entities;
using LIB.Sounds;
using LIB_Com.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LIB_Com.Constants.CommunicationCnst;

namespace LIB_Com.ViewModels
{
    public abstract class OnlineGameViewModelBase<C, S> : OnlineViewModelBase
        where C : CellEntityBase
        where S : OnlineSettingsBase
    {
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
            InitGame();
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
        #endregion

        #region Private Methods
        #endregion

        #region Protected Methods
        protected void PlaySound(string sound)
        {
            SoundsManagment.PlaySoundSingle(sound);
        }
        protected abstract ObservableCollection<C> GenerateGrid();
        protected virtual void InitGame()
        {
            Cells = GenerateGrid();
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
