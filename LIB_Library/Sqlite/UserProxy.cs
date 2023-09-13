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
            return $@"CREATE TABLE IF NOT EXISTS TRIS_SP
                        (
	                        {DataManagment.GAME_DATA_TABLE_COMMON},
	                        HAS_WON BOOL NOT NULL
                        );
                        CREATE TABLE IF NOT EXISTS TRIS_MP
                        (
	                        {DataManagment.GAME_DATA_TABLE_COMMON},
	                        HAS_WON BOOL NOT NULL,
	                        OPPONENT_NAME VARCHAR(255) NULL
                        );";
        }
    }
}
