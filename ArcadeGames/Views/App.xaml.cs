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
using System.Threading;
using System.Diagnostics;
using Core.Logging;
using Core.Interfaces.Logging;

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
        public string LoggersConfig { get; set; }
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

            if (LoggersConfig != null)
            {
                LoggerHelper.InitLoggers(LoggersConfig);
                ILogger logger = LoggerHelper.GetSystemLogger();
                logger.LogAnonym($"-----------------------------Application Startup-----------------------------\n" +
                                 $"-----------------------------{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}-----------------------------");
            }

            NavigateToMainView();

            base.OnStartup(e);
        }
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);;

            ILogger logger = LoggerHelper.GetSystemLogger();
            logger.LogAnonym($"------------------------------Application Close------------------------------\n" +
                             $"-----------------------------{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}-----------------------------");
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
