using LIB.Entities;
using MemoryGame.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MemoryGame.Common.Constants;

namespace MemoryGame.Common.Entities
{
    public class CardEntity : CellEntityBase, ICardEntity
    {
        private CardTypes _cardType;
        private bool _cardTurned;
        private bool _cardEnabled;

        public CardTypes CardType
        {
            get => _cardType;
            set => SetProperty(ref _cardType, value);
        }
        public bool CardTurned
        {
            get => _cardTurned;
            set => SetProperty(ref _cardTurned, value);
        }
        public bool CardEnabled
        {
            get => _cardEnabled;
            set => SetProperty(ref _cardEnabled, value);
        }
    }
}
