using System;
using System.Linq;

namespace UserInfoApp.Validators
{
    public class PESELValidator
    {
        public static bool Validate(string pesel, DateTime birthDate, string gender, out string error)
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
                
                // Validate birth date from PESEL matches provided birth date
                int peselYear = digits[0] * 10 + digits[1];
                int peselMonth = digits[2] * 10 + digits[3];
                int peselDay = digits[4] * 10 + digits[5];

                // Adjust month and determine century based on month coding in PESEL
                int actualYear = birthDate.Year;
                int actualMonth = birthDate.Month;
                int actualDay = birthDate.Day;

                // Convert actual date to PESEL format
                int expectedPeselYear = actualYear % 100;
                int expectedPeselMonth = actualMonth;
                
                // Adjust month based on century
                if (actualYear >= 2000 && actualYear < 2100)
                    expectedPeselMonth += 20;
                else if (actualYear >= 2100 && actualYear < 2200)
                    expectedPeselMonth += 40;
                else if (actualYear >= 2200 && actualYear < 2300)
                    expectedPeselMonth += 60;
                else if (actualYear >= 1800 && actualYear < 1900)
                    expectedPeselMonth += 80;

                if (peselYear != expectedPeselYear || 
                    peselMonth != expectedPeselMonth || 
                    peselDay != actualDay)
                {
                    error = "PESEL number does not match the provided birth date!";
                    return false;
                }

                // Validate gender
                string peselGender = GetGender(pesel);
                if ((gender == "M" && peselGender != "Male") ||
                    (gender == "F" && peselGender != "Female"))
                {
                    error = $"PESEL number indicates {peselGender} but you selected {(gender == "M" ? "Male" : "Female")}!";
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
