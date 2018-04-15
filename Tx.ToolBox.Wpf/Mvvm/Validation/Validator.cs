using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Tx.ToolBox.Wpf.Mvvm.Validation
{
    public class Validator
    {
        public Validator(object parentViewModel)
        {
            _parentViewModel = parentViewModel;
            _cts = new CancellationTokenSource();
            _token = _cts.Token;
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public bool HasErrors => _rules.Values.Any(v => v.HasErrors);

        public IEnumerable<string> GetErrors(string propertyName)
        {
            if (!_rules.TryGetValue(propertyName, out var rules)) return NoErrors;
            return rules.Errors;
        }

        public IEnumerable<string> GetAllErrors()
        {
            return _rules.Values.SelectMany(v => v.Errors);
        }

        public Task AsyncValidation { get; private set; } = Task.CompletedTask;

        public void AddRule(Rule ruleExpression)
        {
            var builder = ruleExpression as IRuleBuilder;
            foreach (var rule in builder.BuildRules())
            {
                if (!_rules.TryGetValue(rule.Property, out var ruleCollection))
                {
                    _rules[rule.Property] = ruleCollection = new RuleCollection {PropertyName = rule.Property };
                }
                if (rule.Rule is ValidationRule syncRule)
                {
                    ruleCollection.AddRule(syncRule);
                }
                else if (rule.Rule is AsyncValidationRule asyncRule)
                {
                    ruleCollection.AddRule(asyncRule);
                }
                else
                {
                    throw new NotSupportedException(rule.Rule.GetType().FullName);
                }
            }
        }

        public async Task ClearErrorsAsync()
        {
            await CancelValidationAsync();
            foreach (var rule in _rules.Values)
            {
                rule.ClearErrors();
            }
        }

        public async Task CancelValidationAsync()
        {
            Task task;
            lock (_taskLock)
            {
                task = AsyncValidation;
                if (task.Status >= TaskStatus.RanToCompletion) return;
                _cts.Cancel();
                _cts.Dispose();
            }
            await task;
        }

        public async Task ValidateAsync(string propertyName = null, RevalidationReason reason = RevalidationReason.ExplicitlyCalled)
        {
            var rules = GetRules(propertyName);
            await ValidateAsync(rules, reason);
        }

        private readonly Dictionary<string, RuleCollection> _rules = new Dictionary<string, RuleCollection>();
        private CancellationTokenSource _cts;
        private CancellationToken _token;
        private static readonly string[] NoErrors = new string[0];
        private readonly object _taskLock = new object();
        private readonly object _parentViewModel;

        private IEnumerable<RuleCollection> GetRules(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                foreach (var collection in _rules.Values)
                {
                    yield return collection;
                }
            }
            else
            {
                if (_rules.TryGetValue(propertyName, out var rules))
                {
                    yield return rules;
                }
            }
        }

        private async Task ValidateAsync(IEnumerable<RuleCollection> rules, RevalidationReason reason)
        {
            var asyncValidation = Task.CompletedTask;
            var hasAsyncRules = rules.Any(r => r.AsyncRules.Any());

            if (hasAsyncRules)
            {
                lock (_taskLock)
                {
                    if (_token.IsCancellationRequested)
                    {
                        _cts = new CancellationTokenSource();
                        _token = _cts.Token;
                    }
                    var token = _token;
                    asyncValidation = AsyncValidation = AsyncValidation.ContinueWith(t =>
                    {
                        foreach (var collection in rules)
                        {
                            var tasks = collection.AsyncRules.Select(r => r.ValidateAsync(token, reason)).ToArray();
                            Task.WaitAll(tasks);
                            if (tasks.Any(task => task.Result))
                            {
                                ErrorsChanged?.Invoke(_parentViewModel, new DataErrorsChangedEventArgs(collection.PropertyName));
                            }
                        }              
                        
                    }, token);
                }
            }

            foreach (var collection in rules)
            {
                var changed = false;
                foreach (var rule in collection.SyncRules)
                {
                    changed |= rule.Validate(reason);
                }
                if (changed)
                {
                    ErrorsChanged?.Invoke(_parentViewModel, new DataErrorsChangedEventArgs(collection.PropertyName));
                }
            }

            await asyncValidation;
        }

        private class RuleCollection
        {
            public string PropertyName { get; set; }

            public bool HasErrors => AllRules.Any(r => r.Result.HasErrors);
            public IEnumerable<string> Errors => AllRules.Where(r => r.Result.HasErrors).SelectMany(r => r.Result.Errors);

            public IReadOnlyCollection<ValidationRule> SyncRules => _syncRules;
            public IReadOnlyCollection<AsyncValidationRule> AsyncRules => _asyncRules;
            public IEnumerable<ValidationRuleBase> AllRules => _syncRules.Concat<ValidationRuleBase>(_asyncRules);

            public void AddRule(ValidationRule rule)
            {
                _syncRules.Add(rule);
            }

            public void AddRule(AsyncValidationRule rule)
            {
                _asyncRules.Add(rule);
            }

            public void ClearErrors()
            {
                foreach (var rule in AllRules)
                {
                    rule.Clear();
                }
            }

            private readonly List<ValidationRule> _syncRules = new List<ValidationRule>();
            private readonly List<AsyncValidationRule> _asyncRules = new List<AsyncValidationRule>();
        }
    }
}
