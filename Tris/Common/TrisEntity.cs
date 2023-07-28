using LIB.Base;
using LIB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tris.Common
{
    public class TrisEntity : CellEntityBase
    {
        private string _text = String.Empty;

        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }
    }
}
