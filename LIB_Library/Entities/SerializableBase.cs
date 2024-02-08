using LIB.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.Entities
{
    public abstract class SerializableBase : ISerializableBase
    {
        public int GetSize()
        {
            return this.Serialize().Length;
        }
        public abstract byte[] Serialize();
        public abstract void Deserialize(byte[] data);
    }
}
