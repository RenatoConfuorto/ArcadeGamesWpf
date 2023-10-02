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

        public int CardsNumber
        {
            get => _cardsNumber;
            set => SetProperty(ref _cardsNumber, value);
        }
    }
}
