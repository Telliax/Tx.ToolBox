using System;
using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Tx.ToolBox.Helpers;
using Tx.ToolBox.Wpf.Mvvm.Validation;

namespace Tx.ToolBox.Wpf.Mvvm
{
    public abstract class ValidationViewModel : ViewModel, INotifyDataErrorInfo
    {
        protected ValidationViewModel()
        {
            Validator = new Validator(this);
        }

        IEnumerable INotifyDataErrorInfo.GetErrors(string propertyName)
        {
            return Validator.GetErrors(propertyName);
        }

        bool INotifyDataErrorInfo.HasErrors => Validator.HasErrors;

        event EventHandler<DataErrorsChangedEventArgs> INotifyDataErrorInfo.ErrorsChanged
        {
            add => Validator.ErrorsChanged += value;
            remove => Validator.ErrorsChanged -= value;
        }

        public Validator Validator { get; }

        protected override void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            Validator.ValidateAsync(propertyName, RevalidationReason.PropertyChanged).Forget();
        }
    }
}
