using System;
using System.Text.RegularExpressions;

namespace UserInfoApp.Validators
{
    public class EmailValidator : IValidator<string>
    {
        private static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);

        public (bool IsValid, string Error) Validate(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return (false, "Email address cannot be empty!");
            }

            string trimmedEmail = email.Trim();

            if (!trimmedEmail.Contains("@"))
            {
                return (false, "Invalid email address! Missing @ symbol.");
            }

            var parts = trimmedEmail.Split('@');
            if (parts.Length != 2)
            {
                return (false, "Invalid email address! Should contain exactly one @ symbol.");
            }

            if (parts[0].Length == 0)
            {
                return (false, "Invalid email address! Local part (before @) cannot be empty.");
            }

            if (!parts[1].Contains("."))
            {
                return (false, "Invalid email address! Domain part must include an extension (e.g., .com).");
            }

            if (!EmailRegex.IsMatch(trimmedEmail))
            {
                return (false, "Invalid email format! Please enter a valid email address.");
            }

            return (true, string.Empty);
        }

        public string Format(string email) => email?.Trim() ?? string.Empty;
    }
}
