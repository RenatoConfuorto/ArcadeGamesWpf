using LIB_Com.Constants;
using LIB_Com.Messages.Base;
using LIB_Com.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB_Com.Messages
{
    public class SendDataConfirmation : MessageBase
    {
        public Guid UserId { get; set; }
        public int UserSeq { get; set; }
        public SendDataConfirmation() { }
        public SendDataConfirmation(Guid userId, int userSeq)
            : base(CommunicationCnst.Messages.SendDataConfirmation, new Guid()/*, CommunicationCnst.MessageType.HostToCliet*/)
        {
            this.UserId     = userId;
            this.UserSeq    = userSeq;
        }

        #region Serialize / Deserialize
        public override void SerializeData(BinaryWriter bw)
        {
            base.SerializeData(bw);
            bw.Write(UserId.ToByteArray());
            bw.Write(UserSeq);
        }


        public override void DeserializeData(BinaryReader br)
        {
            base.DeserializeData(br);
            this.UserId     = br.ReadGuid();
            this.UserSeq    = br.ReadInt32();
        }
        #endregion
    }
}
