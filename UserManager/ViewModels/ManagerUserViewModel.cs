using Core.Attributes;
using Core.Commands;
using Core.Helpers;
using LIB.Attributes;
using LIB.Constants;
using LIB.Entities;
using LIB.Helpers;
using LIB.Sounds;
using LIB.UserMng;
using LIB.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using UserManager.Views;

namespace UserManager.ViewModels
{
    [ViewRef(typeof(ManageUserView))]
    [BackgroundMusic(SoundsManagment.MainBackground)]
    public class ManagerUserViewModel : ContentViewModel
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
                    User.AutoLoginOrder = 0;
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
                if(value)User.AutoLoginOrder = 1;
                //else User.AutoLoginOrder = 0;
                NotifyPropertyChanged();
            }
        }
        public bool IsSecondAccessChecked
        {
            get => User?.AutoLoginOrder == 2;
            set
            {
                if (value) User.AutoLoginOrder = 2;
                //else User.AutoLoginOrder = 0;
                NotifyPropertyChanged();
            }
        }
        #endregion

        #region Commands
        public RelayCommand UpdateCommand { get; set; } 
        public RelayCommand DeleteCommand { get; set; }
        #endregion

        #region Constructor
        public ManagerUserViewModel(object param) : base(ViewNames.ManageUserView, ViewNames.UserMngMainPage, param)
        {
        }
        #endregion

        #region Override Methods
        protected override void OnInitialized()
        {
            base.OnInitialized();
        }
        public override void Dispose()
        {
            base.Dispose();
        }
        protected override void GetViewParameter()
        {
            if(ViewParam != null)
            {
                if(ViewParam is User)
                {
                    User = (User)ViewParam;
                }
            }
            else
            {
                //error
            }
        }
        protected override void InitCommands()
        {
            base.InitCommands();
            UpdateCommand = new RelayCommand(UpdateCommandExecute);
            DeleteCommand = new RelayCommand(DeleteCommandExecute);
        }
        #endregion

        #region Private Methods
        #endregion

        #region Protected Methods
        protected void UpdateCommandExecute(object param)
        {
            string errorMessage = String.Empty;
            if(!UserHelper.UpdateUser(User, out errorMessage))
            {
                MessageDialogHelper.ShowInfoMessage(errorMessage);
            }
            else
            {
                MessageDialogHelper.ShowInfoMessage("Utente aggiornato con successo.");
                ChangeView(ParentView);
            }
        }
        protected void DeleteCommandExecute(object param)
        {
            string errorMessage;
            if(MessageDialogHelper.ShowConfirmationRequestMessage($"Eliminare l'utente {User.Name}?"))
            {
                if(UserHelper.DeleteUser(User.Name, out errorMessage))
                {
                    switch (LIB.UserMng.UserManager.IsUserLoggedIn(User.Name))
                    {
                        case 1:
                            LIB.UserMng.UserManager.MainLoggedUser.UserLogOut();
                            User secondaryUser = LIB.UserMng.UserManager.SecondLoggedUser.CurrentUser;
                            if (secondaryUser != null)
                            {//rendere l'account secondary quello principale
                                LIB.UserMng.UserManager.MainLoggedUser.UserLogIn(secondaryUser);
                                LIB.UserMng.UserManager.SecondLoggedUser.UserLogOut();
                            }
                            break;
                        case 2:
                            LIB.UserMng.UserManager.SecondLoggedUser.UserLogOut();
                            break;
                    }
                    MessageDialogHelper.ShowInfoMessage("Utente eliminato");
                    ChangeView(ParentView);
                }
            }
        }

        #endregion
    }
}
