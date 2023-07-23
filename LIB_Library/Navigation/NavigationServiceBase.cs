using LIB.Base;
using LIB.Interfaces.Navigation;
using LIB.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LIB.Navigation
{
    public class NavigationServiceBase : NotifyerPropertyChangedBase, INavigationService
    {
        private ViewModelBase _viewModel;
        private readonly Func<Type, ViewModelBase> _viewModelFactory;
        public ViewModelBase CurrentView
        {
            get => _viewModel;
            private set => SetProperty(ref _viewModel, value);
        }

        public NavigationServiceBase(Func<Type, ViewModelBase> viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
        }

        public void NavigateTo<T>() where T : ViewModelBase
        {
            ViewModelBase viewModel = _viewModelFactory?.Invoke(typeof(T));
            CurrentView = viewModel;
        }
    }
}
