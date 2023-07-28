using LIB.Attributes;
using LIB.Base;
using LIB.Interfaces.Navigation;
using LIB.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tris.ViewModels
{
    [ParentView(typeof(ViewModelBase))]
    public class TrisHomePageViewModel : ContentViewModel
    {
        #region Private Fields
        #endregion

        #region Public Properties
        public RelayCommand MultiplayerCommand { get; set; }
        #endregion

        #region Constructor
        public TrisHomePageViewModel() : this(null) { }
        public TrisHomePageViewModel(INavigationService navService) : base(navService)
        {
        }
        #endregion

        #region Override Methods
        protected override void InitCommands()
        {
            base.InitCommands();
            MultiplayerCommand = new RelayCommand(MultiplayerCommandExecute);
        }
        #endregion

        #region Private Methods
        private void MultiplayerCommandExecute(object param)
        {
            ChangeView(typeof(TrisMultiplayerViewModel).Name);
        }
        #endregion
    }
}
