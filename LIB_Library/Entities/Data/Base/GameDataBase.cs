using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace LIB.Entities.Data.Base
{
    public class GameDataBase : NotifyerPropertyChangedBase
    {
        private string _gameGUID;
        private string _userName;
        private DateTime _gameDate;

        public string GameGUID
        {
            get => _gameGUID;
            set => SetProperty(ref _gameGUID, value);
        }
        public string USerName
        {
            get => _userName;
            set =>SetProperty(ref _userName, value);
        }
        public DateTime GameDate
        {
            get => _gameDate;
            set => SetProperty(ref _gameDate, value);
        }
    }
}
