using LIB.Attributes;
using LIB.Base;
using LIB.Constants;
using LIB.Helpers;
using LIB.Interfaces.Navigation;
using LIB.Navigation;
using LIB.ViewModels;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ArcadeGames.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Private Fields
        #endregion

        #region Public Properties
        #endregion

        #region Commands
        public RelayCommand ShoutDownCommand { get; set; }
        public RelayCommand PreviousPageCommand { get; set; }
        #endregion

        #region Constructor
        public MainWindowViewModel() 
            : base()
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
        }
        protected override void SetCommandExecutionStatus()
        {
            base.SetCommandExecutionStatus();
            PreviousPageCommand.RaiseCanExecuteChanged();
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
        #endregion
    }
}
