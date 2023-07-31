using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Core.Attributes
{
    public class ViewRef : Attribute
    {
        private string _viewName;
        private Type _viewType;

        public string ViewName
        {
            get => _viewName;
            set => _viewName = value;
        }
        public Type ViewType
        {
            get => _viewType;
            set => _viewType = value;
        }

        public ViewRef(Type viewType) : this(viewType, String.Empty) { }

        public ViewRef(Type viewType, string viewName)
        {
            _viewName = viewName;
            _viewType = viewType;
        }
    }
}
