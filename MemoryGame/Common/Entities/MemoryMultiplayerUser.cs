using Core.Entities;
using LIB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace MemoryGame.Common.Entities
{
    public class MemoryMultiplayerUser : NotifyerPropertyChangedBase, ICloneable
    {
        public User _user;
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
        }
        public MemoryMultiplayerUser(User user)
            : this(user.Name)
        {
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
            set => SetProperty(ref _errors, value);
        }
        public bool CanPlay
        {
            get => _canPlay;
            set => SetProperty(ref _canPlay, value);
        }

        public int Points
        {
            get => _points;
            set => SetProperty(ref _points, value);
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
    }
}
