using System;
using System.Windows;
using System.Windows.Controls;

namespace Tx.ToolBox.Wpf.Behaviors
{
    public static class ToolBarEx
    {
        public static readonly DependencyProperty AutoHideOverflowProperty = DependencyProperty.RegisterAttached("AutoHideOverflow", typeof(bool), typeof(ToolBarEx), new PropertyMetadata(false, OnAutoHideOverflowChanged));
        public static void SetAutoHideOverflow(DependencyObject element, bool value)
        {
            element.SetValue(AutoHideOverflowProperty, value);
        }

        private static void OnAutoHideOverflowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ToolBar toolbar)
            {
                if ((bool)e.NewValue)
                {
                    toolbar.SizeChanged += OnToolbarSizeChanged;
                }
                else
                {
                    toolbar.SizeChanged -= OnToolbarSizeChanged;
                    toolbar.ShowOverflow();
                }
            }
            else
            {
                throw new NotSupportedException(d.GetType().FullName);
            }
        }

        private static void OnToolbarSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var toolbar = (ToolBar)sender;
            if (toolbar.HasOverflowItems)
            {
                toolbar.ShowOverflow();
            }
            else
            {
                toolbar.HideOverflow();
            }
        }

        private static void HideOverflow(this ToolBar toolbar)
        {
            toolbar.SetOverflowVisibility(Visibility.Collapsed);
            toolbar.SetOverflowBorder(new Thickness(0));
        }

        private static void ShowOverflow(this ToolBar toolbar)
        {
            toolbar.SetOverflowVisibility(Visibility.Visible);
            toolbar.SetOverflowBorder(toolbar.Orientation == Orientation.Horizontal
                                            ? new Thickness(0, 0, 11, 0)
                                            : new Thickness(0, 0, 0, 11));
        }

        private static void SetOverflowVisibility(this ToolBar toolbar, Visibility visibility)
        {
            if (toolbar.Template.FindName("OverflowGrid", toolbar) is FrameworkElement overflowGrid)
            {
                overflowGrid.Visibility = visibility;
            }
        }

        private static void SetOverflowBorder(this ToolBar toolbar, Thickness border)
        {
            if (toolbar.Template.FindName("MainPanelBorder", toolbar) is FrameworkElement mainPanelBorder)
            {
                mainPanelBorder.Margin = border;
            }
        }
    }
}
