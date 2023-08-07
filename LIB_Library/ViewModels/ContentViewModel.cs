using Core.Interfaces.Navigation;
using Core.Interfaces.ViewModels;
using Core.ViewModels;
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
        public ContentViewModel(string viewName, string parentView = null, object param = null) : base(viewName, parentView, param)
        {
        }
        #endregion

        #region Override Methods
        protected override void OnInitialized()
        {
            base.OnInitialized();
        }
        public override void Dispose()
        {
            base.Dispose();
        }
        #endregion

        #region Private Methods
        #endregion

        #region Protected Methods
        #endregion
    }
}
