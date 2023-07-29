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
        private string _parentName;
        public string ParentName
        {
            get => _parentName; 
            private set => _parentName = value;
        }

        public ParentView(string parentName)
        {
            ParentName = parentName;
        }
    }
}
