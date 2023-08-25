using Core.Commands;
using LIB.Constants;
using Core.Helpers;
using Core.Interfaces.Navigation;
using Core.ViewModels;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Core.Attributes;
using ArcadeGames.Views;
using LIB.Entities;
using LIB.UserMng;
using LIB.Helpers;
using System.ComponentModel;

namespace ArcadeGames.ViewModels
{
    [ViewRef(typeof(MainWindow))]
    public class MainWindowViewModel : ViewModelBase
    {
        #region Private Fields
        private User _currentUser;
        private User _secondUser;
        #endregion

        #region Public Properties
        public User CurrentUser
        {
            get => _currentUser;
            set => SetProperty(ref _currentUser, value);
        }
        public string CurrentUserName
        {
            get => CurrentUser != null ? CurrentUser.Name : "---";
        }
        public User SecondUser
        {
            get => _secondUser;
            set => SetProperty(ref _secondUser, value);
        }
        public string SecondUserName
        {
            get => SecondUser != null ? SecondUser.Name : "---";
        }
        #endregion

        #region Commands
        public RelayCommand ShoutDownCommand { get; set; }
        public RelayCommand UserManagerCommand { get; set; }
        public RelayCommand PreviousPageCommand { get; set; }
        public RelayCommand ReloadPageCommand { get; set; }
        public RelayCommand ManageUserCommand { get; set; }
        #endregion

        #region Constructor
        public MainWindowViewModel() 
            : base(ViewNames.MainWindow)
        {
            UserManager.MainLoggedUser.PropertyChanged += OnLoggedUserChanged;
            UserManager.SecondLoggedUser.PropertyChanged += OnLoggedUserChanged;
            NavigateToView(ViewNames.Home);
            UserHelper.AutoLogInUsers();
        }
        #endregion

        #region Override Methods
        protected override void OnInitialized()
        {
            base.OnInitialized();
        }
        protected override void InitCommands()
        {
            base.InitCommands();
            ShoutDownCommand = new RelayCommand(ShoutDownCommandExecute);
            PreviousPageCommand = new RelayCommand(PreviousPageCommandExecute, PreviousPageCommandCanExecute);
            ReloadPageCommand = new RelayCommand(ReloadPageCommandExecute);
            UserManagerCommand = new RelayCommand(UserManagerCommandExecute);
            ManageUserCommand = new RelayCommand(ManageUserCommandExecute);
        }
        protected override void SetCommandExecutionStatus()
        {
            base.SetCommandExecutionStatus();
            PreviousPageCommand.RaiseCanExecuteChanged();
            ReloadPageCommand.RaiseCanExecuteChanged();
            ManageUserCommand.RaiseCanExecuteChanged();
        }
        #endregion

        #region Private Methods
        private void OnLoggedUserChanged(object sender, PropertyChangedEventArgs e)
        {
            CurrentUser = UserManager.MainLoggedUser.CurrentUser;
            NotifyPropertyChanged(nameof(CurrentUserName));
            SecondUser = UserManager.SecondLoggedUser.CurrentUser;
            NotifyPropertyChanged(nameof(SecondUserName));
        }
        #endregion

        #region Command Methods
        private void ShoutDownCommandExecute(object param)
        {
            if(MessageDialogHelper.ShowConfirmationRequestMessage("Uscire dall'applicazione?"))
            {
                Application.Current.Shutdown();
            }
        }
        private void PreviousPageCommandExecute(object param) 
        {
            if (String.IsNullOrEmpty(Navigation.ParentViewName))
            {
                NavigateToView(ViewNames.Home);
            }
            else
            {
                NavigateToView(Navigation.ParentViewName);
            }
        }
        private bool PreviousPageCommandCanExecute(object param) => !String.IsNullOrEmpty(Navigation.ParentViewName);
        private void ReloadPageCommandExecute(object param) 
        {
            Navigation.CurrentView.InitViewModel();
        }

        private void UserManagerCommandExecute(object param)
        {
            NavigateToView(ViewNames.UserMngMainPage);
        }
        private void ManageUserCommandExecute(object param)
        {
            if (param == null) return;
            NavigateToView(ViewNames.ManageUserView, param);
        }
        #endregion
    }
}
