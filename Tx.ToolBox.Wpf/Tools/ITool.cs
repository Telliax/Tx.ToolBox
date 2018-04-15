using Tx.ToolBox.Storage;

namespace Tx.ToolBox.Wpf.Tools
{
    public interface ITool
    {
        string Id { get; }
        bool IsVisible { get; set; }
        bool IsEnabled { get; set; }
        
        void SaveState(IStorage settingsStorage);
        void LoadState(IStorage settingsStorage);
    }
}
