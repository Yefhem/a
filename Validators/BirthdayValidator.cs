using System;
using System.Text.RegularExpressions;

namespace UserInfoApp.Validators
{
    public class BirthdayValidator : IValidator<string>
    {
        private const int MinimumAge = 18;
        private const int MaximumAge = 120;
        private static readonly Regex DateFormatRegex = new(@"^\d{2}/\d{2}/\d{2}$");

        public (bool IsValid, string Error) Validate(string birthday)
        {
            if (string.IsNullOrWhiteSpace(birthday))
            {
                return (false, "Birthday cannot be empty!");
            }

            if (!DateFormatRegex.IsMatch(birthday))
            {
                return (false, "Birthday must be in format YY/MM/DD (e.g., 90/12/31 for December 31, 1990)!");
            }

            try
            {
                var parts = birthday.Split('/');
                int twoDigitYear = int.Parse(parts[0]);
                int month = int.Parse(parts[1]);
                int day = int.Parse(parts[2]);

                if (month < 1 || month > 12)
                {
                    return (false, "Month must be between 1 and 12!");
                }

                if (day < 1 || day > DateTime.DaysInMonth(2000, month))
                {
                    return (false, $"Invalid day for month {month}!");
                }

                int year = twoDigitYear >= 0 && twoDigitYear < 30 ? 2000 + twoDigitYear : 1900 + twoDigitYear;
                var birthDate = new DateTime(year, month, day);
                
                if (birthDate > DateTime.Today)
                {
                    return (false, "Birthday cannot be in the future!");
                }

                int age = CalculateAge(birthDate);
                if (age < MinimumAge)
                {
                    return (false, $"You must be at least {MinimumAge} years old to register!");
                }

                if (age > MaximumAge)
                {
                    return (false, $"Please check your birth date. Age cannot be over {MaximumAge} years!");
                }

                return (true, string.Empty);
            }
            catch
            {
                return (false, "Invalid date! Please enter a valid date in YY/MM/DD format.");
            }
        }

        public string Format(string birthday)
        {
            if (string.IsNullOrWhiteSpace(birthday)) return string.Empty;

            try
            {
                var parts = birthday.Split('/');
                int twoDigitYear = int.Parse(parts[0]);
                int year = twoDigitYear >= 0 && twoDigitYear < 30 ? 2000 + twoDigitYear : 1900 + twoDigitYear;
                return $"{year:D4}-{parts[1]:D2}-{parts[2]:D2}";
            }
            catch
            {
                return birthday;
            }
        }

        public DateTime? ParseBirthday(string birthday)
        {
            try
            {
                var parts = birthday.Split('/');
                int twoDigitYear = int.Parse(parts[0]);
                int month = int.Parse(parts[1]);
                int day = int.Parse(parts[2]);
                int year = twoDigitYear >= 0 && twoDigitYear < 30 ? 2000 + twoDigitYear : 1900 + twoDigitYear;
                return new DateTime(year, month, day);
            }
            catch
            {
                return null;
            }
        }

        private static int CalculateAge(DateTime birthDate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;
            if (birthDate.Date > today.AddYears(-age)) age--;
            return age;
        }
    }
}
