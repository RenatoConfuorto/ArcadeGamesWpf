using Core.Attributes;
using Core.Commands;
using LIB.Constants;
using LIB.ViewModels;
using MemoryGame.Common.Entities;
using MemoryGame.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using static MemoryGame.Common.Constants;

namespace MemoryGame.ViewModels
{
    [ViewRef(typeof(MemorySingleplayerSettingsView))]
    public class MemorySingleplayerSettingsViewModel : SettingsViewModelBase<MemorySingleplayerSettings>
    {
        #region Private fields
        private const int MAX_CARD_PER_TYPE = 20;
        private List<Tuple<string, Difficulty>> _difficulties = new List<Tuple<string, Difficulty>>()
        {
            Tuple.Create("Facile", Difficulty.Easy),
            Tuple.Create("Medio", Difficulty.Medium),
            Tuple.Create("Difficile", Difficulty.Hard),
            Tuple.Create("Personalizzato", Difficulty.Custom),
        };
        #endregion

        #region Public Properties
        public List<Tuple<string, Difficulty>> Difficulties
        {
            get => _difficulties;
            set => SetProperty(ref _difficulties, value);
        }
        public Difficulty SelectedDifficulty
        {
            get => Settings.GameDifficulty;
            set
            {
                Settings.GameDifficulty = value;
                NotifyPropertyChanged();
                OnDifficultyChanged(value);
                SetCommandExecutionStatus();
            }
        }
        public int CardsPerType
        {
            get => (int)Settings?.CardsNumber / 6;
            set
            {
                if(Settings != null)
                {
                    Settings.CardsNumber = value * 6;
                }
                NotifyPropertyChanged();
                SetCommandExecutionStatus();
            }
        }
        public int ErrorsLimit
        {
            get => (int)Settings?.ErrorsLimit;
            set
            {
                if(Settings != null)
                {
                    Settings.ErrorsLimit = value;
                }
                NotifyPropertyChanged();
                SetCommandExecutionStatus();
            }
        }
        public bool IsDataEditable
        {
            get => SelectedDifficulty == Difficulty.Custom;
        }
        #endregion

        #region Commands
        public RelayCommand AddPair { get; set; }
        public RelayCommand RemovePair { get; set; }
        public RelayCommand AddErrorLimit { get; set; }
        public RelayCommand RemoveErrorLimit { get; set; }
        public RelayCommand AddErrorLimitMultiple { get; set; }
        public RelayCommand RemoveErrorLimitMultiple { get; set; }
        #endregion

        #region Constructor
        public MemorySingleplayerSettingsViewModel(object param)
            : base(ViewNames.SuperTrisMpSettings, param)
        {
        }
        #endregion

        #region Override Methods
        protected override void GetViewParameter()
        {
            base.GetViewParameter();
            SetCommandExecutionStatus();
        }
        protected override void InitCommands()
        {
            base.InitCommands();
            AddPair = new RelayCommand(AddPairExecute, AddPairCanExecute);
            RemovePair = new RelayCommand(RemovePairExecute, RemovePairCanExecute);
            AddErrorLimit = new RelayCommand(AddErrorLimitExecute, AddErrorLimitCanExecute);
            RemoveErrorLimit = new RelayCommand(RemoveErrorLimitExecute, RemoveErrorLimitCanExecute);
            AddErrorLimitMultiple = new RelayCommand(AddErrorLimitMultipleExecute, AddErrorLimitMultipleCanExecute);
            RemoveErrorLimitMultiple = new RelayCommand(RemoveErrorLimitMultipleExecute, RemoveErrorLimitMultipleCanExecute);
        }
        protected override void SetCommandExecutionStatus()
        {
            base.SetCommandExecutionStatus();
            AddPair.RaiseCanExecuteChanged();
            RemovePair.RaiseCanExecuteChanged();
            AddErrorLimit.RaiseCanExecuteChanged();
            RemoveErrorLimit.RaiseCanExecuteChanged();
            AddErrorLimitMultiple.RaiseCanExecuteChanged();
            RemoveErrorLimitMultiple.RaiseCanExecuteChanged();
        }
        #endregion

        #region Private Methods
        private void OnDifficultyChanged(Difficulty diff)
        {
            switch(diff)
            {
                case Difficulty.Easy:
                    Settings = new MemorySingleplayerSettings()
                    {
                        GameDifficulty = diff,
                        CardsNumber = CARDS_NO_EASY,
                        ErrorsLimit = ERRORS_LIMIT_EASY
                    };
                    break;
                case Difficulty.Medium:
                    Settings = new MemorySingleplayerSettings()
                    {
                        GameDifficulty = diff,
                        CardsNumber = CARDS_NO_MEDIUM,
                        ErrorsLimit = ERRORS_LIMIT_MEDIUM
                    };
                    break;
                case Difficulty.Hard:
                    Settings = new MemorySingleplayerSettings()
                    {
                        GameDifficulty = diff,
                        CardsNumber = CARDS_NO_HARD,
                        ErrorsLimit = ERRORS_LIMIT_HARD
                    };
                    break;
                case Difficulty.Custom:
                    break;
            }
            NotifyPropertyChanged(nameof(CardsPerType));
            NotifyPropertyChanged(nameof(IsDataEditable));
            NotifyPropertyChanged(nameof(ErrorsLimit));
        }
        private void AddPairExecute(object param)
        {
            CardsPerType += 2;
        }
        private bool AddPairCanExecute(object param) => SelectedDifficulty == Difficulty.Custom && CardsPerType < MAX_CARD_PER_TYPE;
        private void RemovePairExecute(object param)
        {
            CardsPerType -= 2;
        }
        private bool RemovePairCanExecute(object param) => SelectedDifficulty == Difficulty.Custom && CardsPerType > 2;

        private void AddErrorLimitExecute(object param) => ErrorsLimit++;
        private bool AddErrorLimitCanExecute(object param) => SelectedDifficulty == Difficulty.Custom;
        private void RemoveErrorLimitExecute(object param) => ErrorsLimit--;
        private bool RemoveErrorLimitCanExecute(object param) => SelectedDifficulty == Difficulty.Custom && ErrorsLimit > 1;

        private void AddErrorLimitMultipleExecute(object param) => ErrorsLimit+=5;
        private bool AddErrorLimitMultipleCanExecute(object param) => AddErrorLimitCanExecute(null);
        private void RemoveErrorLimitMultipleExecute(object param)
        {
            if (ErrorsLimit > 5) ErrorsLimit -= 5;
            else ErrorsLimit = 1;
        }
        private bool RemoveErrorLimitMultipleCanExecute(object param) => RemoveErrorLimitCanExecute(null);
        #endregion
    }
}
