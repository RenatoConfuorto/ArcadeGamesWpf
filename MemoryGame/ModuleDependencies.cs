using Core.Dependency;
using Core.Interfaces.ViewModels;
using LIB.Constants;
using MemoryGame.ViewModels;
using MemoryGame.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryGame
{
    public class ModuleDependencies : DependencyInjectionBase
    {
        public override void InjectDependencies()
        {
            AddDependency<IViewModelBase, MemoryHomePageViewModel>(ViewNames.MemoryGameHomePage);
            AddDependency<IViewModelBase, MemorySingleplayerViewModel>(ViewNames.MemorySingleplayer);
            AddDependency<IViewModelBase, MemorySingleplayerSettingsViewModel>(ViewNames.MemorySingleplayerSettings);
            AddDependency<IViewModelBase, MemoryMultiplayerViewModel>(ViewNames.MemoryMultiplayer);
        }
    }
}
