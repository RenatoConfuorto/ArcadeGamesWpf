﻿using Core.Entities;
using Core.Helpers;
using LIB.Entities;
using LIB.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

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
                user.LastConnected = DateTime.Now;
                CurrentUser = user;
                string errorMessage;
                UserHelper.UpdateUser(user, out errorMessage);
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
        /// <summary>
        /// check if the given user is logged in
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns>0 => the user is not logged int; 1 => the user is logged in as main user; 2 => the user is logged in as second user</returns>
        public static int IsUserLoggedIn(User user) => IsUserLoggedIn(user?.Name);
        /// <summary>
        /// check if the given user is logged in
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns>0 => the user is not logged int; 1 => the user is logged in as main user; 2 => the user is logged in as second user</returns>
        public static int IsUserLoggedIn(string UserName)
        {
            if(String.IsNullOrEmpty(UserName)) return 0;
            if (MainLoggedUser.CurrentUser?.Name == UserName) return 1;
            else if (SecondLoggedUser.CurrentUser?.Name == UserName) return 2;
            else return 0;
        }
        #endregion


    }
}