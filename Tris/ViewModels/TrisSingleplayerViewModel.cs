﻿using LIB.Constants;
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
using System.Threading;
using System.Windows;
using Tris.Common.Entities;
using static Tris.Common.Constants;
using LIB.Entities;
using LIB.Helpers;
using LIB.UserMng;
using LIB.Entities.Data.Tris;
using static LIB.Entities.Data.Base.GameEnums;
using LIB.Sounds;

namespace Tris.ViewModels
{
    [ViewRef(typeof(TrisSingleplayer))]
    public class TrisSingleplayerViewModel : TrisGameBaseModel
    {
        #region Private Fields
        private bool _isPlayerTurn = true;
        private Thread parallelThread = null;
        private GameDataTrisSp _gameResults;
        #endregion

        #region Public Properties
        public bool IsPlayerTurn
        {
            get => _isPlayerTurn;
            set => SetProperty(ref _isPlayerTurn, value);
        }
        #endregion

        #region Constructor
        public TrisSingleplayerViewModel() : base(ViewNames.TrisSingleplayer)
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
            if (parallelThread != null)
            {
                try
                {
                    parallelThread.Abort();
                }
                catch (Exception ex)
                {

                }
            }
            IsPlayerTurn = true;
        }
        protected override void MenageGameUsers()
        {
            base.MenageGameUsers();
            if(MainUser != null)
            {
                _gameResults = new GameDataTrisSp(MainUser.Name, DateTime.Now, TrisResults.defeat);
                MainUser.Proxy.SaveData(_gameResults);
            }
        }
        protected override void SaveGameResults()
        {
            if(MainUser != null)
            {
                MainUser.Proxy.UpdateData(_gameResults);
            }
        }
        #endregion

        #region Private Methods
        private void CloseGame(bool victory = false)
        {
            if (victory)
            {
                string player = MainUserName;
                if (!IsPlayerTurn)
                {
                    player = "Computer";
                }
                if(MainUser != null)
                {
                    if(IsPlayerTurn) _gameResults.GameResults = TrisResults.victory; //player has won if it's not the computer turn
                }
                GameOverMessage = $"{player} ha vinto !";
            }
            else
            {
                GameOverMessage = "Pareggio";
                _gameResults.GameResults = TrisResults.tie;
            }
            EndGame();
        }
        private bool AfterSign()
        {
            if (CheckVictory())
            {
                CloseGame(true);
                return false;
            }
            else if (turn == 9)
            {
                CloseGame();
                return false;
            }
            turn++;
            return true;
        }
        private void ComputerSign()
        {
            try
            {
                //isComputerTurn = true;
                Thread.Sleep(500);
                bool isSignPlaced = false;
                for (int i = 0; i < winningCombinations.Count; i++)
                {//controllare tutte le combinazioni vincenti per decidere dove inserire il segno
                    int[] combination = winningCombinations[i];
                    int a = combination[0];
                    int b = combination[1];
                    int c = combination[2];

                    if (!String.IsNullOrEmpty(Cells[a].Text) &&
                        Cells[a].Text == Cells[b].Text &&
                        String.IsNullOrEmpty(Cells[c].Text))
                    {
                        PlaceComputerSign(c, out isSignPlaced);
                        break;
                    }
                    else if (!String.IsNullOrEmpty(Cells[a].Text)
                        && Cells[c].Text == Cells[a].Text &&
                        String.IsNullOrEmpty(Cells[b].Text))
                    {
                        PlaceComputerSign(b, out isSignPlaced);
                        break;
                    }
                    else if (!String.IsNullOrEmpty(Cells[b].Text) &&
                        Cells[c].Text == Cells[b].Text &&
                        String.IsNullOrEmpty(Cells[a].Text))
                    {
                        PlaceComputerSign(a, out isSignPlaced);
                        break;
                    }
                }
                if (!isSignPlaced)
                {
                    //il segno non è stato piazzato, piazzarne uno a caso
                    Random rd = new Random();
                    while (!isSignPlaced)
                    {
                        int a = rd.Next(Cells.Count);
                        if (String.IsNullOrEmpty(Cells[a].Text)) PlaceComputerSign(a, out isSignPlaced);
                    }
                }
                AfterSign();


                IsPlayerTurn = true;
            }
            catch (Exception ex)
            {

            }
        }

        private void PlaceComputerSign(int cellIdx, out bool isSignPlaced)
        {
            //Cells[cellIdx].Text = Players.O.ToString();
            PlaceSign(cellIdx, Players.O);
            isSignPlaced = true;
        }
        #endregion
        #region Protected Methods
        protected override void OnCellClicked(int cellId)
        {
            if (!IsPlayerTurn) return;
            if (parallelThread != null) parallelThread.Abort();
            TrisEntity entity = Cells.Where(c => c.CellId == cellId).FirstOrDefault();
            if (String.IsNullOrEmpty(entity.Text))
            {
                //entity.Text = Players.X.ToString();
                PlaceSign(entity.CellId, Players.X);
                if (AfterSign())
                {
                    IsPlayerTurn = false;
                    parallelThread = new Thread(() =>
                    {
                        ComputerSign();
                        parallelThread.Abort();
                    });
                    parallelThread.Start();
                }
            }
        }
        #endregion
    }
}
