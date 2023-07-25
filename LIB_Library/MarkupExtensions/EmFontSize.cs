using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows;

namespace LIB.MarkupExtensions
{
    [MarkupExtensionReturnType(typeof(double))]
    public class EmFontSize : MarkupExtension
    {
        public EmFontSize() { }

        public EmFontSize(double size)
        {
            Size = size;
        }

        [ConstructorArgument("size")]
        public double Size { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
                return null;

            // get the target of the extension from the IServiceProvider interface
            IProvideValueTarget ipvt = (IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget));
            if (ipvt.TargetObject.GetType().FullName == "System.Windows.SharedDp")
                return this;

            DependencyObject targetObject = ipvt.TargetObject as DependencyObject;

            var window = TryFindParent<Window>(targetObject);
            if (window != null)
            {
                return window.FontSize * Size;
            }
            return 12 * Size;
        }

        public static T TryFindParent<T>(DependencyObject child) where T : DependencyObject
        {
            //get parent item
            DependencyObject parentObject = GetParentObject(child);

            //we've reached the end of the tree
            if (parentObject == null) return null;

            //check if the parent matches the type we're looking for
            T parent = parentObject as T;
            if (parent != null)
            {
                return parent;
            }
            else
            {
                //use recursion to proceed with next level
                return TryFindParent<T>(parentObject);
            }
        }

        public static DependencyObject GetParentObject(DependencyObject child)
        {
            if (child == null) return null;

            //handle content elements separately
            ContentElement contentElement = child as ContentElement;
            if (contentElement != null)
            {
                DependencyObject parent = ContentOperations.GetParent(contentElement);
                if (parent != null) return parent;

                FrameworkContentElement fce = contentElement as FrameworkContentElement;
                return fce != null ? fce.Parent : null;
            }

            //also try searching for parent in framework elements (such as DockPanel, etc)
            FrameworkElement frameworkElement = child as FrameworkElement;
            if (frameworkElement != null)
            {
                DependencyObject parent = frameworkElement.Parent;
                if (parent != null) return parent;
            }

            //if it's not a ContentElement/FrameworkElement, rely on VisualTreeHelper
            return VisualTreeHelper.GetParent(child);
        }
    }
}
