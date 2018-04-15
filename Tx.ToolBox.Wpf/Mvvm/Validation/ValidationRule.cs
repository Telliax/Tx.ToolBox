using System;

namespace Tx.ToolBox.Wpf.Mvvm.Validation
{
    public class ValidationRule : ValidationRuleBase
    {
        public ValidationRule(Func<ValidationResult> validationDelegate, RevalidationReason mode)
            : base(mode)
        {
            _validationDelegate = validationDelegate;
        }

        public bool Validate(RevalidationReason reason)
        {
            if (reason < Mode) return false;
            var oldResult = Result;
            Result = _validationDelegate();
            return !oldResult.Equals(Result);
        }

        private readonly Func<ValidationResult> _validationDelegate;
    }
}