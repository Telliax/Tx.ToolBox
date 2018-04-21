using System;
using System.Collections.ObjectModel;
using System.Linq;
using Tx.ToolBox.Helpers;
using Tx.ToolBox.Wpf.Templates;

namespace Tx.ToolBox.Wpf.Tools.Drop
{
    [Template(typeof(ComboBoxToolView))]
    public abstract class ComboBoxTool<T> : ToolBase
    {
        public T SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged();
                try
                {
                    OnSelectedItemChanged();
                }
                catch (Exception e)
                {
                    new AggregateException(e).RethrowOnThreadPool();
                }
            }
        }        

        public ObservableCollection<T> Items
        {
            get => _items;
            private set => SetField(ref _items, value);
        }

        public double Width
        {
            get => _width;
            set => SetField(ref _width, value);
        }

        public void SetOptions(params T[] options)
        {
            Items = new ObservableCollection<T>(options);
        }

        protected abstract void OnSelectedItemChanged();

        protected override void LoadDefaultState()
        {
            SelectedItem = Items.FirstOrDefault();
        }

        protected override object GetState()
        {
            return Items.IndexOf(SelectedItem);
        }

        protected override void SetState(object state)
        {
            var index = (int) state;
            if (index < 0 || index >= Items.Count) return;
            SelectedItem = Items[index];
        }

        private ObservableCollection<T> _items = new ObservableCollection<T>();
        private T _selectedItem;
        private double _width = 100;
    }
}
