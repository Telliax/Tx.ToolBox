namespace Tx.ToolBox.Wpf.Mvvm.Validation
{
    public class ValidationResult
    {
        public ValidationResult()
        {
        }

        public ValidationResult(string error)
        {
            Error = error;
        }

        public bool HasError => Error != null;
        public string Error { get; }

        public static implicit operator ValidationResult(string errorMessage)
        {
            if (errorMessage == null) return ValidResult;
            return new ValidationResult(errorMessage);
        }

        public override string ToString()
        {
            return HasError ? Error : "No errors";
        }

        public static readonly ValidationResult ValidResult = new ValidationResult();
    }
}