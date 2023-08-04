using Core.Helpers;
using LIB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.UserMng
{
    public class UserManager
    {
        private static UserManager _loggedUser;
        public static UserManager LoggedUser
        {
            get
            {
                if(_loggedUser == null) _loggedUser = new UserManager();
                return _loggedUser;
            }
        }

        private UserManager() { }

        public User CurrentUser { get; private set; }

        public void UserLogIn(User user)
        {
            if(user != null)
            {
                CurrentUser = user;
                return;
            }
        }

        public void UserLogOut()
        {
            if(CurrentUser != null)
            {
                CurrentUser = null;
            }
        }

        public User GetLoggedInUser()
        {
            return CurrentUser;
        }
    }
}
