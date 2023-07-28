using LIB.Attributes;
using LIB.Helpers;
using LIB.Interfaces.Navigation;
using LIB.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tris.Common;

namespace Tris.ViewModels
{
    [ParentView(typeof(TrisHomePageViewModel))]
    public class TrisMultiplayerViewModel : ContentViewModel
    {
        #region Private Fields
        private int turn;
        private ObservableCollection<TrisEntity> _cells;
        private List<int[]> winningCombinations = new List<int[]>()
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
        public ObservableCollection<TrisEntity> Cells
        {
            get => _cells;
            set => SetProperty(ref _cells, value);
        }
        #endregion

        #region Constructor
        public TrisMultiplayerViewModel() : this(null) { }
        public TrisMultiplayerViewModel(INavigationService navService) : base(navService)
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
            Cells = GenerateGrid();
        }
        #endregion

        #region Private Methods
        private ObservableCollection<TrisEntity> GenerateGrid()
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
        private bool CheckVictory()
        {
            for(int i = 0; i < winningCombinations.Count; i++)
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
        private void CloseGame(bool victory = false)
        {
            if(victory)
            {
                string player = "X";
                if(turn % 2 == 0)
                {
                    player = "O";
                }
                MessageDialogHelper.ShowInfoMessage($"{player} ha vinto !");
            }
            else
            {
                MessageDialogHelper.ShowInfoMessage("Pareggio");
            }
            InitGame();
        }
        #endregion
        #region Protected Methods
        protected virtual void OnCellClicked(int cellId)
        {
            TrisEntity entity = Cells.Where(c => c.CellId == cellId).FirstOrDefault();
            if (String.IsNullOrEmpty(entity.Text))
            {//la cella non è stata cliccata
                if (turn % 2 == 0)
                {
                    entity.Text = "O";
                }
                else
                {
                    entity.Text = "X";
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
