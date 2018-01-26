using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tx.ToolBox.Wpf.Mvvm.Validation
{
    class PropertyValidator
    {
        public PropertyValidator(string property)
        {
            PropertyName = property;
        }

        public bool HasErrors => Rules.Any(r => r.Result.HasError);
        public string PropertyName { get; }
        public IEnumerable<ValidationRule> Rules => _asyncRules.Concat(_rules);
        public IEnumerable<string> Errors => Rules.Where(r => r.Result.HasError).Select(r => r.Result.Error);

        public Task AsyncHandle = Task.CompletedTask;

        public event Action<string> ErrorsChanged;

        public void AddRule(ValidationRule rule)
        {
            if (rule.IsAsync)
            {
                _asyncRules.Add(rule);
            }
            else
            {
                _rules.Add(rule);
            }
        }

        public void Validate(RevalidationReason reason)
        {
            if (_asyncRules.Count > 0)
            {
                AsyncHandle = AsyncHandle.ContinueWith(t =>
                {
                    Validate(_asyncRules, reason);
                });
            }
            Validate(_rules, reason);
        }

        private readonly List<ValidationRule> _rules = new List<ValidationRule>();
        private readonly List<ValidationRule> _asyncRules = new List<ValidationRule>();

        private void Validate(IEnumerable<ValidationRule> rules, RevalidationReason reason)
        {
            bool changed = false;
            foreach (var rule in rules)
            {
                if (reason < rule.Mode) continue;

                var hadError = rule.Result.HasError;
                rule.Validate();
                changed = hadError || rule.Result.HasError;
            }

            if (changed)
            {
                ErrorsChanged?.Invoke(PropertyName);
            }
        }
    }
}