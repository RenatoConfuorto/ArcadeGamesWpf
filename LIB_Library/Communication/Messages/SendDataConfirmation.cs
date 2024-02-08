﻿using LIB.Communication.Constants;
using LIB.Communication.Messages.Base;
using LIB.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.Communication.Messages
{
    public class SendDataConfirmation : MessageBase
    {
        public Guid UserId { get; set; }
        public SendDataConfirmation() { }
        public SendDataConfirmation(Guid userId)
            : base(CommunicationCnst.Messages.SendDataConfirmation, new Guid()/*, CommunicationCnst.MessageType.HostToCliet*/)
        {
            this.UserId = userId;
        }

        #region Serialize / Deserialize
        public override void SerializeData(BinaryWriter bw)
        {
            base.SerializeData(bw);
            bw.Write(UserId.ToByteArray());
        }


        public override void DeserializeData(BinaryReader br)
        {
            base.DeserializeData(br);
            this.UserId = br.ReadGuid();
        }
        #endregion
    }
}
