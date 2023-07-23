using LIB.Base;
using LIB.Interfaces.Navigation;
using LIB.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcadeGames.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Private Fields
        #endregion

        #region Public Properties
        #endregion

        #region Commands
        #endregion

        #region Constructor
        public MainWindowViewModel(INavigationService navService) 
            : base(navService)
        {
        }
        #endregion

        #region Override Methods
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
