using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryGame.Common.Entities
{
    public class MemoryMultiplayerSettings : MemorySettingsBase
    {
        private ObservableCollection<MemoryMultiplayerUser> _users = new ObservableCollection<MemoryMultiplayerUser>();
        private bool _isErrorLimitEnabled;

        public ObservableCollection<MemoryMultiplayerUser> Users
        {
            get => _users;
            set => SetProperty(ref _users, value);
        }
        public bool IsErrorLimitEnabled
        {
            get => _isErrorLimitEnabled;
            set => SetProperty(ref _isErrorLimitEnabled, value);
        }
    }
}
