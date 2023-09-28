using Core.Attributes;
using Core.Commands;
using LIB.Attributes;
using LIB.Constants;
using LIB.Entities;
using LIB.ViewModels;
using MemoryGame.Common;
using MemoryGame.Common.Entities;
using MemoryGame.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static MemoryGame.Common.Constants;

namespace MemoryGame.ViewModels
{
    [ViewRef(typeof(MemorySingleplayer))]
    [SettingsPopup(ViewNames.MemorySingleplayerSettings)]
    public class MemorySingleplayerViewModel : GameViewModelBase<CardEntity>
    {
        #region Private Fields
        private MemorySingleplayerSettings _settings;
        private List<CardEntity> _cellClicked = new List<CardEntity>(2);
        private int _errors;
        private int _maxErrors;

        //view dimensions
        public double BoardWidth { get; set; }
        public double BoardHeight { get; set; }
        public double CellDim { get; set; }
        #endregion

        #region Public Properties
        public int Errors
        {
            get => _errors;
            set => SetProperty(ref _errors, value);
        }
        public int MaxErrors
        {
            get => _maxErrors;
            set => SetProperty(ref _maxErrors, value);
        }
        #endregion

        #region Constructor
        public MemorySingleplayerViewModel()
            : base(ViewNames.MemorySingleplayer, ViewNames.MemoryGameHomePage) { }
        #endregion

        #region Override Methods
        protected override void OnInitialized()
        {
            InitSettings(Difficulty.Easy);
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
        }
        protected override ObservableCollection<CardEntity> GenerateGrid()
        {
            ObservableCollection<CardEntity> result = new ObservableCollection<CardEntity>();
            CardEntity cell;
            int noCardTypes = Enum.GetValues(typeof(CardTypes)).Length;
            int CardsPerType = _settings.CardsNumber / noCardTypes;
            int cardId = 0;
            foreach(CardTypes cardType in Enum.GetValues(typeof(CardTypes)))
            {
                for(int i = 0; i < CardsPerType; i++)
                {
                    cell = new CardEntity()
                    {
                        CellId = cardId,
                        CardTurned = false,
                        CardType = cardType,
                        CardEnabled = true,
                    };
                    cell.cellClicked += OnCellClicked;
                    result.Add(cell);
                    cardId++;
                }
            }
            //casual order of the list
            Random random = new Random();
            result = new ObservableCollection<CardEntity>(result.OrderBy(c => random.Next()).ToList());
            return result;
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
            if(settings is MemorySingleplayerSettings newSettings)
            {
                _settings = newSettings;
                InitUIDimensions();
            }
        }

        protected override void SaveGameResults()
        {
        }
        #endregion

        #region Private Methods
        private void InitSettings(Difficulty diff)
        {
            if(diff == Difficulty.Custom)
            {
                //TODO gestire il caso custom
            }
            else
            {
                switch (diff)
                {
                    case Difficulty.Easy:
                        _settings = new MemorySingleplayerSettings()
                        {
                            GameDifficulty = diff,
                            CardsNumber = CARDS_NO_EASY,
                            ErrorsLimit = ERRORS_LIMIT_EASY
                        };
                        break;
                    case Difficulty.Medium:
                        _settings = new MemorySingleplayerSettings()
                        {
                            GameDifficulty = diff,
                            CardsNumber = CARDS_NO_MEDIUM,
                            ErrorsLimit = ERRORS_LIMIT_MEDIUM
                        };
                        break;
                    case Difficulty.Hard:
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
        private void InitUIDimensions()
        {
            switch (_settings.GameDifficulty)
            {
                case Difficulty.Easy:
                    CellDim = 125.0d;
                    BoardWidth = (CellDim + 8) * 4; //Margin = 4 2, 4 cells each row
                    BoardHeight = (CellDim + 4) * 3;//Margin = 4 2, 3 cells each column
                    break;
                case Difficulty.Medium:
                    CellDim = 110.0d;
                    BoardWidth = (CellDim + 8) * 6; //Margin = 4 2, 6 cells each row
                    BoardHeight = (CellDim + 4) * 4;//Margin = 4 2, 4 cells each column
                    break;
                case Difficulty.Hard:
                    CellDim = 90.0d;
                    BoardWidth = (CellDim + 8) * 8; //Margin = 4 2, 6 cells each row
                    BoardHeight = (CellDim + 4) * 6;//Margin = 4 2, 4 cells each column
                    break;
                case Difficulty.Custom:
                    //TODO
                    break;
            }
            NotifyPropertyChanged(nameof(CellDim));
            NotifyPropertyChanged(nameof(BoardWidth));
            NotifyPropertyChanged(nameof(BoardHeight));
        }
        private void OnCellClicked(int cellId)
        {
            CardEntity cell = Cells.Where(c => c.CellId == cellId).FirstOrDefault();
            if (cell.CardTurned && _cellClicked.Count() >= 2) return;
            cell.CardTurned = true;
            _cellClicked.Add(cell);

            CheckCardTurned();
            if (CheckVictory())
            {
                GameOverMessage = "Vittoria";
                EndGame();
            }else if(Errors == _settings.ErrorsLimit)
            {
                GameOverMessage = "Sconfitta";
                EndGame();
            }
        }

        private void CheckCardTurned()
        {
            if(_cellClicked.Count() != 2) return;
            else
            {
                if (_cellClicked[0].CardType == _cellClicked[1].CardType)
                {
                    _cellClicked[0].CardEnabled = false;
                    _cellClicked[1].CardEnabled = false;
                    _cellClicked.Clear();
                }
                else
                {
                    Errors++;
                    //turn back cards async so UI updates
                    Task.Run(() =>
                    {
                        IsGameEnabled = false;
                        Thread.Sleep(WAIT_TIME_CARD_TURN_BACK);
                        _cellClicked[0].CardTurned = false;
                        _cellClicked[1].CardTurned = false;
                        _cellClicked.Clear();
                        IsGameEnabled = true;
                    });
                }
            }
        }
        private bool CheckVictory()
        {
            return Cells.Where(c => c.CardTurned).Count() == _settings.CardsNumber;
        }
        #endregion
    }
}
