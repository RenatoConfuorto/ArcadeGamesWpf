using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MemoryGame.Common.Constants;

namespace MemoryGame.Common.Interfaces
{
    public interface ICardEntity
    {
        int CellId { get; }
        CardTypes CardType { get; } 
        bool CardTurned { get; }
    }
}
