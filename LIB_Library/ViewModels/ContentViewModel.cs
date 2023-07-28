using LIB.Interfaces.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.ViewModels
{
    public class ContentViewModel : ViewModelBase
    {
        #region Private Fields
        #endregion

        

        #region Public Properties
        #endregion

        #region Constructor
        public ContentViewModel(INavigationService navService) : base(navService)
        {
        }
        #endregion

        #region Override Methods
        protected override void OnInitialized()
        {
            base.OnInitialized();
            InitGame();
        }
        #endregion

        #region Private Methods
        #endregion

        #region Protected Methods
        protected virtual void InitGame() { }
        #endregion
        public void ResetGame()
        {
            InitGame();
        }
    }
}
