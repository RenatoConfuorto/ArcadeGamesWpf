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
            
        }
        #endregion
    }
}
