using ArcadeGames.ViewModels;
using ArcadeGames.Views;
using LIB.Dependency;
using LIB.Constants;
using LIB.Interfaces.Navigation;
using LIB.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tris.ViewModels;

namespace ArcadeGames
{
    public class ViewDependencies : DependencyInjectionBase
    {
        public override void InjectDependencies()
        {
            RegisterView<MainWindowViewModel>();
            services.AddSingleton<MainWindow>(provider => new MainWindow()
            {
                DataContext = provider.GetRequiredService<MainWindowViewModel>()
            });
            RegisterView<HomeViewModel>();
            RegisterView<TrisHomePageViewModel>();

        }
    }
}
