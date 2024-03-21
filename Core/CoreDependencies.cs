using Core.Dependency;
using Core.Interfaces.Logging;
using Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    internal class CoreDependencies : DependencyInjectionBase
    {
        public override void InjectDependencies()
        {
            AddDependency<ILogger, Logger>();
        }
    }
}
