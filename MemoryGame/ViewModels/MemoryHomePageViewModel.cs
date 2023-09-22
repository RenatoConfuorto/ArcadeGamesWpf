using Core.Attributes;
using Core.Commands;
using LIB.Constants;
using LIB.ViewModels;
using MemoryGame.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryGame.ViewModels
{
    [ViewRef(typeof(MemoryHomePageView))]
    public class MemoryHomePageViewModel : ContentViewModel
    {
        #region Private Fields
        #endregion

        #region Public Properties
        public RelayCommand MultiplayerCommand { get; set; }
        public RelayCommand SingleplayerCommand { get; set; }
        #endregion

        #region Constructor
        public MemoryHomePageViewModel() 
            : base(ViewNames.MemoryGameHomePage, ViewNames.Home) { }
        #endregion

        #region Override Methods
        protected override void InitCommands()
        {
            base.InitCommands();
            MultiplayerCommand = new RelayCommand(MultiplayerCommandExecute);
            SingleplayerCommand = new RelayCommand(SingleplayerCommandExecute);
        }
        #endregion

        #region Private Methods
        private void MultiplayerCommandExecute(object param)
        {
        }
        private void SingleplayerCommandExecute(object param)
        {
        }
        #endregion
    }
}
