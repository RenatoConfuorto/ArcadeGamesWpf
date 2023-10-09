using LIB.Constants;
using LIB.Entities.Data.Memory;
using LIB.Entities;
using MemoryGame.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using static LIB.Entities.Data.Base.GameEnums;
using Core.Attributes;
using MemoryGame.Views;
using System.Collections.ObjectModel;
using LIB.ValueConverters;
using LIB.Helpers;
using LIB.UserMng;
using LIB.Attributes;
using LIB.Sounds;

namespace MemoryGame.ViewModels
{
    [ViewRef(typeof(MemoryMultiplayer))]
    [SettingsPopup(ViewNames.MemoryMultiplayerSettings)]
    public class MemoryMultiplayerViewModel : MemoryGameViewModelBase<MemoryMultiplayerSettings, GameDataMemoryMp>
    {
        #region Game Constants
        public const int CARDS_NUMBER_DEFAULT = 60;
        public const int ERRORS_LIMIT_DEFAULT = 20;
        public const bool ERRORS_LIMIT_ENABLED_DEFAULT = true;
        #endregion
        #region Private Fields
        private MemoryMultiplayerUser _currentUser;
        #endregion

        #region Public Properties
        public ObservableCollection<MemoryMultiplayerUser> Users
        {
            get => _settings.Users;
        }
        public int ErrorsLimit
        {
            get => _settings.ErrorsLimit;
        }
        public MemoryMultiplayerUser CurrentUser
        {
            get => _currentUser;
            set
            {
                if(CurrentUser != null) CurrentUser.IsActive = false;
                SetProperty(ref _currentUser, value);
                CurrentUser.IsActive = true;
            }
        }
        #endregion

            #region Constructor
        public MemoryMultiplayerViewModel()
            : base(ViewNames.MemoryMultiplayer)
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
        protected override void InitGame()
        {
            base.InitGame();
            if (Users != null && Users.Count > 0)
            {
                foreach(var user in Users) user.ResetData();
                CurrentUser = Users[0];
                NotifyPropertyChanged(nameof(Users));
            }
        }
        //settings
        protected override void OnPopupClosed()
        {
            base.OnPopupClosed();
        }
        protected override void OnSettingsReceived(object settings)
        {
            //base.OnSettingsReceived(settings);
            if (settings is MemoryMultiplayerSettings gameSettings)
            {
                _settings = gameSettings;
                InitUIDimensions();
                //aggiornare user data
                foreach(MemoryMultiplayerUser user in Users)
                {
                    //inizializzare il proxy dato che viene perso durante il Clone
                    user.InitUserProxy();
                    user.InitGameDataFromSettings(gameSettings);
                    //user.UpdateGameData();
                }
            }
        }
        protected override void MenageGameUsers()
        {
            //base.MenageGameUsers();
            MainUser = UserManager.GetMainLoggedInUser();
            if (MainUser != null)
            {
                //_gameDataMainUser = InitUserGameData(_settings, MainUserName);
                //MainUser.Proxy.SaveData(_gameDataMainUser);
                if(IsUserNotPresentInList(MainUserName) && Users.Count == 0) Users.Add(new MemoryMultiplayerUser(MainUser));
                // && Users.Count == 0 => controllare il numero di utenti in modo da non riaggiungerli se si riavvia
            }
            else
            {
                if(IsUserNotPresentInList("Player 1") && Users.Count == 0) Users.Add(new MemoryMultiplayerUser("Player 1"));
            }
            CurrentUser = Users[0];

            User secondaryUser = UserManager.GetSecondLoggedUser();
            if(secondaryUser != null)
            {
                if(IsUserNotPresentInList(secondaryUser.Name) && Users.Count == 1) Users.Add(new MemoryMultiplayerUser(secondaryUser));
            }
            else
            {
                if(IsUserNotPresentInList("Player 2") && Users.Count == 1) Users.Add(new MemoryMultiplayerUser("Player 2"));
            }
        }

