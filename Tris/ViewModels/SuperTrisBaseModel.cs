using Core.Helpers;
using LIB.Constants;
using LIB.Sounds;
using LIB.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tris.Common;
using Tris.Common.Entities;
using Tris.Common.Interfaces;
using Tris.Common.UserControls;
using static Tris.Common.Constants;

namespace Tris.ViewModels
{
    public abstract class SuperTrisBaseModel : LocalGameViewModelBase<SuperTrisEntity>
    {
        #region Private Fields
        protected const int MACRO_SIGN_PLACED_WAIT_TIME = 400; //ms
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
        public SuperTrisBaseModel(string viewName) : base(viewName, ViewNames.TrisHomePage)
        {
        }
        #endregion

        #region Override Methods
        protected override void OnInitialized()
        {
            base.OnInitialized();
            PlaySound(SoundsManagment.TrisSoundPenClickingTwice);
        }
        protected override void InitGame()
        {
            base.InitGame();
            turn = 1;
        }
        protected override ObservableCollection<SuperTrisEntity> GenerateGrid()
        {
            ObservableCollection<SuperTrisEntity> result = new ObservableCollection<SuperTrisEntity>();
            SuperTrisEntity cell;
            for (int i = 0; i < 9; i++)
            {
                cell = InitMacroCell(i);
                result.Add(cell);
            }
            return result;
        }
        #endregion

        #region Private Methods
        #endregion
        #region Protected Methods
        protected abstract void OnMacroCellClosed(string PlayerSymbol);
        protected bool CheckVictory<T>(ObservableCollection<T> cells, out string winningSymbol) where T : ITrisEntity
        {
            winningSymbol = String.Empty;
            for (int i = 0; i < winningCombinations.Count; i++)
            {
                int[] combination = winningCombinations[i];
                int a = combination[0];
                int b = combination[1];
                int c = combination[2];

                if (!String.IsNullOrEmpty(cells[a].Text) &&
                    cells[a].Text == cells[b].Text &&
                    cells[b].Text == cells[c].Text)
                {
                    winningSymbol = cells[a].Text;
                    PlaySound(SoundsManagment.TrisSoundPenWriteLong);
                    return true;
                }
            }
            return false;
        }
        protected abstract void OnMacroCellClicked(int CellId, int SubCellId);
        protected SuperTrisEntity InitMacroCell(int CellId)
        {
            SuperTrisEntity cell = new SuperTrisEntity()
            {
                CellId = CellId,
                Text = null,
            };
            cell.SubCells = new ObservableCollection<TrisEntity>();
            TrisEntity subCell;
            for (int j = 0; j < 9; j++)
            {
                subCell = new TrisEntity()
                {
                    CellId = j,
                    Text = null,
                };
                cell.SubCells.Add(subCell);
            }
            cell.IsCellActive = true;
            cell.IsCellClosed = false;
            cell.MacroCellClicked += OnMacroCellClicked;
            return cell;
        }
        protected string GetPlayerSymbol()
        {
            if (turn % 2 == 0) return Players.O.ToString();
            else return Players.X.ToString();
        }
        protected void ActivateMacroCell(int SubCellId)
        {
            //prendere la cella da attivare
            SuperTrisEntity newActiveCell = Cells.Where(c => c.CellId == SubCellId).FirstOrDefault();
            //Disattivare tutte le macro celle
            foreach (SuperTrisEntity cell in Cells) cell.IsCellActive = false;
            if (!newActiveCell.IsCellClosed)
            {
                //attivare la nuova cella
                newActiveCell.IsCellActive = true;
            }
            else
            {
                //attivare tutte le macroCelle
                foreach (SuperTrisEntity cell in Cells) if(!cell.IsCellClosed) cell.IsCellActive = true;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CellId"></param>
        /// <returns>true se il gioco può continuare</returns>
        protected bool CheckAndUpdateMacroCellStatus(int CellId)
        {
            
            string winningSymbol = String.Empty;
            SuperTrisEntity cell = Cells.Where(c => c.CellId == CellId).FirstOrDefault();
            //controllare la griglia nella cella
            if (CheckVictory(cell.SubCells, out winningSymbol))
            {
                Thread.Sleep(MACRO_SIGN_PLACED_WAIT_TIME);
                //posizionare il simbolo e chiudere la cella
                cell.Text = winningSymbol;
                cell.IsCellClosed = true;
                PlaySound(SoundsManagment.TrisSoundPenWriteLong);
                OnMacroCellClosed(GetPlayerSymbol());
                //controllare la griglia principale
                if(CheckVictory(Cells, out winningSymbol))
                {
                    CloseGame($"{winningSymbol} ha vinto");
                    return false;
                }else if(Cells.Where(c => String.IsNullOrEmpty(c.Text)).Count() == 0)
                {
                    CloseGame("Pareggio");
                    return false;
                }
            }else if(cell.SubCells.Where(sb => String.IsNullOrEmpty(sb.Text)).Count() == 0)
            {
                //pareggio, resettare la griglia
                Thread.Sleep(MACRO_SIGN_PLACED_WAIT_TIME);
                cell = InitMacroCell(CellId);
                Cells[CellId] = cell;
                NotifyPropertyChanged(nameof(Cells));
            }
            return true;
        }

        protected virtual void CloseGame(string gameOverMessage)
        {
            GameOverMessage = gameOverMessage;
            EndGame();
        }
        #endregion

    }
}
