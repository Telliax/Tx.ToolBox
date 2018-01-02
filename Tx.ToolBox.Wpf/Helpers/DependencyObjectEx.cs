using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Tx.ToolBox.Wpf.Helpers
{
    /// <summary>
    /// A bunch of helper and extension methods for WPF
    /// </summary>
    public static class DependencyObjectEx
    {
        public static TFreezable ToFrozen<TFreezable>(this TFreezable freezable)
            where TFreezable : Freezable
        {
            freezable.Freeze();
            return freezable;
        }

        public static TChild FindVisualChild<TChild>(this DependencyObject parent, string name = null)
            where TChild : DependencyObject
        {
            if (parent == null) return null;
            var queue = new Queue<DependencyObject>();
            queue.Enqueue(parent);
            while (queue.Any())
            {
                var current = queue.Dequeue();
                var count = VisualTreeHelper.GetChildrenCount(current);
                for (int i = 0; i < count; i++)
                {
                    var child = VisualTreeHelper.GetChild(current, i);
                    if (child is TChild result)
                    {
                        if (name == null) return result;
                        if (child is FrameworkElement element && element.Name == name) return result;
                    }
                    queue.Enqueue(child);
                }
            }
            return null;
        }
    }
}
