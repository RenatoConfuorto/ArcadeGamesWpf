﻿using LIB.Constants;
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

namespace MemoryGame.ViewModels
{
    [ViewRef(typeof(MemoryMultiplayer))]
    [SettingsPopup(ViewNames.MemoryMultiplayerSettings)]
    public class MemoryMultiplayerViewModel : MemoryGameViewModelBase<MemoryMultiplayerSettings, GameDataMemoryMp>
    {
        #region Game Constants
        public const int CARDS_NUMBER_DEFAULT = 12;
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
            InitSettings(); //TODO questo resetta le impostazioni se si riavvia
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
            base.OnSettingsReceived(settings);
            if (settings is MemoryMultiplayerSettings)
            {
                InitUIDimensions();
            }
        }
        protected override void MenageGameUsers()
        {
            base.MenageGameUsers();
            if (MainUser != null)
            {
                //_gameDataMainUser = InitUserGameData(_settings, MainUserName);
                //MainUser.Proxy.SaveData(_gameDataMainUser);
                Users.Add(new MemoryMultiplayerUser(MainUser));
            }
            else
            {
                Users.Add(new MemoryMultiplayerUser("Player 1"));
            }
            CurrentUser = Users[0];

            User secondaryUser = UserManager.GetSecondLoggedUser();
            if(secondaryUser != null)
            {
                Users.Add(new MemoryMultiplayerUser(secondaryUser));
            }
            else
            {
                Users.Add(new MemoryMultiplayerUser("Player 2"));
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
            GameDataMemoryMp result = new GameDataMemoryMp(userName,
                DateTime.Now,
                settings.CardsNumber,
                0,
                0,
                MemoryResult.defeat);
            if (gameId != null) result.GameId = (int)gameId;
            return result;
        }
        protected override void InitSettings()
        {
            _settings = new MemoryMultiplayerSettings()
            {
                CardsNumber = CARDS_NUMBER_DEFAULT,
                ErrorsLimit = ERRORS_LIMIT_DEFAULT,
                IsErrorLimitEnabled = ERRORS_LIMIT_ENABLED_DEFAULT
            };

            InitUIDimensions();
        }
        protected override void OnCellClicked(int cellId)
        {
            CardEntity cell = Cells.Where(c => c.CellId == cellId).FirstOrDefault();
            if (cell.CardTurned && _cellClicked.Count() >= 2) return;
            cell.CardTurned = true;
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
                                gameOverMessage += $"{user.Name}, ";
                            }
                            gameOverMessage = gameOverMessage.Remove(gameOverMessage.Length - 2);
                            gameOverMessage += ")";
                        }
                        else
                        {
                            gameOverMessage = $"{winner.Name} ha vinto";
                        }
                        CloseGame(gameOverMessage);
                    }
                    break;
            }
        }
        #endregion

        #region Private Methods
        private void CloseGame(string gameOverMessage)
        {
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
                _gameDataMainUser.GameResult = MemoryResult.victory;
            }
            return victory;
        }
        #endregion
    }
}
