using Core.Entities;
using Core.Helpers;
using LIB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.UserMng
{
    public class UserManager : NotifyerPropertyChangedBase
    {
        #region Static Properties
        private static UserManager _mainLoggedUser;
        public static UserManager MainLoggedUser
        {
            get
            {
                if(_mainLoggedUser == null) _mainLoggedUser = new UserManager();
                return _mainLoggedUser;
            }
        }

        private static UserManager _secondLoggedUser;
        public static UserManager SecondLoggedUser
        {
            get
            {
                if (_secondLoggedUser == null) _secondLoggedUser = new UserManager();
                return _secondLoggedUser;
            }
        }
        #endregion

        private UserManager() { }
        private User _currentUser;

        public User CurrentUser 
        {
            get => _currentUser; 
            private set => SetProperty(ref _currentUser, value); 
        }

        #region Public Methods
        public void UserLogIn(User user)
        {
            if (user != null)
            {
                CurrentUser = user;
                return;
            }
        }

        public void UserLogOut()
        {
            if (CurrentUser != null)
            {
                CurrentUser = null;
            }
        } 
        #endregion

        #region Static Methods
        public static User GetMainLoggedInUser()
        {
            return MainLoggedUser.CurrentUser;
        }
        #endregion


    }
}
