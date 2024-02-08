﻿using LIB.Communication.Constants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using static LIB.Helpers.CommunicationHelper;
using static LIB.Communication.Constants.CommunicationCnst;
using LIB.Extensions;

namespace LIB.Entities
{
    [Serializable]
    public class OnlineUser : SerializableBase
    {

        ///set object byte length in case of modifications <length cref="ONLINE_USER_LENGTH"/>
        private char[] _userName = new char[USER_NAME_LENGTH];

        public string UserName
        {
            get => new string(_userName);
            set => SetArray(ref _userName, value, USER_NAME_LENGTH);
        }
        public Guid UserId { get; set; }

        #region Serialize / Deserialize
        public override byte[] Serialize()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter br = new BinaryWriter(ms))
            {
                br.Write(_userName);
                br.Write(UserId.ToByteArray());

                return ms.ToArray();
            }
        }

        public override void Deserialize(byte[] data)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream(data))
                using (BinaryReader br = new BinaryReader(ms))
                {
                    this.UserName   = br.ReadChars(USER_NAME_LENGTH).ToString();
                    this.UserId     = br.ReadGuid();
                }
            }
            catch (Exception e)
            {

            }
        }
        #endregion

    }
}