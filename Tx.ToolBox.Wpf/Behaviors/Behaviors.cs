using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Markup;

namespace Tx.ToolBox.Wpf.Behaviors
{
    public class Behaviors
    {
        public static readonly DependencyProperty SourceProperty = DependencyProperty.RegisterAttached(
            "Source", typeof(ArrayList), typeof(Behaviors), new PropertyMetadata(new ArrayList(), OnSourceChanged));
        public static void SetSource(DependencyObject element, ArrayList value) => element.SetValue(SourceProperty, value);
        public static ArrayList GetSource(DependencyObject element) => (ArrayList) element.GetValue(SourceProperty);

        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var array = (ArrayList)e.NewValue;
            var behaviors = Interaction.GetBehaviors(d);
            behaviors.Clear();
            foreach (Behavior behavior in array)
            {
                behaviors.Add(behavior);
            }
        }
    }
}
