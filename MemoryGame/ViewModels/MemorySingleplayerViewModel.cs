using Core.Attributes;
using Core.Commands;
using LIB.Attributes;
using LIB.Constants;
using LIB.Entities;
using LIB.Entities.Data.Memory;
using LIB.Sounds;
using LIB.ViewModels;
using MemoryGame.Common;
using MemoryGame.Common.Entities;
using MemoryGame.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static LIB.Entities.Data.Base.GameEnums;
using static MemoryGame.Common.Constants;

namespace MemoryGame.ViewModels
{
    [ViewRef(typeof(MemorySingleplayer))]
    [SettingsPopup(ViewNames.MemorySingleplayerSettings)]
    public class MemorySingleplayerViewModel : MemoryGameViewModelBase<MemorySingleplayerSettings, GameDataMemorySp>
    {
        #region Private Fields
        private int _errors;
        private int _maxErrors;
        private int _points;

        #endregion

        #region Public Properties
        public int Errors
        {
            get => _errors;
            set
            {
                SetProperty(ref _errors, value);
                if(MainUser != null)
                {
                    _gameDataMainUser.ErrorsNumber++;
                }
            }
        }
        public int MaxErrors
        {
            get => _maxErrors;
            set => SetProperty(ref _maxErrors, value);
        }
        public int Points
        {
            get => _points;
            set
            {
                SetProperty(ref _points, value);
                if (MainUser != null)
                {
                    _gameDataMainUser.Points++;
                }
            }
        }
        #endregion

        #region Constructor
        public MemorySingleplayerViewModel()
            : base(ViewNames.MemorySingleplayer) 
        {
        }
        #endregion

        #region Override Methods
        protected override void OnInitialized()
        {
            InitSettings();
            base.OnInitialized();
        }
        protected override void InitCommands()
        {
            base.InitCommands();
        }
        protected override void InitGame()
        {
            base.InitGame();
            Errors = 0;
            Points = 0;
        }
        //settings
        protected override GameSettingsBase PrepareDataForPopup()
        {
            return _settings;
        }
        protected override void OnPopupClosed()
        {
            base.OnPopupClosed();
        }
        protected override void OnSettingsReceived(object settings)
        {
            base.OnSettingsReceived(settings);
            if(settings is MemorySingleplayerSettings)
            {
                MaxErrors = _settings.ErrorsLimit;
                InitUIDimensions();
            }
        }
        protected override void MenageGameUsers()
        {
            base.MenageGameUsers();
            //if(MainUser != null)
            //{
            //    _gameDataMainUser = InitUserGameData(_settings);
            //    MainUser.Proxy.SaveData(_gameDataMainUser);
            //}
        }

        protected override void SaveGameResults()
        {
            if(MainUser != null)
            {
                MainUser.Proxy.UpdateData(_gameDataMainUser);
            }
        }
        protected override GameDataMemorySp InitUserGameData(MemorySingleplayerSettings settings, string userName, int? gameId = null)
        {
            GameDataMemorySp result = new GameDataMemorySp(userName,
                                            DateTime.Now,
                                            settings.CardsNumber,
                                            settings.GameDifficulty,
                                            settings.ErrorsLimit,
                                            0,
                                            0,
                                            MemoryResult.defeat);
            if (gameId != null) result.GameId = (int)gameId;
            return result;
        }
        protected override void InitSettings()
        {
            MemorySpDifficulty diff = _settings == null ? MemorySpDifficulty.Easy : _settings.GameDifficulty;
            if (diff == MemorySpDifficulty.Custom)
            {
                //nel caso di impostazioni custom non c'è nulla da inizializzare
            }
            else
            {
                switch (diff)
                {
                    case MemorySpDifficulty.Easy:
                        _settings = new MemorySingleplayerSettings()
                        {
                            GameDifficulty = diff,
                            CardsNumber = CARDS_NO_EASY,
                            ErrorsLimit = ERRORS_LIMIT_EASY
                        };
                        break;
                    case MemorySpDifficulty.Medium:
                        _settings = new MemorySingleplayerSettings()
                        {
                            GameDifficulty = diff,
                            CardsNumber = CARDS_NO_MEDIUM,
                            ErrorsLimit = ERRORS_LIMIT_MEDIUM
                        };
                        break;
                    case MemorySpDifficulty.Hard:
                        _settings = new MemorySingleplayerSettings()
                        {
                            GameDifficulty = diff,
                            CardsNumber = CARDS_NO_HARD,
                            ErrorsLimit = ERRORS_LIMIT_HARD
                        };
                        break;
                }
            }
            MaxErrors = _settings.ErrorsLimit;
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
                    Errors++;
                    if (Errors == _settings.ErrorsLimit)
                    {
                        GameOverMessage = "Sconfitta";
                        EndGame();
                    }
                    break;
                case 0:
                    //prosegire
                    break;
                case 1:
                    Points++;
                    PlaySound(SoundsManagment.MemorySoundPointScored);
                    if (CheckVictory())
                    {
                        GameOverMessage = "Vittoria";
                        EndGame();
                    }
                    break;
            }
        }
        #endregion

        #region Private Methods

        private void InitUIDimensions()
        {
            switch (_settings.GameDifficulty)
            {
                case MemorySpDifficulty.Easy:
                    setDim(125.0d, 4, 3);
                    break;
                case MemorySpDifficulty.Medium:
                    setDim(110.0d, 6, 4);
                    break;
                case MemorySpDifficulty.Hard:
                    setDim(90.0d, 8, 6);
                    break;
                case MemorySpDifficulty.Custom:
                    SetCustomDimensions();
                    break;
            }
            BoardWidth = (CellDim + 8) * cardsPerRow;     //Margin = 4 2, 6 cells each row
            BoardHeight = (CellDim + 4) * cardsPerColumn; //Margin = 4 2, 4 cells each column

            NotifyPropertyChanged(nameof(CellDim));
            NotifyPropertyChanged(nameof(BoardWidth));
            NotifyPropertyChanged(nameof(BoardHeight));
        }
        
        private bool CheckVictory()
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
