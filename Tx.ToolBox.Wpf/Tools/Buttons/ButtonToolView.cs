using System.Windows;

namespace Tx.ToolBox.Wpf.Tools.Buttons
{
    public class ButtonToolView : System.Windows.Controls.Button
    {
        static ButtonToolView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ButtonToolView), new FrameworkPropertyMetadata(typeof(ButtonToolView)));
        }

        public static readonly DependencyProperty HideImageProperty = DependencyProperty.Register(
            "HideImage", typeof(bool), typeof(ButtonToolView), new PropertyMetadata(false));
        public bool HideImage
        {
            get { return (bool) GetValue(HideImageProperty); }
            set { SetValue(HideImageProperty, value); }
        }
    }
}