using LIB.Base;
using LIB.Dependency;
using LIB.Interfaces.Navigation;
using LIB.Interfaces.ViewModels;
using LIB.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace LIB.Navigation
{
    public class NavigationServiceBase : NotifyerPropertyChangedBase, INavigationService
    {
        private IUnityContainer _container;
        private IViewModelBase _viewModel;
        private string _parentViewName;

        //private readonly Func<Type, ViewModelBase> _viewModelFactory;
        public IUnityContainer Container
        {
            get
            {
                if (_container == null) _container = UnityHelper.Current.GetLocalContainer();
                return _container;
            }
        }
        public IViewModelBase CurrentView
        {
            get => _viewModel;
            private set => SetProperty(ref _viewModel, value);
        }
        public string ParentViewName
        {
            get => _parentViewName;
            private set => SetProperty(ref _parentViewName, value);
        }

        public NavigationServiceBase()
        {
        }

        //public void NavigateTo<T>() where T : ViewModelBase 
        //{
        //    ViewModelBase viewModel = _viewModelFactory?.Invoke(typeof(T));
        //    CurrentView = viewModel;
        //    SetParentView();
        //}

        //public void NavigateTo(Type viewModelType)
        //{
        //    if (viewModelType.IsSubclassOf(typeof(ViewModelBase)))
        //    {
        //        ViewModelBase viewModel = _viewModelFactory?.Invoke(viewModelType);
        //        CurrentView = viewModel;
        //        SetParentView();
        //    }
        //}
        public void NavigateTo(string ViewName)
        {
            if(ViewName != null)
            {
                if(CurrentView != null) CurrentView.Dispose();
                CurrentView = Container.Resolve<IViewModelBase>(ViewName);
                SetParentView();
            }

        }
        private void SetParentView()
        {
            string parent = null;
            //Attributes.ParentView attribute = CurrentView.GetType().GetCustomAttribute<Attributes.ParentView>();
            //if(attribute != null)
            //{
            //    parent = attribute.ParentName;
            //}
            //ParentViewName = parent;
            if(CurrentView != null)
            {
                parent = CurrentView.ParentView;
            }
            ParentViewName = parent;
        }
    }
}
