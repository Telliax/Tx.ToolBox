using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tx.ToolBox.Storage
{
    public class EmptyStorage : IStorage
    {
        public TItem Get<TItem>(string id = null) where TItem : class, new()
        {
            return new TItem();
        }

        public IObservable<TItem> GetObservable<TItem>(string id = null) where TItem : class, new()
        {
            return new ObservableItem<TItem>(new TItem(), id ?? typeof(TItem).FullName);
        }

        public void Set<TItem>(TItem settings, string id = null) where TItem : class, new()
        {
        }

        public bool Contains<TItem>(string id = null) where TItem : class, new()
        {
            return false;
        }

        public bool Remove<TItem>(string id = null) where TItem : class, new()
        {
            return false;
        }

        public void Clear()
        {
        }

        public void Save()
        {
        }

        public void Load()
        {
        }

        public void CopyTo(IStorage otherStorage)
        {
        }
    }
}
