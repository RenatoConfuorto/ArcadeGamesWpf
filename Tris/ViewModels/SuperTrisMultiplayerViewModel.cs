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
using System.Timers;
using LIB.Entities;
using static Tris.Common.Constants;

namespace Tris.ViewModels
{
    [ViewRef(typeof(SuperTrisMp))]
    public class SuperTrisMultiplayerViewModel : SuperTrisBaseModel
    {
        #region Private Fields
        private TimerEntity _firstPlayerTimer;
        private TimerEntity _secondPlayerTimer;
        private int _playersTime = 300;
        private Timer timer; //TODO il timer non si stoppa se si pareggia
        #endregion

        #region Public Properties
        public TimerEntity FirstPlayerTimer
        {
            get => _firstPlayerTimer;
            set => SetProperty(ref _firstPlayerTimer, value);
        }

        public TimerEntity SecondPlayerTimer
        {
            get => _secondPlayerTimer;
            set => SetProperty(ref _secondPlayerTimer, value);
        }
        #endregion

        #region Constructor
        public SuperTrisMultiplayerViewModel() : base(ViewNames.SuperTrisMultiplayer)
        {
        }
        #endregion

        #region Override Methods
        public override void Dispose()
        {
            base.Dispose();
            timer.Dispose();
        }
        protected override void OnInitialized()
        {
            if (timer != null) { timer.Dispose(); }
            base.OnInitialized();
        }
        protected override void InitGame()
        {
            base.InitGame();
            InitPlayerTimers();
            StartTimer();
        }
        protected override void MenageGameUsers()
        {
            base.MenageGameUsers();
        }

        protected override void SaveGameResults()
        {
        }
        protected override void EndGame()
        {
            if(timer != null) { timer.Dispose(); }
            base.EndGame();
        }
        protected override void OnMacroCellClicked(int CellId, int SubCellId)
        {
            SuperTrisEntity entity = Cells.Where(c => c.CellId == CellId).FirstOrDefault();
            if (String.IsNullOrEmpty(entity.Text))
            {//nella macro cella non c'è il simbolo
                //prendere la subCella
                TrisEntity subCell = entity.SubCells.Where(sb => sb.CellId == SubCellId).FirstOrDefault();
                if (String.IsNullOrEmpty(subCell.Text))
                {
                    StopTimer();

                    subCell.Text = GetPlayerSymbol();
                    if (CheckAndUpdateMacroCellStatus(CellId))
                    {
                        ActivateMacroCell(SubCellId);
                        turn++;
                        StartTimer();
                    }
                }
            }
        }
        #endregion
        #region Private Methods
        private void InitPlayerTimers()
        {
            FirstPlayerTimer = new TimerEntity()
            {
                PlayerName = Players.X.ToString(),
                OriginalTime = _playersTime,
                Time = _playersTime,
                TimerEnabled = false,
            };
            SecondPlayerTimer = new TimerEntity()
            {
                PlayerName = Players.O.ToString(),
                OriginalTime = _playersTime,
                Time = _playersTime,
                TimerEnabled = false,
            };
        }
        private void StartTimer()
        {
            //abilitare il timer del giocatore
            if (GetPlayerSymbol() == Players.X.ToString())
            {
                FirstPlayerTimer.TimerEnabled = true;
                SecondPlayerTimer.TimerEnabled = false;
            }
            else
            {
                FirstPlayerTimer.TimerEnabled = false;
                SecondPlayerTimer.TimerEnabled = true;
            }

            timer = new Timer(1000);
            timer.Elapsed += TimerCallBack;
            timer.Start();
        }
        private void StopTimer()
        {
            if (timer != null && timer.Enabled)
            {
                timer.Dispose();
            }
        }
        private void TimerCallBack(Object source, ElapsedEventArgs e)
        {
            if (GetPlayerSymbol() == Players.X.ToString())
            {
                if (FirstPlayerTimer != null && FirstPlayerTimer.Time > 0)
                {
                    FirstPlayerTimer.Time--;
                    if (FirstPlayerTimer.Time == 0) TimeRunOut(0);
                }
            }
            else
            {
                if (SecondPlayerTimer != null && SecondPlayerTimer.Time > 0)
                {
                    SecondPlayerTimer.Time--;
                    if (SecondPlayerTimer.Time == 0) TimeRunOut(1);
                }
            }
        }
        /// <summary>
        /// One player has run out of time and the game is over
        /// </summary>
        /// <param name="player">Giocatore che ha perso 0 => player X, 1 => player O </param>
        private void TimeRunOut(int player)
        {
            if (timer != null) timer.Dispose();
            string message = player == 0 ? Players.O.ToString() : Players.X.ToString();
            message += " Ha vinto \r\nTimeOut";
            GameOverMessage = message;
            EndGame();
        }
        #endregion
    }
}
