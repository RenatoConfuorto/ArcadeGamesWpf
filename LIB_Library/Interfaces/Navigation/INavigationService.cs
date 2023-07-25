using LIB.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.Interfaces.Navigation
{
    public interface INavigationService
    {
        ViewModelBase CurrentView { get; }
        Type ParentView { get; }
        void NavigateTo<T>() where T : ViewModelBase;
        void NavigateTo(Type viewModelType);
    }
}
