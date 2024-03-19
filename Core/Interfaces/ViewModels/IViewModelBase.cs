using Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Core.Interfaces.ViewModels
{
    public interface IViewModelBase : IDisposable
    {
        string ViewName { get; }
        string ParentView { get; }
        object ViewParam { get; }
        bool IsDisposed { get; }
        Dispatcher _dispatcher { get; }
        event ViewChangedEvent viewChangedEvent;
        void InitViewModel();
    }
}
