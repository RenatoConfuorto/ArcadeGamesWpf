using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace LIB.Entities.Data.Base
{
    public class GameEnums
    {
        public enum TrisResults
        {
            defeat = 0,
            victory = 1,
            tie = 2,
        }

        public enum MemorySpDifficulty
        {
            Easy = 0,
            Medium = 1,
            Hard = 2,
            Custom = 3
        }
        public enum MemoryResult
        {
            defeat = 0,
            victory = 1,
            tie = 2,
        }
    }
}
