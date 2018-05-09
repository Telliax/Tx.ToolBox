using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Tx.ToolBox.Wpf.Tools.Misc
{
    public class SeparatorToolView : Separator
    {
        static SeparatorToolView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SeparatorToolView), new FrameworkPropertyMetadata(typeof(SeparatorToolView)));
        }
    }
}
