using Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.ViewModels
{
    public interface IViewModelBase : IDisposable
    {
        string ViewName { get; }
        string ParentView { get; }
        event ViewChangedEvent viewChangedEvent;
    }
}
