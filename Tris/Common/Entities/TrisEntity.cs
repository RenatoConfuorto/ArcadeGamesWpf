using LIB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tris.Common.Interfaces;

namespace Tris.Common.Entities
{
    public class TrisEntity : CellEntityBase , ITrisEntity
    {
        private string _text = String.Empty;

        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }
    }
}
