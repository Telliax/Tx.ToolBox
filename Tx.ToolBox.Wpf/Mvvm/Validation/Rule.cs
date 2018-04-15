using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Tx.ToolBox.Wpf.Mvvm.Validation
{
    public interface IRuleBuilder
    {
        IEnumerable<(string Property, ValidationRuleBase Rule)> BuildRules();
    }
    public class Rule : IRuleBuilder
    {
        public static Rule For(params string[] propertyNames)
        {
            if (!propertyNames.Any())
                throw new ArgumentException("Specify at least one property name.", nameof(propertyNames));
            var rule = new Rule{_properties = propertyNames };
            return rule;
        }

        public Rule Check(Func<ValidationResult> validationRule)
        {
            _rules.Add(validationRule);
            return this;
        }

        public Rule CheckAsync(Func<CancellationToken, ValidationResult> validationRule)
        {
            _asyncRules.Add(t=> Task.Run(() => validationRule(t)));
            return this;
        }

        public Rule CheckAsync(Func<CancellationToken, Task<ValidationResult>> validationRule)
        {
            _asyncRules.Add(validationRule);
            return this;
        }

        public Rule WhenPropertyChanged()
        {
            _mode = RevalidationReason.PropertyChanged;
            return this;
        }

        public Rule WhenExplicitlyRequested()
        {
            _mode = RevalidationReason.ExplicitlyCalled;
            return this;
        }

        private string[] _properties;
        private readonly List<Func<ValidationResult>> _rules = new List<Func<ValidationResult>>();
        private readonly List<Func<CancellationToken, Task<ValidationResult>>> _asyncRules = new List<Func<CancellationToken, Task<ValidationResult>>>();
        private RevalidationReason _mode = RevalidationReason.PropertyChanged;

        private Rule()
        {
        }

        IEnumerable<(string Property, ValidationRuleBase Rule)> IRuleBuilder.BuildRules()
        {
            foreach (var property in _properties)
            {
                foreach (var rule in _rules)
                {
                    yield return (property, new ValidationRule(rule, _mode));
                }

                foreach (var rule in _asyncRules)
                {
                    yield return (property, new AsyncValidationRule(rule, _mode));
                }
            }
        }
    }
}