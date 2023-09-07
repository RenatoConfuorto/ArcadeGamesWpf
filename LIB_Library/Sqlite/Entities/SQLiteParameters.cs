using Core.Interfaces.DbBrowser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.Sqlite.Entities
{
    public class SQLiteParameters : IParameterBase
    {
        private Dictionary<string, object> parameters;

        public SQLiteParameters()
            : base()
        {
        }

        public object this[string key]
        {
            get => parameters[key];
        }
        public void Add(string key, object value)
        {
            parameters.Add(key, value);
        }

        public void RemoveAt(int index)
        {
            if (index < parameters.Count - 1) throw new ArgumentOutOfRangeException("index");
            parameters.Remove(parameters.ElementAt(index).Key);
        }
        public void Remove(string key)
        {
            if (parameters.ContainsKey(key)) parameters.Remove(key);
        }

        public void Clear()
        {
            parameters.Clear();
        }
    }
}
