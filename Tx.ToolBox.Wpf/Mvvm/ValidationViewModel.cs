using System;
using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Tx.ToolBox.Wpf.Mvvm.Validation;

namespace Tx.ToolBox.Wpf.Mvvm
{
    public abstract class ValidationViewModel : ViewModel, INotifyDataErrorInfo
    {
        protected ValidationViewModel()
        {
            Validator = new Validator();
            Validator.ErrorsChanged += OnErrorsChanged;
        }

        public virtual IEnumerable GetErrors(string propertyName)
        {
            return Validator.GetErrors(propertyName);
        }

        public virtual bool HasErrors => Validator.HasErrors;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public virtual async Task ValidateAsync()
        {
            Validator.ValidateAll();
            await Validator.AsyncValidation;
        }

        protected Validator Validator { get; }

        protected override void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (String.IsNullOrEmpty(propertyName))
            {
                Validator.ValidateAll(RevalidationReason.PropertyChanged);
            }
            else
            {
                Validator.Validate(propertyName, RevalidationReason.PropertyChanged);
            }
        }

        protected void OnErrorsChanged([CallerMemberName]string propertyName = null)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
    }
}
