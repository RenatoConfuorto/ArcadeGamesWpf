using Core.Dependency;
using Core.Interfaces.DbBrowser;
using Core.Interfaces.Navigation;
using LIB.Navigation;
using LIB.Sqlite.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB
{
    public class ApplicationDependencies : DependencyInjectionBase
    {
        public override void InjectDependencies()
        {
            AddDependency<INavigationService, NavigationServiceBase>();
        }
    }
}
