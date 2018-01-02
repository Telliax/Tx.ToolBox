using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interactivity;

namespace Tx.ToolBox.Wpf.Behaviors
{
    class FadeOutOnDisabled : Behavior<UIElement>
    {
        public static readonly DependencyProperty DisabledOpacityProperty = DependencyProperty.Register(
            "DisabledOpacity", typeof(double), typeof(FadeOutOnDisabled), new PropertyMetadata(0.5));
        public double DisabledOpacity
        {
            get { return (double) GetValue(DisabledOpacityProperty); }
            set { SetValue(DisabledOpacityProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            var binding = new MultiBinding()
            {
                Bindings =
                {
                    new Binding("IsEnabled") { Source = AssociatedObject },
                    new Binding("DisabledOpacity") { Source = this }
                },
                Mode = BindingMode.OneWay,
                Converter = new OpacityConverter()
            };
            BindingOperations.SetBinding(AssociatedObject, UIElement.OpacityProperty, binding);
        }

        protected override void OnDetaching()
        {
            BindingOperations.ClearBinding(AssociatedObject, UIElement.OpacityProperty);
            base.OnDetaching();
        }

        private class OpacityConverter : IMultiValueConverter
        {
            public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
            {
                if (values[0] is bool isEnabled && values[1] is double disabledOpacity)
                {
                    return isEnabled ? 1.0 : disabledOpacity;
                }
                return DependencyProperty.UnsetValue;
            }

            public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }
    }
}
