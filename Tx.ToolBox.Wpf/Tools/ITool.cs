﻿using Tx.ToolBox.Storage;

namespace Tx.ToolBox.Wpf.Tools
{
    public interface ITool
    {
        string Id { get; }
        
        void SaveState(IStorage settingsStorage);
        void LoadState(IStorage settingsStorage);
    }
}
