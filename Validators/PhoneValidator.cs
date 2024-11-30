using System;
using System.Linq;

namespace UserInfoApp.Validators
{
    public class PhoneValidator
    {
        public static bool Validate(string phone, out string error)
        {
            error = string.Empty;

            if (string.IsNullOrWhiteSpace(phone))
            {
                error = "Phone number cannot be empty!";
                return false;
            }

            // Remove all whitespace
            string cleanPhone = new string(phone.Where(c => !char.IsWhiteSpace(c)).ToArray());

            if (!cleanPhone.StartsWith("+48"))
            {
                error = "Phone number must start with +48 (Polish country code)!";
                return false;
            }

            if (cleanPhone.Length != 12) // +48 + 9 digits
            {
                error = "Invalid Polish phone number! Must have 9 digits after +48.";
                return false;
            }

            if (!cleanPhone.Substring(3).All(char.IsDigit))
            {
                error = "Invalid phone number! Must contain only digits after +48.";
                return false;
            }

            return true;
        }

        public static string Format(string phone)
        {
            string digits = new string(phone.Where(c => !char.IsWhiteSpace(c)).ToArray()).Substring(3);
            return $"+48 {digits.Substring(0,3)} {digits.Substring(3,3)} {digits.Substring(6,3)}";
        }
    }
}
