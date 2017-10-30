using System;
using System.Threading.Tasks;

namespace Tx.ToolBox.Settings
{
    public interface ISettingsStorage
    {
        TSettings GetSettings<TSettings>(string id = null)
            where TSettings : class, new();
        IObservable<TSettings> GetObservable<TSettings>(string id = null)
            where TSettings : class, new();
        void SetSettings<TSettings>(TSettings settings, string id = null)
            where TSettings : class, new();
        bool Contains<TSettings>(string id = null)
            where TSettings : class, new();
        bool RemoveSettings<TSettings>(string id = null)
            where TSettings : class, new();
        void Clear();

        void Save();
        void Load();

        void CopyTo(ISettingsStorage otherStorage);
    }
}
