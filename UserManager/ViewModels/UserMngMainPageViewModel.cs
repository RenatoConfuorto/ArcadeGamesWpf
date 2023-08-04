using Core.Attributes;
using Core.Commands;
using Core.Helpers;
using LIB.Constants;
using LIB.Entities;
using LIB.Helpers;
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
        public RelayCommand LogInMainCommand { get; set; }
        public RelayCommand LogInSecondCommand { get; set; }
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
            Users = new ObservableCollection<User>(UserHelper.GetUsers());
            
        }
        public override void Dispose()
        {
            base.Dispose();
            Users = null;
        }

        protected override void InitCommands()
        {
            base.InitCommands();
            NewCommand = new RelayCommand(NewCommandExecute, NewCommandCanExecute);
            LogInMainCommand = new RelayCommand(LogInMainCommandExecute, LogInMainCommandCanExecute);
            LogInSecondCommand = new RelayCommand(LogInSecondCommandExecute, LogInSecondCommandCanExecute);
            ManageCommand = new RelayCommand(ManageCommandExecute, ManageCommandCanExecute);
            NotifyPropertyChanged(nameof(NewCommand));
            NotifyPropertyChanged(nameof(LogInMainCommand));
            NotifyPropertyChanged(nameof(LogInSecondCommand));
            NotifyPropertyChanged(nameof(ManageCommand));
        }

        protected override void SetCommandExecutionStatus()
        {
            base.SetCommandExecutionStatus();
            NewCommand.RaiseCanExecuteChanged();
            LogInMainCommand.RaiseCanExecuteChanged();
            LogInSecondCommand.RaiseCanExecuteChanged();
            ManageCommand.RaiseCanExecuteChanged();
        }
        #endregion

        #region Private Methods
        private void NewCommandExecute(object param) 
        {
            ChangeView(ViewNames.NewUserPage);
        }
        private void GetLoggedUsers(out User mainLoggedUser, out User secondLoggedUser)
        {
            mainLoggedUser = LIB.UserMng.UserManager.MainLoggedUser.CurrentUser;
            secondLoggedUser = LIB.UserMng.UserManager.SecondLoggedUser.CurrentUser;
        }
        private bool NewCommandCanExecute(object param) => true;
        private void LogInMainCommandExecute(object param) 
        {
            if (LIB.UserMng.UserManager.MainLoggedUser.CurrentUser != null)
            {
                if (MessageDialogHelper.ShowConfirmationRequestMessage("È già stato effettuato l'accesso con un altro utente. \r\n Cambiare l'utente attuale?"))
                {
                    LIB.UserMng.UserManager.MainLoggedUser.UserLogIn(SelectedUser);
                    MessageDialogHelper.ShowInfoMessage("Accesso effettuato");
                }
            }
            else
            {
                LIB.UserMng.UserManager.MainLoggedUser.UserLogIn(SelectedUser);
                MessageDialogHelper.ShowInfoMessage("Accesso effettuato");
            }
            SetCommandExecutionStatus();
            
        }
        private bool LogInMainCommandCanExecute(object param)
        {
            User main;
            User second;
            GetLoggedUsers(out main, out second);
            //return SelectedUser != null && main != null && SelectedUser?.Name != main?.Name && SelectedUser?.Name != second?.Name;
            return SelectedUser != null && main?.Name != SelectedUser?.Name && second?.Name != SelectedUser?.Name;
        }
        private void LogInSecondCommandExecute(object param) 
        {
            if (LIB.UserMng.UserManager.SecondLoggedUser.CurrentUser != null)
            {
                if (MessageDialogHelper.ShowConfirmationRequestMessage("È già stato effettuato l'accesso con un altro utente. \r\n Cambiare l'utente attuale?"))
                {
                    LIB.UserMng.UserManager.SecondLoggedUser.UserLogIn(SelectedUser);
                    MessageDialogHelper.ShowInfoMessage("Accesso effettuato");
                }
            }
            else
            {
                LIB.UserMng.UserManager.SecondLoggedUser.UserLogIn(SelectedUser);
                MessageDialogHelper.ShowInfoMessage("Accesso effettuato");
            }
            SetCommandExecutionStatus();
        }
        private bool LogInSecondCommandCanExecute(object param)
        {
            User main;
            User second;
            GetLoggedUsers(out main, out second);
            return SelectedUser != null && main != null && SelectedUser?.Name != main?.Name && SelectedUser?.Name != second?.Name;
        }
        private void ManageCommandExecute(object param) {}
        private bool ManageCommandCanExecute(object param) => SelectedUser != null;
        #endregion

        #region Protected Methods
        #endregion
    }
}
