using LIB.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.Attributes
{
    public class ParentView : Attribute
    {
        private ViewModelBase _parent;
        public ViewModelBase Parent
        {
            get => _parent; 
            private set => _parent = value;
        }

        public ParentView(ViewModelBase parent)
        {
            Parent = parent;
        }
    }
}
