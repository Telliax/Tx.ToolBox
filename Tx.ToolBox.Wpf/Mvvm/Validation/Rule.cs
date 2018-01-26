using System;
using System.Collections.Generic;
using System.Linq;

namespace Tx.ToolBox.Wpf.Mvvm.Validation
{
    public interface IRuleBuilder
    {
        IEnumerable<(string Property, ValidationRule Rule)> BuildRules();
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

        public Rule Check(Func<string> validationRule)
        {
            _rules.Add(() => validationRule());
            return this;
        }

        public Rule Check(Func<ValidationResult> validationRule)
        {
            _rules.Add(validationRule);
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

        public Rule Async()
        {
            _async = true;
            return this;
        }

        private string[] _properties;
        private readonly List<Func<ValidationResult>> _rules = new List<Func<ValidationResult>>();
        private RevalidationReason _mode = RevalidationReason.PropertyChanged;
        private bool _async;

        private Rule()
        {
        }

        IEnumerable<(string Property, ValidationRule Rule)> IRuleBuilder.BuildRules()
        {
            foreach (var property in _properties)
            {
                foreach (var rule in _rules)
                {
                    yield return (property, new ValidationRule(rule, _mode, _async));
                }
            }
        }
    }
}