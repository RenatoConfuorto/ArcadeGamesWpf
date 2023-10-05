using Core.Commands;
using LIB.Constants;
using Core.Interfaces.Navigation;
using LIB.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Attributes;
using Tris.Views;
using LIB.Attributes;
using LIB.Sounds;

namespace Tris.ViewModels
{
    [ViewRef(typeof(TrisHomePageView))]
    [BackgroundMusic(SoundsManagment.MainBackground)]
    public class TrisHomePageViewModel : ContentViewModel
    {
        #region Private Fields
        #endregion

        #region Public Properties
        public RelayCommand MultiplayerCommand { get; set; }
        public RelayCommand SingleplayerCommand { get; set; }
        public RelayCommand SuperTrisMpCommand { get; set; }
        #endregion

        #region Constructor
        public TrisHomePageViewModel() : base(ViewNames.TrisHomePage, ViewNames.Home) { }
        #endregion

        #region Override Methods
        protected override void InitCommands()
        {
            base.InitCommands();
            MultiplayerCommand = new RelayCommand(MultiplayerCommandExecute);
            SingleplayerCommand = new RelayCommand(SingleplayerCommandExecute);
            SuperTrisMpCommand = new RelayCommand(SuperTrisMpCommandExecute);
        }
        #endregion

        #region Private Methods
        private void MultiplayerCommandExecute(object param)
        {
            ChangeView(ViewNames.TrisMultiplayer);
        }
        private void SingleplayerCommandExecute(object param)
        {
            ChangeView(ViewNames.TrisSingleplayer);
        }
        private void SuperTrisMpCommandExecute(object param)
        {
            ChangeView(ViewNames.SuperTrisMultiplayer);
        }
        #endregion
    }
}
