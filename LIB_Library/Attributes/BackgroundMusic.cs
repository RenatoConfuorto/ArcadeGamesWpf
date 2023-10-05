using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.Attributes
{
    public class BackgroundMusic : Attribute
    {
        private string _backgroundMusicName;

        public string BackgroundMusicName
        {
            get => _backgroundMusicName;
            set => _backgroundMusicName = value;
        }

        public BackgroundMusic(string backgroundMusinName) 
        { 
            BackgroundMusicName = backgroundMusinName;
        }
    }
}
