using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;

namespace Tx.ToolBox.Wpf.Behaviors
{
    public abstract class BehaviorOnLoaded<T> : Behavior<T> 
        where T : FrameworkElement
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            if (AssociatedObject.IsLoaded)
            {
                Load();
            }
            AssociatedObject.Loaded += OnLoaded;
            AssociatedObject.Unloaded += OnUnloaded;
        }

        protected override void OnDetaching()
        {
            if (AssociatedObject != null)
            {
                AssociatedObject.Loaded -= OnLoaded;
                AssociatedObject.Unloaded -= OnUnloaded;
                if (AssociatedObject.IsLoaded)
                {
                    Unload();
                }
            }
            base.OnDetaching();
        }

        protected abstract void Load();

        protected abstract void Unload();

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Load();
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            Unload();
        }
    }
}
