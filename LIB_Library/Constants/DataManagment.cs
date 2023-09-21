using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.Constants
{
    public class DataManagment
    {
        public const string GAME_DATA_TABLE_COMMON = @"GAME_ID INT PK,
	GAME_GUID VARCHAR(40) NOT NULL,
	USER_NAME VARCHAR(255) NOT NULL,
	GAME_DATE DATETIME NOT NULL";
    }
}
