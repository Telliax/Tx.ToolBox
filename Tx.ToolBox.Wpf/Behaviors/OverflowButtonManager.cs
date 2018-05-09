using System.Windows;
using System.Windows.Controls;

namespace Tx.ToolBox.Wpf.Behaviors
{
    public class OverflowButtonManager : BehaviorOnLoaded<ToolBar>
    {
        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(
            "Mode", typeof(OverflowButtonMode), typeof(OverflowButtonManager), 
            new PropertyMetadata(OverflowButtonMode.Visible, (s, a) => ((OverflowButtonManager)s).UpdateOverflow()));
        public OverflowButtonMode Mode
        {
            get => (OverflowButtonMode) GetValue(ModeProperty);
            set => SetValue(ModeProperty, value);
        }

        protected override void Load()
        {
            AssociatedObject.SizeChanged += OnToolbarSizeChanged;
            UpdateOverflow();
        }

        protected override void Unload()
        {
            AssociatedObject.SizeChanged -= OnToolbarSizeChanged;
            SetVisibility(Visibility.Visible);
        }

        private void UpdateOverflow()
        {
            if (AssociatedObject == null) return;

            if (Mode == OverflowButtonMode.Visible)
            {
                SetVisibility(Visibility.Visible);
            }
            else if (Mode == OverflowButtonMode.Collapsed)
            {
                SetVisibility(Visibility.Collapsed);
            }
            else
            {
                var visibility = AssociatedObject.HasOverflowItems ? Visibility.Visible : Visibility.Collapsed;
                SetVisibility(visibility);
            }
        }

        private void OnToolbarSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (Mode != OverflowButtonMode.Auto) return;
            UpdateOverflow();
        }

        private void SetVisibility(Visibility visibility)
        {
            if (AssociatedObject.Template.FindName("OverflowGrid", AssociatedObject) is FrameworkElement overflowGrid)
            {
                overflowGrid.Visibility = visibility;
            }
            if (visibility == Visibility.Visible)
            {
                var border = AssociatedObject.Orientation == Orientation.Horizontal
                    ? new Thickness(0, 0, 11, 0)
                    : new Thickness(0, 0, 0, 11);
                SetBorder(border);
            }
            else
            {
                SetBorder(new Thickness(0));
            }

            void SetBorder(Thickness border)
            {
                if (AssociatedObject.Template.FindName("MainPanelBorder", AssociatedObject) is FrameworkElement mainPanelBorder)
                {
                    mainPanelBorder.Margin = border;
                }
            }
        }
    }

    public enum OverflowButtonMode
    {
        Visible,
        Collapsed,
        Auto
    }
}
