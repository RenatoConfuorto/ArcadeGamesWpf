using LIB.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.Navigation
{
    public static class ViewsManager
    {
        private static Dictionary<string, Type> _views = new Dictionary<string, Type>();

        public static Dictionary<string, Type> Views
        {
            get => _views;
        }

        public static bool AddView(string name, Type model)
        {
            if (model.IsSubclassOf(typeof(ViewModelBase)))
            {
                _views.Add(name, model);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
