namespace Tx.ToolBox.Wpf.Mvvm.Validation
{
    public abstract class ValidationRuleBase
    {
        protected ValidationRuleBase(RevalidationReason mode)
        {
            Mode = mode;
        }

        public RevalidationReason Mode { get; }
        public ValidationResult Result { get; protected set; } = ValidationResult.ValidResult;

        public void Clear()
        {
            Result = ValidationResult.ValidResult;
        }
    }
}