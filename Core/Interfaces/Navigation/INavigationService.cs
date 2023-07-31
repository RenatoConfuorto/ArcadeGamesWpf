using Core.Dependency;
using Core.Interfaces.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Unity;

namespace Core.Interfaces.Navigation
{
    public interface INavigationService
    {
        //ViewModelBase CurrentView { get; }
        //Type ParentView { get; }
        //void NavigateTo<T>() where T : ViewModelBase;
        //void NavigateTo(Type viewModelType);
        IUnityContainer Container { get; }
        IViewModelBase CurrentView { get; }
        UserControl CurrentControl { get; }
        string ParentViewName { get; }

        void NavigateTo(string ViewName);
    }
}
