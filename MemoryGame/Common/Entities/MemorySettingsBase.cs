using LIB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryGame.Common.Entities
{
    public class MemorySettingsBase : GameSettingsBase
    {
        private int _cardsNumber;
        private int _errorsLimit;

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
