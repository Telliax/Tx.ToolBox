using System;

namespace Tx.ToolBox.Wpf.Mvvm.Validation
{
    public class ValidationRule
    {
        public ValidationRule(Func<ValidationResult> validationDelegate, RevalidationReason mode, bool isAsync)
        {
            _validationDelegate = validationDelegate;
            Mode = mode;
            IsAsync = isAsync;
        }

        public RevalidationReason Mode { get; }
        public bool IsAsync { get; }
        public ValidationResult Result { get; private set; } = ValidationResult.ValidResult;

        public void Validate()
        {
            Result = _validationDelegate();
        }

        private readonly Func<ValidationResult> _validationDelegate;
    }
}