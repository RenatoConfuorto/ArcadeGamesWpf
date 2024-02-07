using LIB.Communication.Constants;
using LIB.Helpers;
using LIB.Interfaces.Communication;
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
    public class MessageBase : IMessage
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

        #region IMessage Interface

        #region Serialize / Deserialize
        public virtual byte[] Serialize()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter br = new BinaryWriter(ms))
            {
                br.Write(MessageCode);
                //br.Write((short)MessageType);
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
                    this.SenderId = new Guid(br.ReadBytes(16)); //GUID is 16 bytes
                }
            }catch (Exception e)
            {

            }
        }
        #endregion

        #region Private Methods
        
        #endregion
        #endregion
    }
}
