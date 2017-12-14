using System;

namespace Tx.ToolBox.Storage
{
    public interface IDataStorage
    {
        TItem Get<TItem>(string id = null)
            where TItem : class, new();
        IObservable<TItem> GetObservable<TItem>(string id = null)
            where TItem : class, new();
        void Set<TItem>(TItem settings, string id = null)
            where TItem : class, new();
        bool Contains<TItem>(string id = null)
            where TItem : class, new();
        bool Remove<TItem>(string id = null)
            where TItem : class, new();
        void Clear();

        void Save();
        void Load();

        void CopyTo(IDataStorage otherStorage);
    }
}
