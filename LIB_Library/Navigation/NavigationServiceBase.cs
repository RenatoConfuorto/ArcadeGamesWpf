﻿using Core.Entities;
using Core.Dependency;
using Core.Interfaces.Navigation;
using Core.Interfaces.ViewModels;
using Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Unity;
using System.Windows.Controls;
using Core.Attributes;
using System.Windows;

namespace LIB.Navigation
{
    public class NavigationServiceBase : NotifyerPropertyChangedBase, INavigationService
    {
        private IUnityContainer _container;
        private IViewModelBase _viewModel;
        private UserControl _currnetControl;
        private string _parentViewName;

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
        public UserControl CurrentControl
        {
            get => _currnetControl;
            set => SetProperty(ref _currnetControl, value);
        }
        public string ParentViewName
        {
            get => _parentViewName;
            private set => SetProperty(ref _parentViewName, value);
        }

        public NavigationServiceBase()
        {
        }
        public void NavigateTo(string ViewName)
        {
            if(ViewName != null)
            {
                if(CurrentView != null) CurrentView.Dispose();
                CurrentView = Container.Resolve<IViewModelBase>(ViewName);
                if (CurrentView != null && CurrentView.IsDisposed)
                {
                    CurrentView.InitViewModel();
                }
                SetContentView();
                SetParentView();
            }

        }
        private void SetParentView()
        {
            string parent = null;
            if(CurrentView != null)
            {
                parent = CurrentView.ParentView;
            }
            ParentViewName = parent;
        }

        private void SetContentView()
        {
            if(CurrentView != null)
            {
                ViewRef viewRefAttribute = CurrentView.GetType().GetCustomAttribute<ViewRef>();
                if(viewRefAttribute != null)
                {
                    CurrentControl = (UserControl)Activator.CreateInstance(viewRefAttribute.ViewType);
                }
                else
                {
                    MessageBox.Show($"Nessuna view impostata per il viewModel {CurrentView.GetType().FullName}", "View Mancante", MessageBoxButton.OK, MessageBoxImage.Stop);
                }
            }
        }
    }
}
