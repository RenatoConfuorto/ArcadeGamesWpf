using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Unity;

namespace LIB.Dependency
{
    public class UnityHelper
    {
        private static UnityHelper _current;
        private static IUnityContainer _container;

        public static UnityHelper Current
        {
            get
            {
                if( _current == null) return new UnityHelper();
                return _current;
            }
        }

        private UnityHelper() { }

        public void SetLocalContainer(IUnityContainer cont)
        {
            try
            {
                if (cont != null)
                {
                    _container = cont;
                    return;
                }

                throw new NullReferenceException("null parameter");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IUnityContainer GetLocalContainer()
        {
            if (_container == null)
            {
                _container = new UnityContainer();
            }

            return _container;
        }
    }
}
