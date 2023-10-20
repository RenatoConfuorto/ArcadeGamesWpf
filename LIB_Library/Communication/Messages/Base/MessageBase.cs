using LIB.Communication.Constants;
using LIB.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using Unity.Policy;
using static LIB.Communication.Constants.CommunicationCnst;

namespace LIB.Communication.Messages.Base
{
    [Serializable]
    public class MessageBase
    {
        public Guid SenderId { get; set; }
        public int MessageCode { get; set; }
        public MessageType MessageType { get; set; }
        public MessageBase() { }
        public MessageBase(int messageCode, Guid senderId, MessageType messageType)
        {
            this.MessageCode = messageCode;
            this.SenderId = senderId;
            this.MessageType = messageType;
        }

        public MessageBase(CommunicationCnst.Messages messageCode, Guid senderId, MessageType messageType)
            :this((int)messageCode, senderId, messageType)
        {
        }

        #region Serialize / Deserialize
        public virtual byte[] Serialize()
        {
            using(MemoryStream ms = new MemoryStream())
            using(BinaryWriter br = new BinaryWriter(ms))
            {
                br.Write(MessageCode);
                br.Write((short)MessageType);
                br.Write(SenderId.ToByteArray());

                return ms.ToArray();
            }
        }

        public virtual void Deserialize(byte[] data)
        {
            try
            {
                using(MemoryStream ms = new MemoryStream(data))
                using(BinaryReader br = new BinaryReader(ms))
                {
                    this.MessageCode = br.ReadInt32();
                    this.MessageType = (CommunicationCnst.MessageType)br.ReadInt16();
                    this.SenderId = new Guid(br.ReadBytes(16)); //GUID is 16 bytes
                }
            }catch (Exception e)
            {

            }
        }
        #endregion
    }
}
