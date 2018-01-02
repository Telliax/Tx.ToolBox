﻿namespace Tx.ToolBox.Wpf.Tools
{
    public interface IToolBarBuilder
    {
        IToolBarBuilder Add(params ITool[] tools);
        IToolBarBuilder Remove(params ITool[] tools);
        IToolBarBuilder Clear();
        void Complete();
    }
}