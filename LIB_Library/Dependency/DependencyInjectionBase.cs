using LIB.Interfaces.Navigation;
using LIB.Navigation;
using LIB.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace LIB.Dependency
{
    public abstract class DependencyInjectionBase
    {
        private readonly ServiceProvider _serviceProvider;
        protected IServiceCollection services;

        public ServiceProvider ServiceProvider
        {
            get => _serviceProvider;
        }
        public DependencyInjectionBase()
        {
            services = new ServiceCollection();
            services.AddSingleton<Func<Type, ViewModelBase>>(serviceProvider => viewModelType => (ViewModelBase)serviceProvider.GetRequiredService(viewModelType));
            services.AddSingleton<INavigationService, NavigationServiceBase>();
            InjectDependencies();
            _serviceProvider = services.BuildServiceProvider();
        }

        public abstract void InjectDependencies();

        protected void RegisterView<T>() where T : ViewModelBase
        {
            if(ViewsManager.AddView(typeof(T).Name, typeof(T)))
            {
                services.AddSingleton<T>();
            }
        }
    }
}
