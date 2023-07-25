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
        ViewModelBase ParentView { get; set; }
        void NavigateTo<T>(T parentView = null) where T : ViewModelBase;
    }
}
