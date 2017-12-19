using System.Collections;
using System.Collections.Generic;
using Tx.ToolBox.Wpf.ToolBar.Tools;

namespace Tx.ToolBox.Wpf.ToolBar
{
    interface IToolBar
    {
        IReadOnlyList<ITool> Tools { get; }
        IToolBarBuilder Setup();
    }
}
