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
using LIB.Entities;
using LIB.Entities.Data.Tris;
using LIB.UserMng;
using static LIB.Entities.Data.Base.GameEnums;

namespace Tris.ViewModels
{
    [ViewRef(typeof(TrisMultiplayer))]
    public class TrisMultiplayerViewModel : TrisGameBaseModel
    {
        #region Private Fields
        private User _secondUser;
        private GameDataTrisMp _mainUserGameResult;
        private GameDataTrisMp _secondUserGameResult;
        private bool _isFirstPlayerTurn = true;
        private bool _isSecondPlayerTurn = false;
        #endregion

        #region Public Properties
        public User SecondUser
        {
            get => _secondUser;
            set
            {
                SetProperty(ref _secondUser, value);
                NotifyPropertyChanged(nameof(SecondUserName));
            }
        }
        public string SecondUserName
        {
            get => SecondUser != null ? SecondUser.Name : "Giocatore 2";
        }
        public bool IsFirstPlayerTurn
        {
            get => _isFirstPlayerTurn;
            set
            {
                SetProperty(ref _isFirstPlayerTurn, value);
                if(value) IsSecondPlayerTurn = false;
            }
        }
        public bool IsSecondPlayerTurn
        {
            get => _isSecondPlayerTurn;
            set
            {
                SetProperty(ref _isSecondPlayerTurn, value);
                if(value) IsFirstPlayerTurn = false;
            }
        }
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
            SecondUser = UserManager.GetSecondLoggedUser();
            if(MainUser != null)
            {
                string opponent = SecondUser == null ? String.Empty : SecondUserName;
                _mainUserGameResult = new GameDataTrisMp(MainUserName, DateTime.Now, TrisResults.defeat, opponent);
                MainUser.Proxy.SaveData(_mainUserGameResult);
            }
            if(SecondUser != null)
            {
                string opponent = MainUser == null ? String.Empty : MainUserName;
                _secondUserGameResult = new GameDataTrisMp(SecondUserName, DateTime.Now, TrisResults.defeat, opponent);
                SecondUser.Proxy.SaveData(_secondUserGameResult);
            }
        }
        protected override void SaveGameResults()
        {
            if(MainUser != null)
            {
                MainUser.Proxy.UpdateData(_mainUserGameResult);
            }
            if(SecondUser != null)
            {
                SecondUser.Proxy.UpdateData(_secondUserGameResult);
            }
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
                    if (SecondUser != null && _secondUserGameResult != null) _secondUserGameResult.GameResults = TrisResults.victory;
                }
                else
                {
                    if (MainUser != null && _mainUserGameResult != null) _mainUserGameResult.GameResults = TrisResults.victory;

                }
                GameOverMessage = $"{player} ha vinto !";
            }
            else
            {
                GameOverMessage = "Pareggio";
                if (SecondUser != null && _secondUserGameResult != null) _secondUserGameResult.GameResults = TrisResults.tie;
                if (MainUser != null && _mainUserGameResult != null) _mainUserGameResult.GameResults = TrisResults.tie;
            }
            EndGame();
        }
        private void SwitchPlayer()
        {
            turn++;
            if(turn % 2 == 0)
            {
                IsSecondPlayerTurn = true;
            }
            else
            {
                IsFirstPlayerTurn = true;
            }
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
                SwitchPlayer();
            }
        }
        #endregion
    }
}
