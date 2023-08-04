using LIB.Constants;
using Core.Dependency;
using Core.Interfaces.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManager.ViewModels;

namespace UserManager
{
    public class ModuleDependencies : DependencyInjectionBase
    {
        public override void InjectDependencies()
        {
            AddDependency<IViewModelBase, UserMngMainPageViewModel>(ViewNames.UserMngMainPage);
            AddDependency<IViewModelBase, NewUserViewModel>(ViewNames.NewUserPage);
        }
    }
}
