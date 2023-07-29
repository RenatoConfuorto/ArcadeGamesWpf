﻿using LIB.Base;
using LIB.Dependency;
using LIB.Events;
using LIB.Interfaces.Navigation;
using LIB.Interfaces.ViewModels;
using LIB.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace LIB.ViewModels
{
    public abstract class ViewModelBase : NotifyerPropertyChangedBase , IViewModelBase, IDisposable
    {
        #region Navigation
        private INavigationService _navigation;
        public INavigationService Navigation
        {
            get => _navigation;
            set => SetProperty(ref _navigation, value);
        }
        public string ParentView { get; private set; }
        #endregion

        #region Events
        public event ViewChangedEvent viewChangedEvent;
        #endregion

        #region Constructor
        public ViewModelBase(string parentView = null)
        {
            ParentView = parentView;
            IUnityContainer container = UnityHelper.Current.GetLocalContainer();
            _navigation = container.Resolve<INavigationService>();
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
        protected void OnViewChanged(string viewToCall)
        {
            NavigateToView(viewToCall);
        }
        protected void NavigateToView(string viewName)
        {
            if(Navigation.CurrentView != null)
            {
                Navigation.CurrentView.viewChangedEvent -= OnViewChanged;
            }
            Navigation.NavigateTo(viewName);
            if(Navigation.CurrentView != null)
            {
                Navigation.CurrentView.viewChangedEvent += OnViewChanged;
            }
            SetCommandExecutionStatus();
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

        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Command Methods
        #endregion
    }
}
