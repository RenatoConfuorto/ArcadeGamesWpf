using Core.Entities;
using LIB.Entities;
using LIB.Entities.Data.Memory;
using LIB.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Proxies;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace MemoryGame.Common.Entities
{
    public class MemoryMultiplayerUser : NotifyerPropertyChangedBase, ICloneable
    {
        public User _user;
        public GameDataMemoryMp _gameData;
        private string _name;
        private int _errors = 0;
        private int _points = 0;
        public bool _isActive;
        public bool _canPlay;
        public MemoryMultiplayerUser() { }
        public MemoryMultiplayerUser(string name) 
        {
            _name = name;
            _isActive = false;
            _canPlay = true;
            _gameData = new GameDataMemoryMp(_name, 
                DateTime.Now, 
                0, 0, 0, 0, 
                LIB.Entities.Data.Base.GameEnums.MemoryResult.defeat, 
                1);
        }
        public MemoryMultiplayerUser(User user)
            : this(user.Name)
        {
            if (user.Proxy == null) InitUserProxy();
            _user = user;
        }

        public string Name
        {
            get => String.IsNullOrEmpty(_name) ? _user?.Name : _name;
            set => SetProperty(ref _name, value);
        }

        public int Errors
        {
            get => _errors;
            set
            {
                SetProperty(ref _errors, value);
                _gameData.ErrorsNumber = value;
            }
        }
        public bool CanPlay
        {
            get => _canPlay;
            set => SetProperty(ref _canPlay, value);
        }

        public int Points
        {
            get => _points;
            set
            {
                SetProperty(ref _points, value);
                _gameData.Points = value;
            }
        }
       
        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value);
        }

        public void ResetData()
        {
            Errors = 0;
            Points = 0;
            IsActive = false;
            CanPlay = true;
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public void InitGameDataFromSettings(MemoryMultiplayerSettings settings, int? GameId = null)
        {
            _gameData = new GameDataMemoryMp(Name, 
                DateTime.Now, 
                settings.CardsNumber, 
                0, 
                settings.ErrorsLimit, 
                0, 
                LIB.Entities.Data.Base.GameEnums.MemoryResult.defeat, 
                settings.Users.Count() - 1);
            if (GameId != null) _gameData.GameId = (int)GameId;
        }
        public void UpdateGameData()
        {
            if (_user != null)
            {
                _user.Proxy.UpdateData(_gameData);
            }
        }
        public void SaveGameData()
        {
            if(_user != null)
            {
                _user.Proxy.SaveData(_gameData);
            }
        }
        public void InitUserProxy()
        {
            if(_user != null)
            {
                _user.Proxy = new UserProxy(_user.Name);
            }
        }
    }
}
