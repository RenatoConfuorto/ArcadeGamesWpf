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
using System.ComponentModel.Design;
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

        private User _mainUser;
        private User _secondUser;
        #endregion

        #region Commands
        public RelayCommand NewCommand { get; set; }
        public RelayCommand LogInMainCommand { get; set; }
        public RelayCommand LogInSecondCommand { get; set; }
        public RelayCommand ManageCommand { get; set; }
        public RelayCommand LogOutMainUserCommand { get; set; }
        public RelayCommand LogOutSecondUserCommand { get; set; }
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

        public User MainUser
        {
            get => _mainUser;
            set
            {
                SetProperty(ref _mainUser, value);
                NotifyPropertyChanged(nameof(MainUserName));
            }
        }
        public string MainUserName
        {
            get => MainUser != null ? MainUser.Name : "---";
        }
        public User SecondUser
        {
            get => _secondUser;
            set
            {
                SetProperty(ref _secondUser, value);
                NotifyPropertyChanged(nameof(SecondUserName));
            }
        }
        public string SecondUserName
        {
            get => SecondUser != null ? SecondUser.Name : "---";
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
            //Load logged users
            MainUser = LIB.UserMng.UserManager.MainLoggedUser.CurrentUser;
            SecondUser = LIB.UserMng.UserManager.SecondLoggedUser.CurrentUser;
            
        }
        public override void Dispose()
        {
            base.Dispose();
            Users = null;
            MainUser = null;
            SecondUser = null;
        }

        protected override void InitCommands()
        {
            base.InitCommands();
            NewCommand = new RelayCommand(NewCommandExecute, NewCommandCanExecute);
            LogInMainCommand = new RelayCommand(LogInMainCommandExecute, LogInMainCommandCanExecute);
            LogInSecondCommand = new RelayCommand(LogInSecondCommandExecute, LogInSecondCommandCanExecute);
            ManageCommand = new RelayCommand(ManageCommandExecute, ManageCommandCanExecute);
            LogOutMainUserCommand = new RelayCommand(LogOutMainUserCommandExecute, LogOutMainUserCommandCanExecute);
            LogOutSecondUserCommand = new RelayCommand(LogOutSecondUserCommandExecute, LogOutSecondUserCommandCanExecute);
            NotifyPropertyChanged(nameof(NewCommand));
            NotifyPropertyChanged(nameof(LogInMainCommand));
            NotifyPropertyChanged(nameof(LogInSecondCommand));
            NotifyPropertyChanged(nameof(ManageCommand));
            NotifyPropertyChanged(nameof(LogOutMainUserCommand));
            NotifyPropertyChanged(nameof(LogOutSecondUserCommand));
        }

        protected override void SetCommandExecutionStatus()
        {
            base.SetCommandExecutionStatus();
            NewCommand.RaiseCanExecuteChanged();
            LogInMainCommand.RaiseCanExecuteChanged();
            LogInSecondCommand.RaiseCanExecuteChanged();
            ManageCommand.RaiseCanExecuteChanged();
            LogOutMainUserCommand.RaiseCanExecuteChanged();
            LogOutSecondUserCommand.RaiseCanExecuteChanged();
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

        // COMMANDS METHODS
        private bool NewCommandCanExecute(object param) => true;
        private void LogInMainCommandExecute(object param) 
        {
            if (LIB.UserMng.UserManager.MainLoggedUser.CurrentUser != null)
            {
                if (MessageDialogHelper.ShowConfirmationRequestMessage("È già stato effettuato l'accesso con un altro utente. \r\n Cambiare l'utente attuale?"))
                {
                    LIB.UserMng.UserManager.MainLoggedUser.UserLogIn(SelectedUser);
                    MessageDialogHelper.ShowInfoMessage("Accesso effettuato");
                    MainUser = SelectedUser;
                }
            }
            else
            {
                LIB.UserMng.UserManager.MainLoggedUser.UserLogIn(SelectedUser);
                MessageDialogHelper.ShowInfoMessage("Accesso effettuato");
                MainUser = SelectedUser;
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
            //return SelectedUser.Name != MainUser?.Name;
        }
        private void LogInSecondCommandExecute(object param) 
        {
            if (LIB.UserMng.UserManager.SecondLoggedUser.CurrentUser != null)
            {
                if (MessageDialogHelper.ShowConfirmationRequestMessage("È già stato effettuato l'accesso con un altro utente. \r\n Cambiare l'utente attuale?"))
                {
                    LIB.UserMng.UserManager.SecondLoggedUser.UserLogIn(SelectedUser);
                    MessageDialogHelper.ShowInfoMessage("Accesso effettuato");
                    SecondUser = SelectedUser;
                }
            }
            else
            {
                LIB.UserMng.UserManager.SecondLoggedUser.UserLogIn(SelectedUser);
                MessageDialogHelper.ShowInfoMessage("Accesso effettuato");
                SecondUser = SelectedUser;
            }
            SetCommandExecutionStatus();
        }
        private bool LogInSecondCommandCanExecute(object param)
        {
            User main;
            User second;
            GetLoggedUsers(out main, out second);
            return SelectedUser != null && main != null && SelectedUser?.Name != main?.Name && SelectedUser?.Name != second?.Name;
            //return SelectedUser.Name != SecondUser?.Name;
        }
        private void ManageCommandExecute(object param) 
        {
            ChangeView(ViewNames.ManageUserView, SelectedUser);
        }
        private bool ManageCommandCanExecute(object param) => SelectedUser != null;

        private void LogOutMainUserCommandExecute(object param) 
        {
            if(MessageDialogHelper.ShowConfirmationRequestMessage("Disconnettere l'account?"))
            {
                LIB.UserMng.UserManager.MainLoggedUser.UserLogOut();
                MainUser = null;
                MessageDialogHelper.ShowInfoMessage("Account disconnesso");

                if(SecondUser != null)
                {//rendere l'account secondary quello principale
                    LIB.UserMng.UserManager.SecondLoggedUser.UserLogOut();
                    LIB.UserMng.UserManager.MainLoggedUser.UserLogIn(SecondUser);
                    MainUser = SecondUser;
                    SecondUser = null;
                }
                SetCommandExecutionStatus();
            }
        }
        private bool LogOutMainUserCommandCanExecute(object param) => MainUser != null;
        private void LogOutSecondUserCommandExecute(object param) 
        {
            if (MessageDialogHelper.ShowConfirmationRequestMessage("Disconnettere l'account?"))
            {
                LIB.UserMng.UserManager.SecondLoggedUser.UserLogOut();
                SecondUser = null;
                MessageDialogHelper.ShowInfoMessage("Account disconnesso");
            }
            SetCommandExecutionStatus();
        }
        private bool LogOutSecondUserCommandCanExecute(object param) => SecondUser != null;
        #endregion

        #region Protected Methods
        #endregion
    }
}
