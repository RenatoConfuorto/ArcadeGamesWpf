using LIB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using static MemoryGame.Common.Constants;

namespace MemoryGame.Common.Entities
{
    public class MemorySingleplayerSettings : GameSettingsBase
    {
        private Difficulty _gameDifficulty;
        private int _cardsNumber;
        private int _errorsLimit;

        public Difficulty GameDifficulty
        {
            get => _gameDifficulty;
            set => SetProperty(ref _gameDifficulty, value);
        }
        public int CardsNumber
        {
            get => _cardsNumber;
            set => SetProperty(ref _cardsNumber, value);
        }
        public int ErrorsLimit
        {
            get => _errorsLimit;
            set => SetProperty(ref _errorsLimit, value);
        }
    }
}
