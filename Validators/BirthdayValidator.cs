using System;
using System.Text.RegularExpressions;

namespace UserInfoApp.Validators
{
    public class BirthdayValidator
    {
        public static bool Validate(string birthday, out string error)
        {
            error = string.Empty;

            if (string.IsNullOrWhiteSpace(birthday))
            {
                error = "Birthday cannot be empty!";
                return false;
            }

            if (!Regex.IsMatch(birthday, @"^\d{2}/\d{2}/\d{2}$"))
            {
                error = "Birthday must be in format YY/MM/DD!";
                return false;
            }

            try
            {
                var parts = birthday.Split('/');
                int year = 2000 + int.Parse(parts[0]);
                int month = int.Parse(parts[1]);
                int day = int.Parse(parts[2]);

                var birthDate = new DateTime(year, month, day);
                
                if (birthDate > DateTime.Today)
                {
                    error = "Birthday cannot be in the future!";
                    return false;
                }

                int age = CalculateAge(birthDate);
                if (age < 18)
                {
                    error = "You must be at least 18 years old to register!";
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
            var parts = birthday.Split('/');
            int age = CalculateAge(new DateTime(2000 + int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2])));
            return $"20{parts[0]}/{parts[1]}/{parts[2]} (Age: {age} years)";
        }

        private static int CalculateAge(DateTime birthDate)
        {
            var age = DateTime.Today.Year - birthDate.Year;
            if (birthDate.Date > DateTime.Today.AddYears(-age))
                age--;
            return age;
        }
    }
}
