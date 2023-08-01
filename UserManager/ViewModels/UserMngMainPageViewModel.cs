using Core.Attributes;
using Core.Commands;
using Core.Helpers;
using LIB.Constants;
using LIB.Entities;
using LIB.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManager.Views;

namespace UserManager.ViewModels
{
    [ViewRef(typeof(UserMngMainPage))]
    public class UserMngMainPageViewModel : ContentViewModel
    {
        #region Private Fields
        private ObservableCollection<User> _users;
        private User _selectedUser;
        #endregion

        #region Commands
        public RelayCommand NewCommand { get; set; }
        public RelayCommand ManageCommand { get; set; }
        #endregion

        #region Public Properties
        public ObservableCollection<User> Users
        {
            get => _users;
            set
            {
                SetProperty(ref _users, value);
                SetCommandExecutionStatus();
            }
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
        #endregion

        #region Constructor
        public UserMngMainPageViewModel()
            : base(ViewNames.UserMngMainPage, ViewNames.Home)
        {
            
        }
        #endregion

        #region Override Methods
        protected override void OnInitialized()
        {
            base.OnInitialized();
            Users = new ObservableCollection<User>()
            {
                new User() {Id=0, Name="Test1", Created=DateTime.Now, Updated=DateTime.Now, IsDefaultAccess=true},
                new User() {Id=1, Name="Test2", Created=DateTime.Now, Updated=DateTime.Now, IsDefaultAccess=false},
                new User() {Id=2, Name="Test3", Created=DateTime.Now, Updated=DateTime.Now, IsDefaultAccess=false},
                new User() {Id=3, Name="Test4", Created=DateTime.Now, Updated=DateTime.Now, IsDefaultAccess=false},
            };
            string test = XmlSerializerBase.SerializeObjectToString(Users[1]);
        }
        public override void Dispose()
        {
            base.Dispose();
        }

        protected override void InitCommands()
        {
            base.InitCommands();
            NewCommand = new RelayCommand(NewCommandExecute, NewCommandCanExecute);
            ManageCommand = new RelayCommand(ManageCommandExecute, ManageCommandCanExecute);
        }

        protected override void SetCommandExecutionStatus()
        {
            base.SetCommandExecutionStatus();
            NewCommand.RaiseCanExecuteChanged();
            ManageCommand.RaiseCanExecuteChanged();
        }
        #endregion

        #region Private Methods
        private void NewCommandExecute(object param) {}
        private bool NewCommandCanExecute(object param) => true;
        private void ManageCommandExecute(object param) {}
        private bool ManageCommandCanExecute(object param) => SelectedUser != null;
        #endregion

        #region Protected Methods
        #endregion
    }
}
