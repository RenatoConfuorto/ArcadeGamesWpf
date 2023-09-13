using LIB.Entities.Data.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.Entities.Data.Base
{
    public class GameDataTrisBase : GameDataBase
    {
        private bool _userHasWon;

        public bool UserHasWon
        {
            get => _userHasWon;
            set => SetProperty(ref _userHasWon, value);
        }
    }
}
