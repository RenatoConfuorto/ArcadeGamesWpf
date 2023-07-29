using ArcadeGames.ViewModels;
using ArcadeGames.Views;
using LIB.Dependency;
using LIB.Constants;
using LIB.Interfaces.Navigation;
using LIB.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tris.ViewModels;
using LIB.Interfaces.ViewModels;

namespace ArcadeGames
{
    public class ViewDependencies : DependencyInjectionBase
    {
        public override void InjectDependencies()
        {
            AddDependency<IViewModelBase, MainWindowViewModel>(ViewNames.MainWindow);
            AddDependency<IViewModelBase, HomeViewModel>(ViewNames.Home);
        }
    }
}
