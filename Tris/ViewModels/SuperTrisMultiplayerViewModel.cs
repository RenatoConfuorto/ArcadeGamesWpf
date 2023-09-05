using LIB.Constants;
using Core.Helpers;
using Core.Interfaces.Navigation;
using LIB.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tris.Common;
using Core.Attributes;
using Tris.Views;
using Tris.Common.Entities;

namespace Tris.ViewModels
{
    [ViewRef(typeof(SuperTrisMp))]
    public class SuperTrisMultiplayerViewModel : SuperTrisBaseModel
    {
        #region Private Fields
        #endregion

        #region Public Properties
        #endregion

        #region Constructor
        public SuperTrisMultiplayerViewModel() : base(ViewNames.SuperTrisMultiplayer) 
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
        }
        #endregion

        #region Private Methods
        //private void CloseGame(bool victory = false)
        //{
        //    if(victory)
        //    {
        //        string player = "X";
        //        if(turn % 2 == 0)
        //        {
        //            player = "O";
        //        }
        //        GameOverMessage = $"{player} ha vinto !";
        //    }
        //    else
        //    {
        //        GameOverMessage = "Pareggio";
        //    }
        //    EndGame();
        //}
        #endregion
        #region Protected Methods
        protected override void OnMacroCellClicked(int CellId, int SubCellId)
        {
            SuperTrisEntity entity = Cells.Where(c => c.CellId == CellId).FirstOrDefault();
            if (String.IsNullOrEmpty(entity.Text))
            {//nella macro cella non c'è il simbolo
                //prendere la subCella
                TrisEntity subCell = entity.SubCells.Where(sb => sb.CellId == SubCellId).FirstOrDefault();
                if (String.IsNullOrEmpty(subCell.Text))
                {
                    subCell.Text = GetPlayerSymbol();
                    CheckAndUpdateMacroCellStatus(CellId);

                    ActivateMacroCell(SubCellId);
                    turn++;
                }
            }
        }
        #endregion
    }
}
