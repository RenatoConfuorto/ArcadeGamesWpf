using LIB.Base;
using LIB.Interfaces.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.ViewModels
{
    public abstract class ViewModelBase : NotifyerPropertyChangedBase
    {
        #region Navigation
        private INavigationService _navigation;
        public INavigationService Navigation
        {
            get => _navigation;
            set => SetProperty(ref _navigation, value);
        }
        #endregion

        #region Constructor
        public ViewModelBase(INavigationService navService)
        {
            Navigation = navService;
            OnInitialized();
        }
        #endregion

        #region Commands
        #endregion

        #region Virtual Methods
        protected virtual void OnInitialized()
        {
            InitCommands();
            SetCommandExecutionStatus();
        }
        protected virtual void InitCommands()
        {

        }
        protected virtual void SetCommandExecutionStatus()
        {

        }
        #endregion

        #region Command Methods
        #endregion
    }
}
