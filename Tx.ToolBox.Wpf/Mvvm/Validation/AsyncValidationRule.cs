using System;
using System.Threading;
using System.Threading.Tasks;

namespace Tx.ToolBox.Wpf.Mvvm.Validation
{
    public class AsyncValidationRule : ValidationRuleBase
    {
        public AsyncValidationRule(Func<CancellationToken, Task<ValidationResult>> validationDelegate, RevalidationReason mode)
            : base(mode)
        {
            _validationDelegate = validationDelegate;
        }

        public async Task<bool> ValidateAsync(CancellationToken token, RevalidationReason reason)
        {
            if (reason < Mode) return false;
            var oldResult = Result;
            var res = await _validationDelegate(token);
            if (token.IsCancellationRequested) return false;
            Result = res;
            return !oldResult.Equals(Result);
        }

        private readonly Func<CancellationToken, Task<ValidationResult>> _validationDelegate;
    }
}