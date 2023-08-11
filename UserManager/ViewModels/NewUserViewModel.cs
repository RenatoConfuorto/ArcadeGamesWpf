using Core.Attributes;
using Core.Commands;
using Core.Helpers;
using LIB.Constants;
using LIB.Entities;
using LIB.Helpers;
using LIB.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManager.Views;

namespace UserManager.ViewModels
{
    [ViewRef(typeof(NewUserView))]
    public class NewUserViewModel : ContentViewModel
    {
        #region Private Fields
        private User _user;
        #endregion

        #region Public Properties
        public User User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }
        public string UserName
        {
            get => User?.Name;
            set
            {
                if(User != null)
                {
                    User.Name = value;
                    NotifyPropertyChanged();
                    SetCommandExecutionStatus();
                }
            }
        }
        public bool IsDefaultAccess
        {
            get => User.IsDefaultAccess;
            set
            {
                User.IsDefaultAccess = value;
                if (!value)
                {
                    IsFirstAccessChecked = false;
                    IsSecondAccessChecked = false;
                }
                else
                {
                    IsFirstAccessChecked = true;
                }
                NotifyPropertyChanged();
            }
        }
        public bool IsFirstAccessChecked
        {
            get => User?.AutoLoginOrder == 1;
            set
            {
                if (value) User.AutoLoginOrder = 1;
                else User.AutoLoginOrder = 0;
                NotifyPropertyChanged();
            }
        }
        public bool IsSecondAccessChecked
        {
            get => User?.AutoLoginOrder == 2;
            set
            {
                if (value) User.AutoLoginOrder = 2;
                else User.AutoLoginOrder = 0;
            }
        }
        #endregion

        #region Commands
        public RelayCommand SaveCommand { get; set; }
        #endregion

        #region Constructor
        public NewUserViewModel() 
            : base(ViewNames.NewUserPage, ViewNames.UserMngMainPage)
        {
        }
        #endregion

        #region Override Methods
        protected override void OnInitialized()
        {
            base.OnInitialized();
            _user = new User();
        }
        public override void Dispose()
        {
            base.Dispose();
            _user = new User();
        }
        protected override void InitCommands()
        {
            base.InitCommands();
            SaveCommand = new RelayCommand(SaveCommandExecute, SaveCommandCanExecute);
        }
        protected override void SetCommandExecutionStatus()
        {
            base.SetCommandExecutionStatus();
            SaveCommand.RaiseCanExecuteChanged();
        }
        #endregion

        #region Private Methods
        private void SaveCommandExecute(object param) 
        {
            string errorMessage = "Errore durante la creazione dell'utente.";
            if(!UserHelper.CreateNewUser(User, out errorMessage))
            {
                MessageDialogHelper.ShowInfoMessage(errorMessage);
            }
            else
            {
                MessageDialogHelper.ShowInfoMessage("Utente creato con successo.");
                ChangeView(ViewNames.UserMngMainPage);
            }
        }
        private bool SaveCommandCanExecute(object param) => !String.IsNullOrEmpty(User?.Name);
        #endregion

        #region Protected Methods
        #endregion
    }
}
