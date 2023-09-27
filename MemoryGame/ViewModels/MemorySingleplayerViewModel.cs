using Core.Attributes;
using Core.Commands;
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
using System.Threading.Tasks;
using static MemoryGame.Common.Constants;

namespace MemoryGame.ViewModels
{
    [ViewRef(typeof(MemorySingleplayer))]
    public class MemorySingleplayerViewModel : GameViewModelBase<CardEntity>
    {
        #region Private Fields
        private MemorySingleplayerSettings _settings;
        #endregion

        #region Public Properties
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
        }
        private void OnCellClicked(int cellId)
        {

        }
        #endregion
    }
}
