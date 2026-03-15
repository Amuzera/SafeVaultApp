namespace SafeVault.Security
{
    public sealed class ValidationResult
    {
        public bool IsValid { get; }
        public string ErrorMessage { get; }

        private ValidationResult(bool isValid, string errorMessage)
        {
            IsValid = isValid;
            ErrorMessage = errorMessage;
        }

        public static ValidationResult Ok()
        {
            return new ValidationResult(true, string.Empty);
        }

        public static ValidationResult Fail(string message)
        {
            return new ValidationResult(false, message);
        }
    }
}