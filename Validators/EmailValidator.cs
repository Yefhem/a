using System;
using System.Text.RegularExpressions;

namespace UserInfoApp.Validators
{
    public class EmailValidator
    {
        private static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);

        public static bool Validate(string email, out string error)
        {
            error = string.Empty;

            if (string.IsNullOrWhiteSpace(email))
            {
                error = "Email address cannot be empty!";
                return false;
            }

            string trimmedEmail = email.Trim();

            if (!trimmedEmail.Contains("@"))
            {
                error = "Invalid email address! Missing @ symbol.";
                return false;
            }

            var parts = trimmedEmail.Split('@');
            if (parts.Length != 2)
            {
                error = "Invalid email address! Should contain exactly one @ symbol.";
                return false;
            }

            if (parts[0].Length == 0)
            {
                error = "Invalid email address! Local part (before @) cannot be empty.";
                return false;
            }

            if (!parts[1].Contains("."))
            {
                error = "Invalid email address! Domain part must include an extension (e.g., .com).";
                return false;
            }

            if (!EmailRegex.IsMatch(trimmedEmail))
            {
                error = "Invalid email format! Please enter a valid email address.";
                return false;
            }

            return true;
        }
    }
}
