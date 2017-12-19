using Tx.ToolBox.Wpf.ToolBar.Tools;

namespace Tx.ToolBox.Wpf.ToolBar
{
    internal interface IToolBarBuilder
    {
        IToolBarBuilder Add(params ITool[] tools);
        IToolBarBuilder Remove(params ITool[] tools);
        IToolBarBuilder Clear();
        void Complete();
    }
}