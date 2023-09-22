using Core.Dependency;
using Core.Interfaces.ViewModels;
using Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Unity;
using Unity.Resolution;

namespace Core.Views
{
    /// <summary>
    /// Logica di interazione per PopUp.xaml
    /// </summary>
    public partial class PopUp : Window
    {
        private object popResult;
        public IViewModelBase viewModel { get; set; }
        public PopUp(string ViewName, object param = null)
        {
            IUnityContainer container = UnityHelper.Current.GetLocalContainer();
            if(param == null)
            {
                viewModel = container.Resolve<IViewModelBase>(ViewName);
            }
            else
            {
                viewModel = container.Resolve<IViewModelBase>(ViewName, new ParameterOverride("param", param));
            }
            this.DataContext = viewModel;
            ((PopupViewModelBase)viewModel).closePopup += ClosePopup;
            InitializeComponent();
        }

        private void ClosePopup(object popResult)
        {
            this.popResult = popResult;
            this.Close();
        }

        public new object Show()
        {
            this.ShowDialog();
            return popResult;
        }
    }
}
