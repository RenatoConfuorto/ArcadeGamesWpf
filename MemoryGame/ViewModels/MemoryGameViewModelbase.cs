using LIB.Constants;
using LIB.Entities.Data.Memory;
using LIB.Entities;
using LIB.ViewModels;
using MemoryGame.Common.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static LIB.Entities.Data.Base.GameEnums;
using static MemoryGame.Common.Constants;
using LIB.Sounds;

namespace MemoryGame.ViewModels
{
    public abstract class MemoryGameViewModelBase<S, G> : LocalGameViewModelBase<CardEntity>
        where S : MemorySettingsBase, new()
        where G : GameDataMemoryBase, new()
    {
        #region Private Fields
        protected G _gameDataMainUser;
        protected S _settings;

        protected List<CardEntity> _cellClicked = new List<CardEntity>(2);
        protected List<string> _cardsSound = new List<string>()
        {
            SoundsManagment.MemorySoundCard_1,
            SoundsManagment.MemorySoundCard_2,
            SoundsManagment.MemorySoundCard_3
        };


        //view dimensions
        public double BoardWidth { get; set; }
        public double BoardHeight { get; set; }
        public double CellDim { get; set; }
        protected int cardsPerRow = 0;
        protected int cardsPerColumn = 0;
        #endregion

        #region Public Properties
        #endregion

        #region Constructor
        public MemoryGameViewModelBase(string ViewName)
            : base(ViewName, ViewNames.MemoryGameHomePage)
        {
        }
        #endregion

        #region Override Methods
        protected override void OnInitialized()
        {
            InitSettings();
            PlaySound(SoundsManagment.MemorySoundCardsShuffle);
            base.OnInitialized();
        }
        protected override void InitCommands()
        {
            base.InitCommands();
        }
        protected override void InitGame()
        {
            base.InitGame();
        }
        protected override ObservableCollection<CardEntity> GenerateGrid()
        {
            ObservableCollection<CardEntity> result = new ObservableCollection<CardEntity>();
            CardEntity cell;
            int noCardTypes = Enum.GetValues(typeof(CardTypes)).Length;
            int CardsPerType = _settings.CardsNumber / noCardTypes;
            int cardId = 0;
            foreach (CardTypes cardType in Enum.GetValues(typeof(CardTypes)))
            {
                for (int i = 0; i < CardsPerType; i++)
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
            if (settings is S newSettings)
            {
                _settings = newSettings;
                if (MainUser != null)
                {
                    _gameDataMainUser = InitUserGameData(_settings, MainUserName, _gameDataMainUser.GameId);
                    MainUser.Proxy.UpdateData(_gameDataMainUser);
                }
            }
        }
        protected override void MenageGameUsers()
        {
            base.MenageGameUsers();
            if (MainUser != null)
            {
                _gameDataMainUser = InitUserGameData(_settings, MainUserName);
                MainUser.Proxy.SaveData(_gameDataMainUser);
            }
        }

        protected override void SaveGameResults()
        {
            if (MainUser != null)
            {
                MainUser.Proxy.UpdateData(_gameDataMainUser);
            }
        }
        #endregion
        #region Virtual Methods
        #endregion

        #region Private Methods
        protected virtual G InitUserGameData(S settings, string username, int? gameId = null)
        {
            return new G();
        }
        protected void setDim(double _cardDim, int _cardsPerRow, int _cardsPerColumn)
        {
            CellDim = _cardDim;
            cardsPerRow = _cardsPerRow;
            cardsPerColumn = _cardsPerColumn;
        }
        protected virtual void InitSettings()
        {
        }
        protected void SetCustomDimensions()
        {
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
        }
        protected virtual void OnCellClicked(int cellId)
        {
            
        }
        /// <summary>
        /// </summary>
        /// <returns>
        /// -1 se la coppia selezionata dall'utente è errata
        /// 0 se l'utenta ha selezionato solo una carta
        /// 1 se la coppia selezionata dall'utente è corretta
        /// </returns>
        protected int CheckCardTurned()
        {
            int result = 0;
            if (_cellClicked.Count() != 2) return result;
            else
            {
                if (_cellClicked[0].CardType == _cellClicked[1].CardType)
                {
                    _cellClicked[0].CardEnabled = false;
                    _cellClicked[1].CardEnabled = false;
                    _cellClicked.Clear();
                    PlayCardFlip();
                    result = 1;
                }
                else
                {
                    result = -1;
                    //turn back cards async so UI updates
                    Task.Run(() =>
                    {
                        IsGameEnabled = false;
                        Thread.Sleep(WAIT_TIME_CARD_TURN_BACK);
                        _cellClicked[0].CardTurned = false;
                        PlayCardFlip();
                        _cellClicked[1].CardTurned = false;
                        PlayCardFlip();
                        _cellClicked.Clear();
                        IsGameEnabled = true;
                    });
                }
            }
            return result;
        }

        protected void PlayCardFlip()
        {
            Random rdm = new Random();
            string sound = _cardsSound[rdm.Next(_cardsSound.Count())];
            PlaySound(sound);
        }
        #endregion
    }
}
