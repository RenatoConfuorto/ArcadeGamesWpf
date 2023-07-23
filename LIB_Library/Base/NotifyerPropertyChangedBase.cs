using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LIB.Base
{
    public class NotifyerPropertyChangedBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged([CallerMemberName] string propertyChanged = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyChanged));
        }

        public void SetProperty<T>(ref T field, T value, [CallerMemberName]string propertyName = null)
        {
            field = value;
            NotifyPropertyChanged(propertyName);
        }
    }
}
