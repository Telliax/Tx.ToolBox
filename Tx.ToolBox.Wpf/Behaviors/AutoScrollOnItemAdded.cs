using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Tx.ToolBox.Wpf.Helpers;

namespace Tx.ToolBox.Wpf.Behaviors
{
    public class AutoScrollOnItemAdded : BehaviorOnLoaded<ItemsControl>
    {
        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(
            "Mode", typeof(AutoScrollMode), typeof(AutoScrollOnItemAdded), new PropertyMetadata(AutoScrollMode.VerticalWhenInactive));
        public AutoScrollMode Mode
        {
            get => (AutoScrollMode) GetValue(ModeProperty);
            set => SetValue(ModeProperty, value);
        }

        protected override void Load()
        {
            var binding = new Binding("ItemsSource.Count")
            {
                Source = AssociatedObject,
                Mode = BindingMode.OneWay
            };
            BindingOperations.SetBinding(this, ItemsCountProperty, binding);
            _scroll = AssociatedObject.FindVisualChild<ScrollViewer>() ?? throw new NotSupportedException("ScrollViewer was not found!");
        }

        protected override void Unload()
        {
            BindingOperations.ClearBinding(this, ItemsCountProperty);
        }

        private static readonly DependencyProperty ItemsCountProperty = DependencyProperty.Register(
            "ItemsCount", typeof(int), typeof(AutoScrollOnItemAdded), new PropertyMetadata(0, (s, e) => ((AutoScrollOnItemAdded)s).OnCountChanged()));
        private ScrollViewer _scroll;

        private void OnCountChanged()
        {
            var mode = Mode;
            if (mode == AutoScrollMode.Vertical)
            {
                _scroll.ScrollToBottom();
            }
            else if (mode == AutoScrollMode.Horizontal)
            {
                _scroll.ScrollToRightEnd();
            }
            else if (mode == AutoScrollMode.VerticalWhenInactive)
            {
                if (_scroll.IsKeyboardFocusWithin) return;
                _scroll.ScrollToBottom();
            }
            else if (mode == AutoScrollMode.HorizontalWhenInactive)
            {
                if (_scroll.IsKeyboardFocusWithin) return;
                _scroll.ScrollToRightEnd();
            }
        }
    }

    public enum AutoScrollMode
    {
        /// <summary>
        /// No auto scroll
        /// </summary>
        Disabled,
        /// <summary>
        /// Automatically scrolls horizontally, but only if items control has no keyboard focus
        /// </summary>
        HorizontalWhenInactive,
        /// <summary>
        /// Automatically scrolls vertically, but only if itmes control has no keyboard focus
        /// </summary>
        VerticalWhenInactive,
        /// <summary>
        /// Automatically scrolls horizontally regardless of where the focus is
        /// </summary>
        Horizontal,
        /// <summary>
        /// Automatically scrolls vertically regardless of where the focus is
        /// </summary>
        Vertical
    }
}
