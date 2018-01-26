using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Tx.ToolBox.Wpf.Tools.Buttons
{
    public class ToggleButtonToolView : ToggleButton
    {
        static ToggleButtonToolView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ToggleButtonToolView), new FrameworkPropertyMetadata(typeof(ToggleButtonToolView)));
        }
    }
}
