using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tx.ToolBox.Helpers;

namespace Tx.ToolBox.Settings
{
    public abstract class SettingsStorageBase : ISettingsStorage
    {
        public IObservable<TSettings> GetObservable<TSettings>(string id = null)
            where TSettings : class, new()
        {
            return GetContainer<TSettings>(id);
        }

        public TSettings GetSettings<TSettings>(string id = null)
            where TSettings : class, new()
        {
            return GetContainer<TSettings>(id).Settings;
        }

        public void SetSettings<TSettings>(TSettings settings, string id = null)
            where TSettings : class, new()
        {
            GetContainer<TSettings>(id).Set(settings);
        }

        public bool Contains<TSettings>(string id = null)
            where TSettings : class, new()
        {
            id = id ?? GetDefaultId(typeof(TSettings));
            lock (SettingsMap)
            {
                return SettingsMap.TryGetValue(id, out var container) && container.SettingsType == typeof(TSettings);
            }
        }

        public bool RemoveSettings<TSettings>(string id = null) where TSettings : class, new()
        {
            id = id ?? GetDefaultId(typeof(TSettings));
            return Remove(id);
        }

        public void CopyTo(ISettingsStorage otherStorage)
        {
            lock (SettingsMap)
            {
                foreach (var container in SettingsMap.Values)
                {
                    container.CopyTo(otherStorage);
                }
            }
        }

        public void Clear()
        {
            lock (SettingsMap)
            {
                SettingsMap.Values.AsDisposable().Dispose();
                SettingsMap.Clear();
            }
        }

        public abstract void Load();
        public abstract void Save();

        protected Dictionary<string, ISettingsContainer> SettingsMap = new Dictionary<string, ISettingsContainer>();

        protected ObservableSettings<TSettings> GetContainer<TSettings>(string id = null)
            where TSettings : class, new()
        {
            id = id ?? GetDefaultId(typeof(TSettings));
            return (ObservableSettings<TSettings>)GetContainerInternal(id, () => new ObservableSettings<TSettings>(new TSettings(), id));
        }

        protected ISettingsContainer GetContainer(Type settingsType, string id = null)
        {
            id = id ?? GetDefaultId(settingsType);
            return GetContainerInternal(id, () => (ISettingsContainer)Activator.CreateInstance(typeof(ObservableSettings<>).MakeGenericType(settingsType), id));
        }

        protected bool Remove(string id)
        {
            lock (SettingsMap)
            {
                if (!SettingsMap.TryGetValue(id, out var container)) return false;
                container.Dispose();
                SettingsMap.Remove(id);
                return true;
            }
        }

        private string GetDefaultId(Type type)
        {
            return type.FullName;
        }

        private ISettingsContainer GetContainerInternal(string id, Func<ISettingsContainer> factory)
        {
            lock (SettingsMap)
            {
                if (!SettingsMap.TryGetValue(id, out var container))
                {
                    SettingsMap[id] = container = factory();
                }
                return container;
            }
        }
    }
}
