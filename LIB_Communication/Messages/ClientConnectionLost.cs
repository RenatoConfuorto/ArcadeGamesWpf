﻿using LIB_Com.Constants;
using LIB_Com.Extensions;
using LIB_Com.Messages.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB_Com.Messages
{
    public class ClientConnectionLost : MessageBase
    {
        public Guid UserId { get; set; }

        public ClientConnectionLost()
            :base(CommunicationCnst.Messages.ClientConnectionLost, new Guid())
        { 
        }

        public ClientConnectionLost(Guid userId)
            :this()
        {
            this.UserId = userId;
        }

        #region Serialize/Deserialize
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