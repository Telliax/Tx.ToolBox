using System.Collections.Generic;

namespace Tx.ToolBox.Wpf.Tools
{
    interface IToolBar
    {
        IReadOnlyList<ITool> Tools { get; }
        IToolBarBuilder Setup();
    }
}
