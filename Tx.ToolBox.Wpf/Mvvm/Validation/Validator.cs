using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tx.ToolBox.Wpf.Mvvm.Validation
{
    public class Validator
    {
        public event Action<string> ErrorsChanged;

        public bool HasErrors => _validators.Values.Any(v => v.HasErrors);

        public IEnumerable<string> GetErrors(string propertyName)
        {
            if (!_validators.TryGetValue(propertyName, out var validator)) return Enumerable.Empty<string>();
            return validator.Errors;
        }

        public IEnumerable<(string Property, string Error)> GetAllErrors()
        {
            return _validators.Values.SelectMany(v => v.Errors.Select(e => (v.PropertyName, e)));
        }

        public Task AsyncValidation
        {
            get
            {
                return Task.WhenAll(_validators.Values.Select(v => v.AsyncHandle).ToArray());
            }
        }

        public void AddRule(Rule ruleExpression)
        {
            var builder = ruleExpression as IRuleBuilder;
            foreach (var rule in builder.BuildRules())
            {
                if (!_validators.TryGetValue(rule.Property, out var validator))
                {
                    _validators[rule.Property] = validator = new PropertyValidator(rule.Property);
                    validator.ErrorsChanged += OnErrorsChanged;
                }
                validator.AddRule(rule.Rule);
            }
        }

        public void Validate(string propertyName, RevalidationReason reason = RevalidationReason.ExplicitlyCalled)
        {
            if (!_validators.TryGetValue(propertyName, out var validator)) return;
            validator.Validate(reason);
        }

        public void ValidateAll(RevalidationReason reason = RevalidationReason.ExplicitlyCalled)
        {
            foreach (var validator in _validators.Values)
            {
                validator.Validate(reason);
            }
        }

        private readonly Dictionary<string, PropertyValidator> _validators = new Dictionary<string, PropertyValidator>();
        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(propertyName);
        }
    }
}
