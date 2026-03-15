using System.Text;

namespace SafeVault.Security
{
    public class Sanitizer
    {
        public string SanitizeInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            var trimmed = input.Trim();
            var builder = new StringBuilder(trimmed.Length);

            foreach (var c in trimmed)
            {
                if (!char.IsControl(c))
                    builder.Append(c);
            }

            return builder.ToString();
        }
    }
}