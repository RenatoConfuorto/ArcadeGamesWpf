using Core.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB_Com.Interfaces.Communication
{
    public interface IMessage : ISerializableBase
    {
        int MessageCode { get; }
        Guid SenderId { get; }
        //MessageType MessageType { get; }

        //int GetSize();
        //byte[] Serialize();
        //void Deserialize(byte[] data);
    }
}
