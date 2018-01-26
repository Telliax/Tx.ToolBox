using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tx.ToolBox.Wpf.Tools.Buttons
{
    abstract class ToggleButtonTool : ButtonTool
    {
        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                if (IsChecked == value) return;
                SetIsChecked(value);
            } 
        }

        protected override void Execute()
        {
        }

        protected abstract void OnIsChekedChanged();

        protected override void SetState(object state)
        {
            SetIsChecked((bool)state);
        }

        protected override object GetState()
        {
            return _isChecked;
        }

        protected override void SetDefaultState()
        {
            SetIsChecked(false);
        }

        protected void SetIsChecked(bool value)
        {
            _isChecked = value;
            OnPropertyChanged(nameof(IsChecked));
            OnIsChekedChanged();
        }

        private bool _isChecked;


    }
}
