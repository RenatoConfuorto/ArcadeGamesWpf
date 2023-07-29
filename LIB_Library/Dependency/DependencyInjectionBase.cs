using LIB.Interfaces.Navigation;
using LIB.Navigation;
using LIB.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Practices.Unity.Configuration;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using Unity;
using Unity.Lifetime;

namespace LIB.Dependency
{
    public abstract class DependencyInjectionBase
    {
        private IUnityContainer _container;
        public IUnityContainer Container
        {
            get
            {
                if( _container == null ) _container = UnityHelper.Current.GetLocalContainer();
                return _container;
            }
        }

        //private readonly ServiceProvider _serviceProvider;
        //protected IServiceCollection services;

        //public ServiceProvider ServiceProvider
        //{
        //    get => _serviceProvider;
        //}
        public DependencyInjectionBase()
        {
            //services = new ServiceCollection();
            //services.AddSingleton<Func<Type, ViewModelBase>>(serviceProvider => viewModelType => (ViewModelBase)serviceProvider.GetRequiredService(viewModelType));
            //services.AddSingleton<INavigationService, NavigationServiceBase>();
            //InjectDependencies();
            //_serviceProvider = services.BuildServiceProvider();
        }

        public abstract void InjectDependencies();

        //protected void RegisterView<T>() where T : ViewModelBase
        //{
        //    if(ViewsManager.AddView(typeof(T).Name, typeof(T)))
        //    {
        //        services.AddSingleton<T>();
        //    }
        //}
        protected void AddDependency(Type interfaceType, Type classType)
        {
            Container.RegisterType(interfaceType, classType, new ContainerControlledLifetimeManager());
        }
        protected void AddDependency<I, C>()
        {
            AddDependency(typeof(I), typeof(C));
        }
        protected void AddDependency(Type interfaceType, Type classType, string name)
        {
            Container.RegisterType(interfaceType, classType, name, new ContainerControlledLifetimeManager());
        }
        protected void AddDependency<I, C>(string name)
        {
            AddDependency(typeof(I), typeof(C), name);
        }

        public static void InitDependencies()
        {
            IUnityContainer container = UnityHelper.Current.GetLocalContainer();
            container.RegisterType(typeof(INavigationService), typeof(NavigationServiceBase));

            List<DependencyInjectionBase> dependencies = new List<DependencyInjectionBase>();

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                Type viewDependenciesType = assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(DependencyInjectionBase))).FirstOrDefault();

                if(viewDependenciesType != null && viewDependenciesType.IsSubclassOf(typeof(DependencyInjectionBase)))
                {
                    DependencyInjectionBase viewDependencies = (DependencyInjectionBase)Activator.CreateInstance(viewDependenciesType);
                    viewDependencies.InjectDependencies();
                }
            }
        }
    }
}
