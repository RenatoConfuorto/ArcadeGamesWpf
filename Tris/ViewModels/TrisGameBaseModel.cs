using Core.Helpers;
using LIB.Constants;
using LIB.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tris.Common;
using Tris.Common.Entities;

namespace Tris.ViewModels
{
    public abstract class TrisGameBaseModel : GameViewModelBase<TrisEntity>
    {
        #region Private Fields
        protected int turn;
        protected List<int[]> winningCombinations = new List<int[]>()
        {
            new int[] {0, 1, 2},
            new int[] {3, 4, 5},
            new int[] {6, 7, 8},
            new int[] {0, 3, 6},
            new int[] {1, 4, 7},
            new int[] {2, 5, 8},
            new int[] {0, 4, 8},
            new int[] {2, 4, 6}
        };
        #endregion

        #region Public Properties
        #endregion

        #region Constructor
        public TrisGameBaseModel(string viewName) : base(viewName, ViewNames.TrisHomePage)
        {
        }
        #endregion

        #region Override Methods
        protected override void OnInitialized()
        {
            base.OnInitialized();

        }
        protected override void InitGame()
        {
            base.InitGame();
            turn = 1;
        }
        protected override ObservableCollection<TrisEntity> GenerateGrid()
        {
            ObservableCollection<TrisEntity> result = new ObservableCollection<TrisEntity>();
            TrisEntity cell;
            for (int i = 0; i < 9; i++)
            {
                cell = new TrisEntity()
                {
                    CellId = i,
                    Text = null,
                };
                cell.cellClicked += OnCellClicked;
                result.Add(cell);
            }
            return result;
        }
        #endregion

        #region Private Methods
        protected bool CheckVictory()
        {
            for (int i = 0; i < winningCombinations.Count; i++)
            {
                int[] combination = winningCombinations[i];
                int a = combination[0];
                int b = combination[1];
                int c = combination[2];

                if (!String.IsNullOrEmpty(Cells[a].Text) &&
                    Cells[a].Text == Cells[b].Text &&
                    Cells[b].Text == Cells[c].Text)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
        #region Protected Methods
        protected abstract void OnCellClicked(int cellId);
        #endregion
    }
}
