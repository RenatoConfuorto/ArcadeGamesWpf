using LIB.Dependency;
using LIB.Interfaces.ViewModels;
using LIB.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace LIB.Interfaces.Navigation
{
    public interface INavigationService
    {
        //ViewModelBase CurrentView { get; }
        //Type ParentView { get; }
        //void NavigateTo<T>() where T : ViewModelBase;
        //void NavigateTo(Type viewModelType);
        IUnityContainer Container { get; }
        IViewModelBase CurrentView { get; }
        string ParentViewName { get; }

        void NavigateTo(string ViewName);
    }
}
