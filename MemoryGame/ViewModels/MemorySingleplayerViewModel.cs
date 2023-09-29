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
                MaxErrors = _settings.ErrorsLimit;
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
                //nel caso di impostazioni custom non c'è nulla da inizializzare
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
            int cardsPerRow = 0;
            int cardsPerColumn = 0;
            void setDim(double _cardDim, int _cardsPerRow, int _cardsPerColumn)
            {
                CellDim = _cardDim;
                cardsPerRow = _cardsPerRow;
                cardsPerColumn = _cardsPerColumn;
            }
            switch (_settings.GameDifficulty)
            {
                case Difficulty.Easy:
                    setDim(125.0d, 4, 3);
                    //CellDim = 125.0d;
                    //BoardWidth = (CellDim + 8) * 4; //Margin = 4 2, 4 cells each row
                    //BoardHeight = (CellDim + 4) * 3;//Margin = 4 2, 3 cells each column
                    break;
                case Difficulty.Medium:
                    setDim(110.0d, 6, 4);
                    //CellDim = 110.0d;
                    //BoardWidth = (CellDim + 8) * 6; //Margin = 4 2, 6 cells each row
                    //BoardHeight = (CellDim + 4) * 4;//Margin = 4 2, 4 cells each column
                    break;
                case Difficulty.Hard:
                    setDim(90.0d, 8, 6);
                    //CellDim = 90.0d;
                    //BoardWidth = (CellDim + 8) * 8; //Margin = 4 2, 6 cells each row
                    //BoardHeight = (CellDim + 4) * 6;//Margin = 4 2, 4 cells each column
                    break;
                case Difficulty.Custom:
                    switch (_settings.CardsNumber / 6)
                    {
                        case 2:
                            setDim(125.0d, 4, 3);
                            break;
                        case 4:
                            setDim(110.0d, 6, 4);
                            break;
                        case 6:
                            setDim(100.0d, 9, 4);
                            break;
                        case 8:
                            setDim(90.0d, 8, 6);
                            break;
                        case 10:
                            setDim(80.0d, 10, 6);
                            break;
                        case 12:
                            setDim(80.0d, 9, 8);
                            break;
                        case 14:
                            setDim(60.0d, 12, 7);
                            break;
                        case 16:
                            setDim(60.0d, 12, 8);
                            break;
                        case 18:
                            setDim(60.0d, 12, 9);
                            break;
                        case 20:
                            setDim(60.0d, 12, 10);
                            break;
                    }
                    break;
            }
            BoardWidth = (CellDim + 8) * cardsPerRow;     //Margin = 4 2, 6 cells each row
            BoardHeight = (CellDim + 4) * cardsPerColumn; //Margin = 4 2, 4 cells each column

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