        protected override void SaveGameResults()
        {
            if (MainUser != null)
            {
                //MainUser.Proxy.UpdateData(_gameDataMainUser);
            }
        }
        protected override GameDataMemoryMp InitUserGameData(MemoryMultiplayerSettings settings, string userName, int? gameId = null)
        {
            //GameDataMemoryMp result = new GameDataMemoryMp(userName,
            //    DateTime.Now,
            //    settings.CardsNumber,
            //    0,
            //    settings.ErrorsLimit,
            //    0,
            //    MemoryResult.defeat,
            //    settings.Users.Count());
            //if (gameId != null) result.GameId = (int)gameId;
            //return result;
            return null; //Per Memory Mp la gestione dei dati è gestita in modo differente
        }
        protected override void InitSettings()
        {
            if (_settings == null)
            {
                _settings = new MemoryMultiplayerSettings()
                {
                    CardsNumber = CARDS_NUMBER_DEFAULT,
                    ErrorsLimit = ERRORS_LIMIT_DEFAULT,
                    IsErrorLimitEnabled = ERRORS_LIMIT_ENABLED_DEFAULT
                };
                foreach(MemoryMultiplayerUser user in Users)
                {
                    user.InitGameDataFromSettings(_settings);
                }
            }

            InitUIDimensions();
        }
        protected override void OnCellClicked(int cellId)
        {
            CardEntity cell = Cells.Where(c => c.CellId == cellId).FirstOrDefault();
            if (cell.CardTurned && _cellClicked.Count() >= 2) return;
            cell.CardTurned = true;
            PlayCardFlip();
            _cellClicked.Add(cell);

            switch (CheckCardTurned())
            {
                case -1:
                    CurrentUser.Errors++;
                    //TODO Controllare se l'utente ha perso
                    if (_settings.IsErrorLimitEnabled)
                    {
                        if(CurrentUser.Errors >= ErrorsLimit)
                        {
                            CurrentUser.CanPlay = false;
                            //controllare se è rimasto un solo utente
                            if(Users.Where(u => u.CanPlay).ToList().Count() == 1)
                            {
                                string Username = Users.Where(u => u.CanPlay).FirstOrDefault().Name;
                                CloseGame($"{Username} ha vinto");
                            }
                        }
                    }
                    SwitchUser();
                    break;
                case 0:
                    //prosegire
                    break;
                case 1:
                    CurrentUser.Points++;
                    PlaySound(SoundsManagment.MemorySoundPointScored);
                    if (CheckUserVictory())
                    {
                        string gameOverMessage = String.Empty;
                        MemoryMultiplayerUser winner = Users.OrderByDescending(u => u.Points).FirstOrDefault();
                        //controllare se c'è un pareggio
                        List<MemoryMultiplayerUser> tieUsers = Users.Where(u => u.Points == winner.Points).ToList();
                        if (tieUsers.Count() > 1)
                        {
                            gameOverMessage = "Pareggio (";
                            foreach(MemoryMultiplayerUser user in tieUsers)
                            {
                                user._gameData.GameResult = MemoryResult.tie;
                                gameOverMessage += $"{user.Name}, ";
                            }
                            gameOverMessage = gameOverMessage.Remove(gameOverMessage.Length - 2);
                            gameOverMessage += ")";
                        }
                        else
                        {
                            gameOverMessage = $"{winner.Name} ha vinto";
                            winner._gameData.GameResult = MemoryResult.victory;
                        }
                        CloseGame(gameOverMessage);
                    }
                    break;
            }
        }
        #endregion

        #region Private Methods
        private bool IsUserNotPresentInList(string userName)
        {
            return Users.Where(u => u.Name == userName).Count() == 0;
        }
        private void CloseGame(string gameOverMessage)
        {
            foreach(MemoryMultiplayerUser user in Users)
            {
                //user.UpdateGameData();
                user.SaveGameData();
            }
            GameOverMessage = gameOverMessage;
            EndGame();
        }
        private void SwitchUser()
        {
            int currentUserIdx = Users.IndexOf(CurrentUser);
            if(currentUserIdx >= Users.Count - 1) { currentUserIdx = -1; }
            CurrentUser = Users[currentUserIdx + 1];
            if (!CurrentUser.CanPlay) SwitchUser();
        }

        private void InitUIDimensions()
        {
            SetCustomDimensions();
            BoardWidth = (CellDim + 8) * cardsPerRow;     //Margin = 4 2, 6 cells each row
            BoardHeight = (CellDim + 4) * cardsPerColumn; //Margin = 4 2, 4 cells each column

            NotifyPropertyChanged(nameof(CellDim));
            NotifyPropertyChanged(nameof(BoardWidth));
            NotifyPropertyChanged(nameof(BoardHeight));
        }

        private bool CheckUserVictory()
        {
            bool victory = Cells.Where(c => c.CardTurned).Count() == _settings.CardsNumber;
            if (victory && MainUser != null)
            {
                //_gameDataMainUser.GameResult = MemoryResult.victory;
            }
            return victory;
        }
        #endregion
    }
}
