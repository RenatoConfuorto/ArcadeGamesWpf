using Core.Entities;
using Core.Helpers;
using LIB.Constants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LIB.Constants.DefaultSettings;

namespace LIB.Settings
{
    public class GlobalSettings
    {
        private static int _globalVolume;
        public static int GlobalVolume
        {
            get => _globalVolume;
            set => UpdateSetting(ref _globalVolume, value);
        }

        private static void UpdateSetting<T>(ref T _field, T value)
        {
            _field = value;
            ApplicationSettings settings = new ApplicationSettings()
            {
                GlobalVolume = _globalVolume,
            };
            XmlSerializerBase.SerializeObject(Cnst.ApplicationSettingsFilePath, settings);
        }

        public static void LoadGlobalSettings()
        {
            ApplicationSettings settings;
            if (File.Exists(Cnst.ApplicationSettingsFilePath))
            {
                settings = (ApplicationSettings)XmlSerializerBase.DeserializeObject(Cnst.ApplicationSettingsFilePath, typeof(ApplicationSettings));
            }
            else
            {
                //creare le impostazioni di base
                settings = new ApplicationSettings()
                {
                    GlobalVolume = DEFAULT_VOLUME_LEVEL
                };
                XmlSerializerBase.SerializeObject(Cnst.ApplicationSettingsFilePath, settings);
            }
            //caricare le impostazioni
            GlobalVolume = settings.GlobalVolume;
        }
    }
}
