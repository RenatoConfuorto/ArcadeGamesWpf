using LIB.Constants;
using Core.Dependency;
using Core.Interfaces.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tris.ViewModels;

namespace Tris
{
    public class ModuleDependencies : DependencyInjectionBase
    {
        public override void InjectDependencies()
        {
            AddDependency<IViewModelBase, TrisHomePageViewModel>(ViewNames.TrisHomePage);
            AddDependency<IViewModelBase, TrisMultiplayerViewModel>(ViewNames.TrisMultiplayer);
            AddDependency<IViewModelBase, TrisSingleplayerViewModel>(ViewNames.TrisSingleplayer);
        }
    }
}
