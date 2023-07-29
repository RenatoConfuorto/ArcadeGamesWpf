using LIB.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.Interfaces.ViewModels
{
    public interface IViewModelBase : IDisposable
    {
        string ParentView { get; }
        event ViewChangedEvent viewChangedEvent;
    }
}
