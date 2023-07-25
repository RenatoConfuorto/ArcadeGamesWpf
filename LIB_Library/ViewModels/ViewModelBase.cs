using LIB.Attributes;
using LIB.Base;
using LIB.Interfaces.Navigation;
using LIB.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
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

        #region Events
        public delegate void ViewChangedEvent(string viewToCall);
        public event ViewChangedEvent viewChangedEvent;
        #endregion

        #region Constructor
        public ViewModelBase()
        {
        }
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

        #region Protected Methods
        protected void NavigateTo<T>(T viewToCall) where T : ViewModelBase
        {
            if(Navigation.CurrentView != null)
            {
                Navigation.CurrentView.viewChangedEvent -= OnViewChanged;
            }
            Navigation.NavigateTo(typeof(T));
            if(Navigation.CurrentView != null)
            {
                Navigation.CurrentView.viewChangedEvent += OnViewChanged;
            }
            SetCommandExecutionStatus();
        }
        protected void OnViewChanged(string viewToCall)
        {
            NavigateToView(viewToCall);
        }
        protected void NavigateToView(string viewName)
        {
            if(ViewsManager.Views.ContainsKey(viewName))
            {
                Type viewType = ViewsManager.Views[viewName];
                if(viewType != null)
                {
                    if (Navigation.CurrentView != null)
                    {
                        Navigation.CurrentView.viewChangedEvent -= OnViewChanged;
                    }
                    Navigation.NavigateTo(viewType);
                    if (Navigation.CurrentView != null)
                    {
                        Navigation.CurrentView.viewChangedEvent += OnViewChanged;
                    }
                    SetCommandExecutionStatus();
                }
            }
        }
        protected void ChangeView(string viewToCall)
        {
            viewChangedEvent?.Invoke(viewToCall);
        }
        protected void NavigateToView(Type viewModelType)
        {
            if (viewModelType.IsSubclassOf(typeof(ViewModelBase)))
            {
                if(ViewsManager.Views.ContainsKey(viewModelType.Name))
                {
                    NavigateToView(viewModelType.Name);
                }
            }
        }
        #endregion

        #region Command Methods
        #endregion
    }
}
