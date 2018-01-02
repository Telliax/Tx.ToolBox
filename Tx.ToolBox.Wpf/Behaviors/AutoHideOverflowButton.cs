using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Tx.ToolBox.Wpf.Behaviors
{
    public class AutoHideOverflowButton : BehaviorOnLoaded<ToolBar>
    {
        protected override void Load()
        {
            AssociatedObject.SizeChanged += OnToolbarSizeChanged;
            UpdateOverflow();
        }

        protected override void Unload()
        {
            AssociatedObject.SizeChanged -= OnToolbarSizeChanged;
            AssociatedObject.ShowOverflow();
        }

        private void UpdateOverflow()
        {
            if (AssociatedObject.HasOverflowItems)
            {
                AssociatedObject.ShowOverflow();
            }
            else
            {
                AssociatedObject.HideOverflow();
            }
        }

        private void OnToolbarSizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateOverflow();
        }

    }

    public static class ToolBarEx
    {
        public static void HideOverflow(this ToolBar toolbar)
        {
            toolbar.SetOverflowVisibility(Visibility.Collapsed);
            toolbar.SetOverflowBorder(new Thickness(0));
        }

        public static void ShowOverflow(this ToolBar toolbar)
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
