using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Tx.ToolBox.Helpers;
using Tx.ToolBox.Storage;
using Tx.ToolBox.Wpf.Mvvm;
using Tx.ToolBox.Wpf.Templates;
using Tx.ToolBox.Wpf.ToolBar.Tools;

namespace Tx.ToolBox.Wpf.ToolBar
{
    [Template(typeof(ToolBarView))]
    class ToolBarViewModel : ViewModelBase, IToolBar
    {
        public ToolBarViewModel() : this(new EmptyStorage())
        {
        }

        public ToolBarViewModel(IStorage toolStateStorage)
        {
            _storage = toolStateStorage ?? throw new ArgumentNullException(nameof(toolStateStorage));
        }

        public IReadOnlyList<ITool> Tools
        {
            get => _tools;
            private set
            {
                _tools = new ObservableCollection<ITool>(value);
                OnPropertyChanged();
            }
        }

        public IToolBarBuilder Setup()
        {
            return new ToolBarBuilder(this);
        }

        private readonly IStorage _storage;
        private ObservableCollection<ITool> _tools = new ObservableCollection<ITool>();

        private class ToolBarBuilder : IToolBarBuilder
        {
            public ToolBarBuilder(ToolBarViewModel toolBar)
            {
                _toolBar = toolBar;
                _tools = toolBar.Tools.ToList();
            }

            public IToolBarBuilder Add(params ITool[] tools)
            {
                tools.ForEach(t => t.LoadState(_toolBar._storage));
                _tools.AddRange(tools);
                return this;
            }

            public IToolBarBuilder Remove(params ITool[] tools)
            {
                tools.ForEach(t => t.SaveState(_toolBar._storage));
                _tools.RemoveAll(tools.Contains);
                return this;
            }

            public IToolBarBuilder Clear()
            {
                _tools.ForEach(t => t.SaveState(_toolBar._storage));
                _tools.Clear();
                return this;
            }

            public void Complete()
            {
                _toolBar.Tools = new ObservableCollection<ITool>(_tools);
            }

            private readonly ToolBarViewModel _toolBar;
            private readonly List<ITool> _tools;
        }
    }
}
