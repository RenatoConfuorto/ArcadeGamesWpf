using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.DbBrowser
{
    public interface IProxyBase
    {
        void OpenConnection();
        void CloseConnection();
        int GetNextIntValue(string TableName, string FieldName);
    }
}
