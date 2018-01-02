using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;

namespace Tx.ToolBox.Wpf.Behaviors
{
    class CollapseOnEmptyImage : Behavior<Image>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            var binding = new Binding("Source")
            {
                Path = new PropertyPath(Image.SourceProperty),
                Source = AssociatedObject,
                Mode = BindingMode.OneWay
            };
            BindingOperations.SetBinding(this, ImageSourceProperty, binding);
        }

        protected override void OnDetaching()
        {
            BindingOperations.ClearBinding(this, ImageSourceProperty);
            base.OnDetaching();
        }

        private static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register(
            "ImageSource", typeof(object), typeof(CollapseOnEmptyImage), new PropertyMetadata(new object(), (s, e) => ((CollapseOnEmptyImage)s).OnImageChanged(e.NewValue)));

        private void OnImageChanged(object image)
        {
            AssociatedObject.Visibility = image == null ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}
