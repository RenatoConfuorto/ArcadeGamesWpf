using Core.Attributes;
using Core.Helpers;
using Core.Interfaces.Navigation;
using Core.Interfaces.ViewModels;
using Core.Navigation;
using Core.ViewModels;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Unity;
using Unity.Lifetime;

namespace Core.Dependency
{
    public abstract class DependencyInjectionBase
    {
        private IUnityContainer _container;
        public IUnityContainer Container
        {
            get
            {
                if (_container == null) _container = UnityHelper.Current.GetLocalContainer();
                return _container;
            }
        }
        public DependencyInjectionBase()
        {
        }

        public abstract void InjectDependencies();

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
        /// <summary>
        /// Init all the dependencies defined in the application modules
        /// </summary>
        public static void InitDependencies()
        {
            IUnityContainer container = UnityHelper.Current.GetLocalContainer();
            container.RegisterType(typeof(INavigationService), typeof(NavigationServiceBase));

            Assembly[] assemblies = AssemblyHelper.LoadApplicationAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                Type viewDependenciesType = assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(DependencyInjectionBase))).FirstOrDefault();

                if (viewDependenciesType != null && viewDependenciesType.IsSubclassOf(typeof(DependencyInjectionBase)))
                {
                    DependencyInjectionBase viewDependencies = (DependencyInjectionBase)Activator.CreateInstance(viewDependenciesType);
                    viewDependencies.InjectDependencies();
                }
            }

            InitViewTemplates();
        }
        /// <summary>
        /// Create The templates for the views from the registerd viewModels
        /// </summary>
        public static void InitViewTemplates()
        {
            //get view Types
            List<Type> viewTypes = GetTypesRegistrations<IViewModelBase>();

            if (viewTypes?.Count == 0)
            {
                MessageBox.Show($"Nessuna view registrata", "InitViewTemplates", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }
            else
            {
                foreach (Type viewModel in viewTypes)
                {
                    try
                    {
                        ViewRef viewRefAtt = viewModel.GetCustomAttribute<ViewRef>();
                        if (viewRefAtt == null)
                        {
                            MessageBox.Show($"Nessun Attributo di tipo {typeof(ViewRef).FullName} trovato per il view model {viewModel.GetType().FullName}",
                                "InitViewTemplates", MessageBoxButton.OK, MessageBoxImage.Stop);
                            continue;
                        }
                        DataTemplate viewTemplate = new DataTemplate { DataType = viewModel.GetType() };
                        FrameworkElementFactory homeViewFactory = new FrameworkElementFactory(viewRefAtt.ViewType);
                        viewTemplate.VisualTree = homeViewFactory;

                        Application.Current.Resources.Add(new DataTemplateKey(viewModel), viewTemplate);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "InitViewTemplates", MessageBoxButton.OK, MessageBoxImage.Stop);
                    }
                }
            }
        }
        /// <summary>
        /// Gets all the type mapped to the given type
        /// </summary>
        /// <param name="registeredType"></param>
        /// <returns></returns>
        public static List<Type> GetTypesRegistrations(Type registeredType)
        {
            IUnityContainer container = UnityHelper.Current.GetLocalContainer();
            List<Type> result = container.Registrations.Where(r => r.RegisteredType == registeredType).Select(s => s.MappedToType).ToList();
            return result;
        }
        /// <summary>
        /// Gets all the type mapped to the given type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<Type> GetTypesRegistrations<T>()
        {
            return GetTypesRegistrations(typeof(T));
        }
    }
}
