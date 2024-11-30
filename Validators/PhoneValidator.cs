using System;
using System.Linq;

namespace UserInfoApp.Validators
{
    public class PhoneValidator : IValidator<string>
    {
        public (bool IsValid, string Error) Validate(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
            {
                return (false, "Phone number cannot be empty!");
            }

            // Remove all whitespace
            string cleanPhone = new string(phone.Where(c => !char.IsWhiteSpace(c)).ToArray());

            if (!cleanPhone.StartsWith("+48"))
            {
                return (false, "Phone number must start with +48 (Polish country code)!");
            }

            if (cleanPhone.Length != 12) // +48 + 9 digits
            {
                return (false, "Invalid Polish phone number! Must have 9 digits after +48.");
            }

            if (!cleanPhone.Substring(3).All(char.IsDigit))
            {
                return (false, "Invalid phone number! Must contain only digits after +48.");
            }

            return (true, string.Empty);
        }

        public string Format(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) return string.Empty;
            
            string digits = new string(phone.Where(c => !char.IsWhiteSpace(c)).ToArray());
            if (digits.Length < 11) return phone;
            
            digits = digits.StartsWith("+48") ? digits.Substring(3) : digits;
            return $"+48 {digits.Substring(0,3)} {digits.Substring(3,3)} {digits.Substring(6,3)}";
        }
    }
}
