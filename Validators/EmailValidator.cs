using System;
using System.Text.RegularExpressions;

namespace UserInfoApp.Validators
{
    public class EmailValidator
    {
        public static bool Validate(string email, out string error)
        {
            error = string.Empty;

            if (string.IsNullOrWhiteSpace(email))
            {
                error = "Email address cannot be empty!";
                return false;
            }

            if (!email.Contains("@"))
            {
                error = "Invalid email address! Missing @ symbol.";
                return false;
            }

            if (!email.Contains("."))
            {
                error = "Invalid email address! Missing domain extension (e.g., .com, .net).";
                return false;
            }

            try
            {
                var regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
                if (!regex.IsMatch(email))
                {
                    error = "Invalid email address! Please enter a valid email (e.g., example@domain.com).";
                    return false;
                }

                return true;
            }
            catch
            {
                error = "Invalid email format!";
                return false;
            }
        }
    }
}
