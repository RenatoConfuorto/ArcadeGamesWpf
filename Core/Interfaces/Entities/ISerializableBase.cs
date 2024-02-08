using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Entities
{
    public interface ISerializableBase
    {
        int GetSize();
        byte[] Serialize();
        void Deserialize(byte[] data);
    }
}
