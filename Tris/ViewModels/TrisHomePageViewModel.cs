using LIB.Attributes;
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
        #endregion

        #region Constructor
        public TrisHomePageViewModel(INavigationService navService) : base(navService)
        {
        }
        #endregion

        #region Override Methods
        #endregion

        #region Private Methods
        #endregion
    }
}
