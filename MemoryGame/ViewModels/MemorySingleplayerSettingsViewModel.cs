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
        }
        protected override void SetCommandExecutionStatus()
        {
            base.SetCommandExecutionStatus();
            AddPair.RaiseCanExecuteChanged();
            RemovePair.RaiseCanExecuteChanged();
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
        }
        private void AddPairExecute(object param)
        {
            CardsPerType += 2;
        }
        private bool AddPairCanExecute(object param) => SelectedDifficulty == Difficulty.Custom;
        private void RemovePairExecute(object param)
        {
            CardsPerType -= 2;
        }
        private bool RemovePairCanExecute(object param) => SelectedDifficulty == Difficulty.Custom && CardsPerType > 2;
        #endregion
    }
}
