using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Tx.ToolBox.Wpf.Behaviors
{
    class UpdateTextSourceOnEnter : Behavior<TextBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewKeyDown += OnKeyPressed;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.PreviewKeyDown -= OnKeyPressed;
            base.OnDetaching();
        }

        private void OnKeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;

            var binding = BindingOperations.GetBindingExpression(AssociatedObject, TextBox.TextProperty);
            binding?.UpdateSource();
        }
    }
}
