using ArcadeGames.ViewModels;
using ArcadeGames.Views;
using LIB.Constants;
using Core.Dependency;
using Core.Interfaces.ViewModels;
using Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Unity;
using Core.Helpers;

namespace ArcadeGames.Views
{
    /// <summary>
    /// Logica di interazione per App.xaml
    /// </summary>
    public partial class App : Application
    {
        private SplashScreen splashScreen;

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
        public SplashScreen SplashScreen
        {
            set { splashScreen = value; }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            if (splashScreen != null) splashScreen.Show(autoClose: true);
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
