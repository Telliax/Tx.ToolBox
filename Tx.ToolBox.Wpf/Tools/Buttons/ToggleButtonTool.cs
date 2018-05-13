using System;
using Tx.ToolBox.Helpers;
using Tx.ToolBox.Wpf.Templates;

namespace Tx.ToolBox.Wpf.Tools.Buttons
{
    [Template(typeof(ToggleButtonToolView))]
    public abstract class ToggleButtonTool : ButtonTool
    {
        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                _isChecked = value;
                OnPropertyChanged();
                try
                {
                    OnIsChekedChanged();
                }
                catch (Exception e)
                {
                    //Wpf tends to swallow exceptions thrown during databinding update
                    //this makes sure that exception does not go unnoticed
                    new AggregateException(e).RethrowOnThreadPool();
                }
            } 
        }

        protected override void Execute()
        {
        }

        protected abstract void OnIsChekedChanged();

        protected override void SetState(object state)
        {
            IsChecked = (bool)state;
        }

        protected override object GetState()
        {
            return _isChecked;
        }

        protected override void LoadDefaultState()
        {
            IsChecked = false;
        }

        private bool _isChecked;
    }
}
