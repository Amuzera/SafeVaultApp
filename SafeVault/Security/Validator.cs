using System.Text.RegularExpressions;

namespace SafeVault.Security
{
    public class Validator
    {
        private static readonly Regex UsernameRegex =
            new Regex(@"^[a-zA-Z0-9_.-]{3,30}$", RegexOptions.Compiled);

        private static readonly Regex EmailRegex =
            new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

        public ValidationResult ValidateUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return ValidationResult.Fail("Username is required.");

            var trimmed = username.Trim();

            if (trimmed.Length < 3 || trimmed.Length > 30)
                return ValidationResult.Fail("Username must be between 3 and 30 characters.");

            if (!UsernameRegex.IsMatch(trimmed))
                return ValidationResult.Fail("Username contains invalid characters. Use letters, numbers, underscores, dashes, and dots only.");

            return ValidationResult.Ok();
        }

        public ValidationResult ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return ValidationResult.Fail("Email is required.");

            var trimmed = email.Trim();

            if (trimmed.Length > 100)
                return ValidationResult.Fail("Email is too long.");

            if (!EmailRegex.IsMatch(trimmed))
                return ValidationResult.Fail("Email format is invalid.");

            return ValidationResult.Ok();
        }

        public ValidationResult ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return ValidationResult.Fail("Password is required.");

            if (password.Length < 8)
                return ValidationResult.Fail("Password must be at least 8 characters.");

            if (!password.Any(char.IsUpper))
                return ValidationResult.Fail("Password must contain at least one uppercase letter.");

            if (!password.Any(char.IsLower))
                return ValidationResult.Fail("Password must contain at least one lowercase letter.");

            if (!password.Any(char.IsDigit))
                return ValidationResult.Fail("Password must contain at least one number.");

            return ValidationResult.Ok();
        }
    }
}