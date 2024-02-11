using Core.Dependency;
using Core.Entities;
using Core.Events;
using Core.Interfaces.Navigation;
using Core.Interfaces.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Unity;

namespace Core.ViewModels
{
    public abstract class ViewModelBase : NotifyerPropertyChangedBase , IViewModelBase, IDisposable
    {
        #region Navigation
        IUnityContainer container;
        private INavigationService _navigation;
        public INavigationService Navigation
        {
            get => _navigation;
            set => SetProperty(ref _navigation, value);
        }
        public string ParentView { get; private set; }
        #endregion

        #region Dispatcher
        public Dispatcher dispatcher { get; private set; } = Application.Current.Dispatcher;
        #endregion

        #region IsDisposed
        private bool _isDisposed = false;

        public bool IsDisposed { get => _isDisposed; }
        #endregion
        #region View Param
        private object _viewParam;
        public object ViewParam { get => _viewParam; }
        #endregion

        #region Events
        public event ViewChangedEvent viewChangedEvent;
        #endregion
        #region Public Properties
        public string ViewName { get; private set; }
        #endregion

        #region Constructor
        public ViewModelBase(string viewName, string parentView = null, object param = null)
        {
            ViewName = viewName;
            ParentView = parentView;
            _viewParam = param;
            container = UnityHelper.Current.GetLocalContainer();
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
            GetViewParameter();
        }
        protected virtual void InitCommands()
        {

        }
        protected virtual void SetCommandExecutionStatus()
        {

        }
        #endregion

        #region Protected Methods
        protected void OnViewChanged(string viewToCall, object viewParam)
        {
            NavigateToView(viewToCall, viewParam);
        }
        protected void NavigateToView(string viewName, object param = null)
        {
            if(Navigation.CurrentView != null)
            {
                Navigation.CurrentView.viewChangedEvent -= OnViewChanged;
            }
            Navigation.NavigateTo(viewName, param);
            if(Navigation.CurrentView != null)
            {
                Navigation.CurrentView.viewChangedEvent += OnViewChanged;
            }
            SetCommandExecutionStatus();
        }
        protected void ChangeView(string viewToCall, object viewParam = null)
        {
            viewChangedEvent?.Invoke(viewToCall, viewParam);
        }
        protected virtual void GetViewParameter()
        {
        }

        public virtual void Dispose()
        {
            _isDisposed = true;
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Public Methods
        public void InitViewModel()
        {
            OnInitialized();
        }
        #endregion

        #region Command Methods
        #endregion
    }
}
