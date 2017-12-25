using System;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace Tx.ToolBox.Wpf
{
    /// <summary>
    /// A bunch of helper and extension methods for WPF
    /// </summary>
    public static class WpfHelper
    {
        public static TFreezable ToFrozen<TFreezable>(this TFreezable freezable)
            where TFreezable : Freezable
        {
            freezable.Freeze();
            return freezable;
        }

        public static TChild FindVisualChild<TChild>(this DependencyObject parent)
            where TChild : DependencyObject
        {
            TChild result = null;
            if (parent != null)
            {
                var count = VisualTreeHelper.GetChildrenCount(parent);
                for (int i = 0; i < count; i++)
                {
                    var child = VisualTreeHelper.GetChild(parent, i);
                    result = child as TChild ?? child.FindVisualChild<TChild>();
                    if (result == null) break;
                }
            }
            return result;
        }

        /// <summary>
        /// Wrapper around regular BeginInvoke that takes Action instead of a Delegate.
        /// </summary>
        /// <param name="dispatcher">Dispatcher</param>
        /// <param name="action">Action to invoke</param>
        /// <param name="priority">Priority</param>
        /// <returns>Awaitable result.</returns>
        public static DispatcherOperation BeginInvoke(this Dispatcher dispatcher, Action action, DispatcherPriority priority = DispatcherPriority.Normal)
        {
            return dispatcher.BeginInvoke(action, priority);
        }

        /// <summary>
        /// Waits for Dispatcher.CurrentDispatcher to process its queue.
        /// </summary>
        /// <param name="priority">Dispatcher will process message of this or higher priority.</param>
        /// <remarks>
        /// This method will work even on background thread and even if you did not launch dipatcher with Dispatcher.Run().
        /// However it will affect that thread's dispatcher, and not UI thread's dispatcher.
        /// </remarks>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static void DoEvents(DispatcherPriority priority = DispatcherPriority.Background)
        {
            var frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(priority, new DispatcherOperationCallback(ExitFrame), frame);
            Dispatcher.PushFrame(frame);
        }

        private static object ExitFrame(object frame)
        {
            ((DispatcherFrame)frame).Continue = false;
            return null;
        }
    }
}
