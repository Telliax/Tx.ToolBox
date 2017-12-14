using System;
using System.Collections.Generic;
using Tx.ToolBox.Helpers;

namespace Tx.ToolBox.Storage
{
    public abstract class StorageBase : IDataStorage
    {
        public IObservable<TItem> GetObservable<TItem>(string id = null)
            where TItem : class, new()
        {
            return GetContainer<TItem>(id);
        }

        public TItem Get<TItem>(string id = null)
            where TItem : class, new()
        {
            return GetContainer<TItem>(id).Item;
        }

        public void Set<TItem>(TItem settings, string id = null)
            where TItem : class, new()
        {
            GetContainer<TItem>(id).Set(settings);
        }

        public bool Contains<TItem>(string id = null)
            where TItem : class, new()
        {
            id = id ?? GetDefaultId(typeof(TItem));
            lock (Items)
            {
                return Items.TryGetValue(id, out var container) && container.ItemType == typeof(TItem);
            }
        }

        public bool Remove<TItem>(string id = null) where TItem : class, new()
        {
            id = id ?? GetDefaultId(typeof(TItem));
            return Remove(id);
        }

        public void CopyTo(IDataStorage otherStorage)
        {
            lock (Items)
            {
                foreach (var container in Items.Values)
                {
                    container.CopyTo(otherStorage);
                }
            }
        }

        public void Clear()
        {
            lock (Items)
            {
                Items.Values.AsDisposable().Dispose();
                Items.Clear();
            }
        }

        public abstract void Load();
        public abstract void Save();

        protected Dictionary<string, IItemContainer> Items = new Dictionary<string, IItemContainer>();

        protected ObservableItem<TItem> GetContainer<TItem>(string id = null)
            where TItem : class, new()
        {
            id = id ?? GetDefaultId(typeof(TItem));
            return (ObservableItem<TItem>)GetContainerInternal(id, () => new ObservableItem<TItem>(new TItem(), id));
        }

        protected IItemContainer GetContainer(Type settingsType, string id = null)
        {
            id = id ?? GetDefaultId(settingsType);
            return GetContainerInternal(id, () => (IItemContainer)Activator.CreateInstance(typeof(ObservableItem<>).MakeGenericType(settingsType), id));
        }

        protected bool Remove(string id)
        {
            lock (Items)
            {
                if (!Items.TryGetValue(id, out var container)) return false;
                container.Dispose();
                Items.Remove(id);
                return true;
            }
        }

        private string GetDefaultId(Type type)
        {
            return type.FullName;
        }

        private IItemContainer GetContainerInternal(string id, Func<IItemContainer> factory)
        {
            lock (Items)
            {
                if (!Items.TryGetValue(id, out var container))
                {
                    Items[id] = container = factory();
                }
                return container;
            }
        }
    }
}
