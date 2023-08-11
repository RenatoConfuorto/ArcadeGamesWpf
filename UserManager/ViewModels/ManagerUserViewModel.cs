using Core.Attributes;
using LIB.Constants;
using LIB.Entities;
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
        #endregion

        #region Private Methods
        #endregion

        #region Protected Methods
        #endregion
    }
}
