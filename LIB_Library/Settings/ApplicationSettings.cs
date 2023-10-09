using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.Settings
{
    public class ApplicationSettings : NotifyerPropertyChangedBase
    {
        private int _globalVolume;

        public int GlobalVolume
        {
            get => _globalVolume;
            set => SetProperty(ref _globalVolume, value);
        }
    }
}
