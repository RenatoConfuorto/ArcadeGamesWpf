using Core.Interfaces.Navigation;
using Core.Interfaces.ViewModels;
using Core.ViewModels;
using LIB.Attributes;
using LIB.Sounds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            InitBackgroundMusic();
        }
        public override void Dispose()
        {
            base.Dispose();
        }
        #endregion

        #region Private Methods
        private void InitBackgroundMusic()
        {
            BackgroundMusic attribute = this.GetType().GetCustomAttribute<BackgroundMusic>();
            if(attribute != null)
            {
                SoundsManagment.ChangeBackground(attribute.BackgroundMusicName);
            }
            else
            {
                SoundsManagment.ChangeBackground(String.Empty);
            }
        }
        #endregion

        #region Protected Methods
        #endregion
    }
}
