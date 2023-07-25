using ArcadeGames.ViewModels;
using ArcadeGames.Views;
using LIB.Dependency;
using LIB.Interfaces.Navigation;
using LIB.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcadeGames
{
    public class ViewDependencies : DependencyInjectionBase
    {
        public override void InjectDependencies()
        {
            services.AddSingleton<MainWindow>(provider => new MainWindow()
            {
                DataContext = provider.GetRequiredService< MainWindowViewModel>()
            });
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<HomeViewModel>();
        }
    }
}
