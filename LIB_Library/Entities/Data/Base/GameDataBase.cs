using Core.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace LIB.Entities.Data.Base
{
    public class GameDataBase : NotifyerPropertyChangedBase
    {
        private int _gameId = -1; //default value
        private string _gameGUID;
        private string _userName;
        private DateTime _gameDate;

        public GameDataBase()
        {
        }

        public GameDataBase(string gameGUID, 
            string userName, 
            DateTime gameDate)
        {
            GameId = -1;
            GameGUID = gameGUID;
            UserName = userName;
            GameDate = gameDate;
        }

        public int GameId
        {
            get => _gameId;
            set => SetProperty(ref _gameId, value);
        }
        public string GameGUID
        {
            get => _gameGUID;
            set => SetProperty(ref _gameGUID, value);
        }
        public string UserName
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
