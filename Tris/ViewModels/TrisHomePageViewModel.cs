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

namespace Tris.ViewModels
{
    [ViewRef(typeof(TrisHomePageView))]
    public class TrisHomePageViewModel : ContentViewModel
    {
        #region Private Fields
        #endregion

        #region Public Properties
        public RelayCommand MultiplayerCommand { get; set; }
        public RelayCommand SingleplayerCommand { get; set; }
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
        #endregion
    }
}
