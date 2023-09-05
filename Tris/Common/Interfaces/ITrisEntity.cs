using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tris.Common.Interfaces
{
    public interface ITrisEntity
    {
        int CellId { get; }
        string Text { get; }
    }
}
