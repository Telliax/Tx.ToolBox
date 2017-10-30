using System;

namespace Tx.ToolBox.Settings
{
    public interface ISettingsContainer : IDisposable
    {
        string Id { get; }
        object Settings { get; }
        Type SettingsType { get; }
        void Set(object settings);
        void CopyTo(ISettingsStorage otherStorage);
    }
}
