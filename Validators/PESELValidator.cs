using System;
using System.Linq;

namespace UserInfoApp.Validators
{
    public class PESELValidator
    {
        public static bool Validate(string pesel, out string error)
        {
            error = string.Empty;

            if (string.IsNullOrWhiteSpace(pesel))
            {
                error = "PESEL number cannot be empty!";
                return false;
            }

            if (pesel.Length != 11)
            {
                error = "PESEL number must be exactly 11 digits!";
                return false;
            }

            if (!pesel.All(char.IsDigit))
            {
                error = "PESEL number must contain only digits!";
                return false;
            }

            try
            {
                int[] digits = pesel.Select(c => c - '0').ToArray();
                
                // Extract date parts
                int year = digits[0] * 10 + digits[1];
                int month = digits[2] * 10 + digits[3];
                int day = digits[4] * 10 + digits[5];

                // Adjust month and year based on century
                if (month > 80)
                {
                    year += 1800;
                    month -= 80;
                }
                else if (month > 60)
                {
                    year += 2200;
                    month -= 60;
                }
                else if (month > 40)
                {
                    year += 2100;
                    month -= 40;
                }
                else if (month > 20)
                {
                    year += 2000;
                    month -= 20;
                }
                else
                {
                    year += 1900;
                }

                // Validate date
                try
                {
                    var birthDate = new DateTime(year, month, day);
                    if (birthDate > DateTime.Today)
                    {
                        error = "PESEL indicates a birth date in the future!";
                        return false;
                    }
                }
                catch
                {
                    error = "PESEL contains invalid birth date!";
                    return false;
                }

                // Validate checksum
                int[] weights = { 1, 3, 7, 9, 1, 3, 7, 9, 1, 3, 1 };
                int sum = 0;
                for (int i = 0; i < 11; i++)
                {
                    sum += digits[i] * weights[i];
                }

                if (sum % 10 != 0)
                {
                    error = "Invalid PESEL number! Checksum verification failed.";
                    return false;
                }

                return true;
            }
            catch
            {
                error = "Invalid PESEL number format!";
                return false;
            }
        }

        public static string GetGender(string pesel)
        {
            int genderDigit = pesel[9] - '0';
            return genderDigit % 2 == 0 ? "Female" : "Male";
        }
    }
}
