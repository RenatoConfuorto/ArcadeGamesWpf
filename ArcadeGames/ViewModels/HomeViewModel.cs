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
using ArcadeGames.Views;

namespace ArcadeGames.ViewModels
{
    [ViewRef(typeof(HomeView))]
    public class HomeViewModel : ContentViewModel
    {
        #region Private Fields
        #endregion

        #region Command
        public RelayCommand TrisPageCommand { get; set; }
        #endregion

        #region Public Properties
        #endregion

        #region Constructor
        public HomeViewModel() : base() { }
        #endregion

        #region Override Methods
        protected override void InitCommands()
        {
            base.InitCommands();
            TrisPageCommand = new RelayCommand(TrisPageCommandExecute);
        }
        #endregion

        #region Private Methods
        private void TrisPageCommandExecute(object param)
        {
            ChangeView(ViewNames.TrisHomePage);
        }
        #endregion
    }
}
