using System;
using System.Linq;

namespace Tx.ToolBox.Wpf.Mvvm.Validation
{
    public class ValidationResult
    {
        public ValidationResult(params string[] errors)
        {
            Errors = errors;
        }

        public bool HasErrors => Errors.Length > 0;
        public string[] Errors { get; }

        public static implicit operator ValidationResult(string errorMessage)
        {
            if (errorMessage == null) return ValidResult;
            return new ValidationResult(errorMessage);
        }

        public override string ToString()
        {
            return HasErrors ? String.Join("\n", Errors) : "No errors";
        }

        public override bool Equals(object other)
        {
            if (other is ValidationResult result)
            {
                if (Errors.Length != result.Errors.Length) return false;
                return Errors.SequenceEqual(result.Errors);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return String.Join("", Errors).GetHashCode();
        }

        public static readonly ValidationResult ValidResult = new ValidationResult();
    }
}