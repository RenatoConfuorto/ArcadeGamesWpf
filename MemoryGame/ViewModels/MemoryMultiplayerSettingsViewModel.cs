using Core.Attributes;
using Core.Commands;
using LIB.Constants;
using LIB.Entities;
using LIB.Helpers;
using LIB.ViewModels;
using MemoryGame.Common.Entities;
using MemoryGame.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using static LIB.Entities.Data.Base.GameEnums;
using static MemoryGame.Common.Constants;

namespace MemoryGame.ViewModels
{
    [ViewRef(typeof(MemoryMultiplayerSettingsView))]
    public class MemoryMultiplayerSettingsViewModel : SettingsViewModelBase<MemoryMultiplayerSettings>
    {
        #region Private fields
        private const int MAX_CARD_PER_TYPE = 20;
        private MemoryMultiplayerUser _selectedMemoryUser;
        private List<User> _registeredUsers;
        private User _selectedUser;
        private string _createdUser;
        #endregion

        #region Public Properties
        public int CardsPerType
        {
            get => (int)Settings?.CardsNumber / 6;
            set
            {
                if(Settings != null)
                {
                    Settings.CardsNumber = value * 6;
                }
                NotifyPropertyChanged();
                SetCommandExecutionStatus();
            }
        }
        public int ErrorsLimit
        {
            get => (int)Settings?.ErrorsLimit;
            set
            {
                if(Settings != null)
                {
                    Settings.ErrorsLimit = value;
                }
                NotifyPropertyChanged();
                SetCommandExecutionStatus();
            }
        }
        public ObservableCollection<MemoryMultiplayerUser> Users
        {
            get => Settings?.Users;
            set
            {
                if( Settings != null)
                {
                    Settings.Users = value;
                }
                NotifyPropertyChanged();
                SetCommandExecutionStatus();
            }
        }
        public MemoryMultiplayerUser SelectedMemoryUser
        {
            get => _selectedMemoryUser;
            set
            {
                SetProperty(ref _selectedMemoryUser, value);
                SetCommandExecutionStatus();
            }
        }
        public List<User> RegisteredUsers
        {
            get => _registeredUsers;
            set => SetProperty(ref _registeredUsers, value);
        }
        public User SelectedUser
        {
            get => _selectedUser;
            set
            {
                SetProperty(ref _selectedUser, value);
                SetCommandExecutionStatus();
            }
        }
        public string CreatedUser
        {
            get => _createdUser;
            set
            {
                SetProperty(ref _createdUser, value);
                SetCommandExecutionStatus();
            }
        }
        #endregion

        #region Commands
        public RelayCommand AddPair { get; set; }
        public RelayCommand RemovePair { get; set; }
        public RelayCommand AddErrorLimit { get; set; }
        public RelayCommand RemoveErrorLimit { get; set; }
        public RelayCommand AddErrorLimitMultiple { get; set; }
        public RelayCommand RemoveErrorLimitMultiple { get; set; }
        public RelayCommand MoveUserUpCommand { get; set; }
        public RelayCommand MoveDownUserCommand { get; set; }
        public RelayCommand RemoveUserCommand { get; set; }
        public RelayCommand AddRegisterdUser { get; set; }
        public RelayCommand AddCreatedUser { get; set; }
        #endregion

        #region Constructor
        public MemoryMultiplayerSettingsViewModel(object param)
            : base(ViewNames.MemoryMultiplayerSettings, param)
        {
        }
        #endregion

