using Tx.ToolBox.Wpf.Templates;

namespace Tx.ToolBox.Wpf.Tools.Buttons
{
    [Template(typeof(ToggleButtonToolView))]
    abstract class ToggleButtonTool : ButtonTool
    {
        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                _isChecked = value;
                OnPropertyChanged();
                OnIsChekedChanged();
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

        protected override void SetDefaultState()
        {
            IsChecked = false;
        }

        private bool _isChecked;
    }
}
