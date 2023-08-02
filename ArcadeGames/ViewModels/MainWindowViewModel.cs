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

namespace ArcadeGames.ViewModels
{
    [ViewRef(typeof(MainWindow))]
    public class MainWindowViewModel : ViewModelBase
    {
        #region Private Fields
        #endregion

        #region Public Properties
        #endregion

        #region Commands
        public RelayCommand ShoutDownCommand { get; set; }
        public RelayCommand UserManagerCommand { get; set; }
        public RelayCommand PreviousPageCommand { get; set; }
        public RelayCommand ReloadPageCommand { get; set; }
        #endregion

        #region Constructor
        public MainWindowViewModel() 
            : base(ViewNames.MainWindow)
        {
            NavigateToView(ViewNames.Home);
        }
        #endregion

        #region Override Methods
        protected override void InitCommands()
        {
            base.InitCommands();
            ShoutDownCommand = new RelayCommand(ShoutDownCommandExecute);
            PreviousPageCommand = new RelayCommand(PreviousPageCommandExecute, PreviousPageCommandCanExecute);
            ReloadPageCommand = new RelayCommand(ReloadPageCommandExecute);
            UserManagerCommand = new RelayCommand(UserManagerCommandExecute);
        }
        protected override void SetCommandExecutionStatus()
        {
            base.SetCommandExecutionStatus();
            PreviousPageCommand.RaiseCanExecuteChanged();
            ReloadPageCommand.RaiseCanExecuteChanged();
        }
        #endregion

        #region Private Methods
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
        private void ReloadPageCommandExecute(object param) { NavigateToView(Navigation.CurrentView.ViewName); }

        private void UserManagerCommandExecute(object param)
        {
            NavigateToView(ViewNames.UserMngMainPage);
        }
        #endregion
    }
}
