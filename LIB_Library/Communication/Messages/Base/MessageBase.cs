using LIB.Communication.Constants;
using LIB.Entities;
using LIB.Extensions;
using LIB.Helpers;
using LIB.Interfaces.Communication;
using LIB.Interfaces.Entities;
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
    public class MessageBase : SerializableBase , IMessage
    {
        public int MessageCode { get; set; }
        public Guid SenderId { get; set; }
        //public MessageType MessageType { get; set; }

        public MessageBase() { }
        public MessageBase(int messageCode, Guid senderId)
            :this()
        {
            this.MessageCode = messageCode;
            this.SenderId = senderId;
            //this.MessageType = messageType;
        }

        public MessageBase(CommunicationCnst.Messages messageCode, Guid senderId)
            :this((int)messageCode, senderId)
        {
        }

        protected int GetBaseMembersSize(out int size)
        {
            size  = sizeof(int);
            size += GUID_LENGTH;
            return size;
        }

        #region Serialize / Deserialize
        public override byte[] Serialize()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                bw.Write(MessageCode);
                //bw.Write((short)MessageType);
                bw.Write(SenderId.ToByteArray());
                SerializeData(bw);
                return ms.ToArray();
            }
        }

        public virtual void SerializeData(BinaryWriter bw)
        {

        }

        public override void Deserialize(byte[] data)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream(data))
                using (BinaryReader br = new BinaryReader(ms))
                {
                    this.MessageCode    = br.ReadInt32();
                    this.SenderId       = br.ReadGuid();
                    DeserializeData(br);
                }
            }
            catch (Exception e)
            {

            }
        }

        public virtual void DeserializeData(BinaryReader br)
        {

        }
        #endregion

        #region Private Methods

        #endregion
    }
}
