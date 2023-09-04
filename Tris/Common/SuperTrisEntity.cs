using LIB.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tris.Common
{
    public class SuperTrisEntity : CellEntityBase
    {
        public delegate void SuperTrisCellClicked(int CellId, int subCellId);
        public SuperTrisCellClicked MacroCellClicked;

        private string _text = String.Empty;
        private ObservableCollection<TrisEntity> _subCells;
        private bool _isCellActive;
        private bool _isCellClosed;

        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }

        public ObservableCollection<TrisEntity> SubCells
        {
            get => _subCells;
            set => SetProperty(ref _subCells, value);
        }

        public bool IsCellActive
        {
            get => _isCellActive;
            set => SetProperty(ref _isCellActive, value);
        }

        public bool IsCellClosed
        {
            get => _isCellActive;
            set => SetProperty(ref _isCellClosed, value);
        }
    }
}
