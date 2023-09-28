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
using LIB.Entities.Data.Tris;
using LIB.UserMng;
using static LIB.Entities.Data.Base.GameResults;
using LIB.Attributes;

namespace Tris.ViewModels
{
    [ViewRef(typeof(SuperTrisMp))]
    [SettingsPopup(ViewNames.SuperTrisMpSettings)]
    public class SuperTrisMultiplayerViewModel : SuperTrisBaseModel
    {
        #region Private Fields
        private TimerEntity _firstPlayerTimer;
        private TimerEntity _secondPlayerTimer;
        private SuperTrisSettings _settings = new SuperTrisSettings(Constants.SUPER_TRIS_DEFAULT_START_TIME);
        private Timer timer;
        private User _secondUser;
        private GameDataSuperTrisMp _mainUserGameResult;
        private GameDataSuperTrisMp _secondUserGameResult;
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
        public User SecondUser
        {
            get => _secondUser;
            set
            {
                SetProperty(ref _secondUser, value);
            }
        }
        public string SecondUserName
        {
            get => SecondUser != null ? SecondUser.Name : "Giocatore 2";
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
            if (timer != null) { timer.Dispose(); }
            InitPlayerTimers();
            StartTimer();
        }

        protected override void EndGame()
        {
            if(timer != null) { timer.Dispose(); }
            base.EndGame();
        }
        protected override void MenageGameUsers()
        {
            base.MenageGameUsers();
            SecondUser = UserManager.GetSecondLoggedUser();
            if (MainUser != null)
            {
                string opponent = SecondUser == null ? String.Empty : SecondUserName;
                _mainUserGameResult = new GameDataSuperTrisMp(MainUserName, DateTime.Now, TrisResults.defeat, opponent, 0, 0);
                MainUser.Proxy.SaveData(_mainUserGameResult);
            }
            if (SecondUser != null)
            {
                string opponent = MainUser == null ? String.Empty : MainUserName;
                _secondUserGameResult = new GameDataSuperTrisMp(SecondUserName, DateTime.Now, TrisResults.defeat, opponent, 0, 0);
                SecondUser.Proxy.SaveData(_secondUserGameResult);
            }
        }
        protected override void SaveGameResults()
        {
            if (MainUser != null)
            {
                _mainUserGameResult.RemainingTime = FirstPlayerTimer.Time;
                MainUser.Proxy.UpdateData(_mainUserGameResult);
            }
            if (SecondUser != null)
            {
                _secondUserGameResult.RemainingTime = SecondPlayerTimer.Time;
                SecondUser.Proxy.UpdateData(_secondUserGameResult);
            }
        }
        protected override void OnMacroCellClosed(string PlayerSymbol)
        {
            if(PlayerSymbol == Players.X.ToString())
            {
                if(MainUser != null)
                {
                    _mainUserGameResult.CellsWon++;
                }
            }
            else if(PlayerSymbol == Players.O.ToString())
            {
                if (SecondUser != null)
                {
                    _secondUserGameResult.CellsWon++;
                }
            }
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
        protected override GameSettingsBase PrepareDataForPopup()
        {
            if (timer.Enabled) timer.Stop();
            return _settings;
        }
        protected override void OnPopupClosed()
        {
            base.OnPopupClosed();
            if(!timer.Enabled && !IsGameOver)timer.Start(); //!IsGameOver means the timer is not disposed
        }
        protected override void CloseGame(string gameOverMessage)
        {
            if(gameOverMessage == "Pareggio")
            {
                if (MainUser != null) _mainUserGameResult.GameResults = TrisResults.tie;
                if (SecondUser != null) _secondUserGameResult.GameResults = TrisResults.tie;
            }
            else
            {
                string player = GetPlayerSymbol();
                if(player == Players.X.ToString())
                {
                    if(MainUser != null)
                    {
                        _mainUserGameResult.GameResults = TrisResults.victory;
                    }
                    if(SecondUser != null)
                    {
                        _secondUserGameResult.GameResults = TrisResults.defeat;
                    }
                }
                else if(player == Players.O.ToString())
                {
                    if (MainUser != null)
                    {
                        _mainUserGameResult.GameResults = TrisResults.defeat;
                    }
                    if (SecondUser != null)
                    {
                        _secondUserGameResult.GameResults = TrisResults.victory;
                    }
                }
            }
            base.CloseGame(gameOverMessage);
        }
        protected override void OnSettingsReceived(object settings)
        {
            if(settings is SuperTrisSettings set)
            {
                settings = set;
            }
        }
        #endregion
        #region Private Methods
        private void InitPlayerTimers()
        {
            int _playersTime = _settings.PlayersTime;
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
            UpdateUsersData();
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
        private void UpdateUsersData()
        {
            if(MainUser != null)
            {
                _mainUserGameResult.StartingTime = _settings.PlayersTime;
                MainUser.Proxy.UpdateData(_mainUserGameResult);
            }
            if(SecondUser != null)
            {
                _secondUserGameResult.StartingTime = _settings.PlayersTime;
                SecondUser.Proxy.UpdateData(_secondUserGameResult);
            }
        }
        #endregion
    }
}
