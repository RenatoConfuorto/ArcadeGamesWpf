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
using static Tris.Common.Constants;

namespace Tris.ViewModels
{
    [ViewRef(typeof(TrisMultiplayer))]
    public class TrisMultiplayerViewModel : TrisGameBaseModel
    {
        #region Private Fields
        #endregion

        #region Public Properties
        #endregion

        #region Constructor
        public TrisMultiplayerViewModel() : base(ViewNames.TrisMultiplayer) 
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
        protected override void MenageGameUsers()
        {
            base.MenageGameUsers();
        }

        protected override void SaveGameResults()
        {
        }
        #endregion

        #region Private Methods
        private void CloseGame(bool victory = false)
        {
            if(victory)
            {
                string player = Players.X.ToString();
                if(turn % 2 == 0)
                {
                    player = Players.O.ToString();
                }
                GameOverMessage = $"{player} ha vinto !";
            }
            else
            {
                GameOverMessage = "Pareggio";
            }
            EndGame();
        }
        #endregion
        #region Protected Methods
        protected override void OnCellClicked(int cellId)
        {
            TrisEntity entity = Cells.Where(c => c.CellId == cellId).FirstOrDefault();
            if (String.IsNullOrEmpty(entity.Text))
            {//la cella non è stata cliccata
                if (turn % 2 == 0)
                {
                    entity.Text = Players.O.ToString();
                }
                else
                {
                    entity.Text = Players.X.ToString();
                }
                if (CheckVictory())
                {
                    CloseGame(true);
                    return;
                }
                else if (turn == 9)
                {
                    CloseGame();
                    return;
                }
                turn++;
            }
        }
        #endregion
    }
}
