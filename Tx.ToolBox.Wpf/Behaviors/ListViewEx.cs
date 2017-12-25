using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Tx.ToolBox.Wpf.Behaviors
{
    public static class ListViewEx 
    {
        public static readonly DependencyProperty AutoScrollProperty = DependencyProperty.RegisterAttached(
            "AutoScroll", typeof(bool), typeof(ListViewEx), new PropertyMetadata(false, OnAutoScrollChanged));
        public static void SetAutoScroll(DependencyObject element, bool value)
        {
            element.SetValue(AutoScrollProperty, value);
        }

        private static readonly DependencyProperty ItemsCountProperty = DependencyProperty.RegisterAttached(
            "ItemsCount", typeof(int), typeof(ListViewEx), new PropertyMetadata(0, OnCountChanged));
        private static void SetItemsCount(DependencyObject element, int value)
        {
            element.SetValue(ItemsCountProperty, value);
        }
        private static int GetItemsCount(DependencyObject element)
        {
            return (int) element.GetValue(ItemsCountProperty);
        }

        private static void OnAutoScrollChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var itemsControl = d as ItemsControl ?? throw new NotSupportedException("Only ItemsControl and its descendants are suppoerted.");
            if ((bool) e.NewValue)
            {
                var binding = new Binding("ItemsSource.Count")
                {
                    Source = itemsControl,
                    Mode = BindingMode.OneWay
                };
                itemsControl.SetBinding(ListViewEx.ItemsCountProperty, binding);
            }
            else
            {
                BindingOperations.ClearBinding(itemsControl, ListViewEx.ItemsCountProperty);
            }
        }

        private static void OnCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }
    }
}