        #region Override Methods
        protected override void OnInitialized()
        {
            base.OnInitialized();
            RegisteredUsers = UserHelper.GetUsers();
        }
        protected override void GetViewParameter()
        {
            base.GetViewParameter();
            SetCommandExecutionStatus();
        }
        protected override void InitCommands()
        {
            base.InitCommands();
            AddPair = new RelayCommand(AddPairExecute, AddPairCanExecute);
            RemovePair = new RelayCommand(RemovePairExecute, RemovePairCanExecute);
            AddErrorLimit = new RelayCommand(AddErrorLimitExecute, AddErrorLimitCanExecute);
            RemoveErrorLimit = new RelayCommand(RemoveErrorLimitExecute, RemoveErrorLimitCanExecute);
            AddErrorLimitMultiple = new RelayCommand(AddErrorLimitMultipleExecute, AddErrorLimitMultipleCanExecute);
            RemoveErrorLimitMultiple = new RelayCommand(RemoveErrorLimitMultipleExecute, RemoveErrorLimitMultipleCanExecute);
            MoveUserUpCommand = new RelayCommand(MoveUserUpCommandExecute, MoveUserUpCommandCanExecute);
            MoveDownUserCommand = new RelayCommand(MoveDownUserCommandExecute, MoveDownUserCommandCanExecute);
            RemoveUserCommand = new RelayCommand(RemoveUserCommandExecute, RemoveUserCommandCanExecute);
            AddRegisterdUser = new RelayCommand(AddRegisterdUserExecute, AddRegisterdUserCanExecute);
            AddCreatedUser = new RelayCommand(AddCreatedUserExecute, AddCreatedUserCanExecute);
        }
        protected override void SetCommandExecutionStatus()
        {
            base.SetCommandExecutionStatus();
            AddPair.RaiseCanExecuteChanged();
            RemovePair.RaiseCanExecuteChanged();
            AddErrorLimit.RaiseCanExecuteChanged();
            RemoveErrorLimit.RaiseCanExecuteChanged();
            AddErrorLimitMultiple.RaiseCanExecuteChanged();
            RemoveErrorLimitMultiple.RaiseCanExecuteChanged();
            MoveUserUpCommand.RaiseCanExecuteChanged();
            MoveDownUserCommand.RaiseCanExecuteChanged();
            RemoveUserCommand.RaiseCanExecuteChanged();
            AddRegisterdUser.RaiseCanExecuteChanged();
            AddCreatedUser.RaiseCanExecuteChanged();
        }
        #endregion

        #region Private Methods
        private bool IsUserNotPresentInList(string userName)
        {
            return Users.Where(u => u.Name == userName).Count() == 0;
        }
        //Commands Methods
        private void AddPairExecute(object param)
        {
            CardsPerType += 2;
        }
        private bool AddPairCanExecute(object param) => CardsPerType < MAX_CARD_PER_TYPE;
        private void RemovePairExecute(object param)
        {
            CardsPerType -= 2;
        }
        private bool RemovePairCanExecute(object param) => CardsPerType > 2;

        private void AddErrorLimitExecute(object param) => ErrorsLimit++;
        private bool AddErrorLimitCanExecute(object param) => true;
        private void RemoveErrorLimitExecute(object param) => ErrorsLimit--;
        private bool RemoveErrorLimitCanExecute(object param) => ErrorsLimit > 1;

        private void AddErrorLimitMultipleExecute(object param) => ErrorsLimit+=5;
        private bool AddErrorLimitMultipleCanExecute(object param) => AddErrorLimitCanExecute(null);
        private void RemoveErrorLimitMultipleExecute(object param)
        {
            if (ErrorsLimit > 5) ErrorsLimit -= 5;
            else ErrorsLimit = 1;
        }
        private bool RemoveErrorLimitMultipleCanExecute(object param) => RemoveErrorLimitCanExecute(null);

        private void MoveUserUpCommandExecute(object param)
        {
            int userIdx = Users.IndexOf(SelectedMemoryUser);
            MemoryMultiplayerUser user = SelectedMemoryUser;
            Users.RemoveAt(userIdx);
            Users.Insert(userIdx - 1, user);
            SelectedMemoryUser = Users[userIdx - 1];
        }
        private bool MoveUserUpCommandCanExecute(object param) => SelectedMemoryUser != null && Users.IndexOf(SelectedMemoryUser) != 0;
        private void MoveDownUserCommandExecute(object param)
        {
            int userIdx = Users.IndexOf(SelectedMemoryUser);
            MemoryMultiplayerUser user = SelectedMemoryUser;
            Users.RemoveAt(userIdx);
            Users.Insert(userIdx + 1, user);
            SelectedMemoryUser = Users[userIdx + 1];
            //NotifyPropertyChanged(nameof(Users));
        }
        private bool MoveDownUserCommandCanExecute(object param) => SelectedMemoryUser != null && Users.IndexOf(SelectedMemoryUser) != Users.Count - 1;
        private void RemoveUserCommandExecute(object param) 
        {
            Users.Remove(SelectedMemoryUser);
            SetCommandExecutionStatus();
        }
        private bool RemoveUserCommandCanExecute(object param) => SelectedMemoryUser != null && Users.Count > 2;

        private void AddRegisterdUserExecute(object param)
        {
            Users.Add(new MemoryMultiplayerUser(SelectedUser));
            SetCommandExecutionStatus();
        }
        private bool AddRegisterdUserCanExecute(object param) => SelectedUser != null && IsUserNotPresentInList(SelectedUser.Name);
        private void AddCreatedUserExecute(object param)
        {
            Users.Add(new MemoryMultiplayerUser(CreatedUser));
            SetCommandExecutionStatus();
        }
        private bool AddCreatedUserCanExecute(object param) => !String.IsNullOrEmpty(CreatedUser) && IsUserNotPresentInList(CreatedUser);
        #endregion
    }
}
