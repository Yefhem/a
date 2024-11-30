using System;
using System.Text.RegularExpressions;

namespace UserInfoApp.Validators
{
    public class BirthdayValidator
    {
        private const int MinimumAge = 18;
        private static readonly Regex DateFormatRegex = new(@"^\d{2}/\d{2}/\d{2}$");

        public static bool Validate(string birthday, out string error)
        {
            error = string.Empty;

            if (string.IsNullOrWhiteSpace(birthday))
            {
                error = "Birthday cannot be empty!";
                return false;
            }

            if (!DateFormatRegex.IsMatch(birthday))
            {
                error = "Birthday must be in format YY/MM/DD (e.g., 90/12/31 for December 31, 1990)!";
                return false;
            }

            try
            {
                var parts = birthday.Split('/');
                int twoDigitYear = int.Parse(parts[0]);
                int month = int.Parse(parts[1]);
                int day = int.Parse(parts[2]);

                // Validate month and day ranges before creating DateTime
                if (month < 1 || month > 12)
                {
                    error = "Month must be between 1 and 12!";
                    return false;
                }

                if (day < 1 || day > DateTime.DaysInMonth(2000, month))  // Use 2000 as it's a leap year
                {
                    error = $"Invalid day for month {month}!";
                    return false;
                }

                // Convert two-digit year to four-digit year
                // Years 00-29 are considered 2000-2029
                // Years 30-99 are considered 1930-1999
                int year = twoDigitYear >= 0 && twoDigitYear < 30 ? 2000 + twoDigitYear : 1900 + twoDigitYear;

                var birthDate = new DateTime(year, month, day);
                
                if (birthDate > DateTime.Today)
                {
                    error = "Birthday cannot be in the future!";
                    return false;
                }

                int age = CalculateAge(birthDate);
                if (age < MinimumAge)
                {
                    error = $"You must be at least {MinimumAge} years old to register!";
                    return false;
                }

                if (age > 120)
                {
                    error = "Please check your birth date. Age cannot be over 120 years!";
                    return false;
                }

                return true;
            }
            catch
            {
                error = "Invalid date! Please enter a valid date in YY/MM/DD format.";
                return false;
            }
        }

        public static string Format(string birthday)
        {
            if (string.IsNullOrWhiteSpace(birthday)) return "";

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

        private static int CalculateAge(DateTime birthDate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;
            if (birthDate.Date > today.AddYears(-age)) age--;
            return age;
        }
    }
}
