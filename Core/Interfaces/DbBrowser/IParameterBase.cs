using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.DbBrowser
{
    public interface IParameterBase
    {
        object this[string key] { get; }
        void Add(string key, object value);
        void RemoveAt(int index);
        void Remove(string key);
        void Clear();
    }
}
