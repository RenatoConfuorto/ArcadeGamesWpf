using ArcadeGames.Views;
using Core.Attributes;
using Core.Commands;
using Core.Helpers;
using LIB.Constants;
using LIB.Helpers;
using LIB.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace ArcadeGames.ViewModels
{
    [ViewRef(typeof(MultiPlayerLobbyView))]
    public class MultiPlayerLobbyViewModel : ContentViewModel
    {
        #region Private Fields
        #endregion

        #region Command
        #endregion

        #region Public Properties
        #endregion

        #region Constructor
        public MultiPlayerLobbyViewModel(object param) : base(ViewNames.MultiPlayerLobby, ViewNames.MultiPlayerForm, param) { }
        #endregion

        #region Override Methods
        protected override void OnInitialized()
        {
            base.OnInitialized();
        }
        protected override void InitCommands()
        {
            base.InitCommands();
        }
        protected override void SetCommandExecutionStatus()
        {
            base.SetCommandExecutionStatus();
        }
        #endregion

        #region Private Methods
        #endregion
    }
}
