namespace Tx.ToolBox.Wpf.Mvvm
{
    public class SettingsViewModel : ValidationViewModel
    {
        public bool HasChanges
        {
            get => _hasChanges;
            set => SetField(ref _hasChanges, value);
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            HasChanges = true;
        }

        private bool _hasChanges;
    }
}
