using LIB.Constants;
using LIB.Sqlite.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.Sqlite
{
    public class UserProxy : ProxyBase
    {
        public UserProxy(string UserName) 
            : base(String.Format(Cnst.UserGameDataLocation, UserName))
        {
        }

        public override string GetCreateTableIfExistsStatement()
        {
            return "";
        }
    }
}
