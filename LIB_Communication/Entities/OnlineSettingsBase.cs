using Core.Entities;
using Core.Helpers;
using LIB.Entities;
using LIB_Com.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LIB_Com.Constants.CommunicationCnst;
using static LIB.Helpers.CommunicationHelper;
using static LIB_Com.Extensions.BinaryReaderExtensions;

namespace LIB_Com.Entities
{
    public class OnlineSettingsBase : GameSettingsBase, IOnlineSettingsBase
    {
        private int _playersTime;
        public int PlayersTime
        {
            get => _playersTime;
            set => SetProperty(ref _playersTime, value);
        }

        //private char[] _settingsPopupName = new char[ONLINE_SETTINGS_POPUP_NAME_LEN];
        //public string SettingsPopupName
        //{
        //    get => new string(_settingsPopupName).Trim();
        //    set => SetString(ref _settingsPopupName, value, ONLINE_SETTINGS_POPUP_NAME_LEN);
        //}

        #region ISerializableBase
        public byte[] Serialize()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                bw.Write(PlayersTime);
                //bw.Write(_settingsPopupName);
                SerializeData(bw);
                return ms.ToArray();
            }
        }

        public virtual void SerializeData(BinaryWriter bw)
        {

        }

        public void Deserialize(byte[] data)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream(data))
                using (BinaryReader br = new BinaryReader(ms))
                {
                    this.PlayersTime = br.ReadInt32();
                    //this.SettingsPopupName = br.ReadString(ONLINE_SETTINGS_POPUP_NAME_LEN);
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

        public int GetSize()
        {
            return this.Serialize().Length;
        }
        #endregion
    }
}
