using LIB.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.Entities
{
    public class CellEntityBase : NotifyerPropertyChangedBase
    {
        public delegate void CellClick(int cellId);
        public CellClick cellClicked;

        private int _cellId;

        public int CellId
        {
            get => _cellId;
            set => SetProperty(ref _cellId, value);
        }
    }
}
