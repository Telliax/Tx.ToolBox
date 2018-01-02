using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;

namespace Tx.ToolBox.Wpf.Behaviors
{
    class CollapseOnEmptyText : Behavior<TextBlock>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            var binding = new Binding("Text")
            {
                Source = AssociatedObject,
                Mode = BindingMode.OneWay
            };
            BindingOperations.SetBinding(this, TextProperty, binding);
        }

        protected override void OnDetaching()
        {
            BindingOperations.ClearBinding(this, TextProperty);
            base.OnDetaching();
        }

        private static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(CollapseOnEmptyText), new PropertyMetadata(null, (s,e) => ((CollapseOnEmptyText)s).OnTextChanged((string)e.NewValue)));

        private void OnTextChanged(string text)
        {
            AssociatedObject.Visibility = String.IsNullOrEmpty(text) ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}
