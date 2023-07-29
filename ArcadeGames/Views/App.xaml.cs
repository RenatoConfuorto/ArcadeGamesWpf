using ArcadeGames.ViewModels;
using ArcadeGames.Views;
using LIB.Constants;
using LIB.Dependency;
using LIB.Interfaces.ViewModels;
using LIB.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Unity;

namespace ArcadeGames
{
    /// <summary>
    /// Logica di interazione per App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IUnityContainer _container;
        public IUnityContainer Container
        {
            get
            {
                if(_container == null )_container = UnityHelper.Current.GetLocalContainer();
                return _container;
            }
        }
        public App()
        {
            
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            ViewDependencies dependencies = new ViewDependencies();
            FrameworkElement.StyleProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata
            {
                DefaultValue = FindResource(typeof(Window))
            });
            DependencyInjectionBase.InitDependencies();
            NavigateToMainView();

            base.OnStartup(e);
        }

        public void NavigateToMainView()
        {
            IUnityContainer container = UnityHelper.Current.GetLocalContainer();
            MainWindow mainWindow = new MainWindow();
            mainWindow.DataContext = container.Resolve<IViewModelBase>(ViewNames.MainWindow);
            mainWindow.Show();

        }
    }
}
