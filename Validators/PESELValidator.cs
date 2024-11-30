using System;
using System.Linq;
using UserInfoApp.Models;

namespace UserInfoApp.Validators
{
    public class PESELValidator : IValidator<(string Pesel, DateTime BirthDate, Gender Gender)>
    {
        public (bool IsValid, string Error) Validate((string Pesel, DateTime BirthDate, Gender Gender) input)
        {
            var (pesel, birthDate, gender) = input;
            
            if (string.IsNullOrWhiteSpace(pesel))
            {
                return (false, "PESEL number cannot be empty!");
            }

            if (pesel.Length != 11)
            {
                return (false, "PESEL number must be exactly 11 digits!");
            }

            if (!pesel.All(char.IsDigit))
            {
                return (false, "PESEL number must contain only digits!");
            }

            try
            {
                int[] digits = pesel.Select(c => c - '0').ToArray();
                
                int peselYear = digits[0] * 10 + digits[1];
                int peselMonth = digits[2] * 10 + digits[3];
                int peselDay = digits[4] * 10 + digits[5];

                int actualYear = birthDate.Year;
                int actualMonth = birthDate.Month;
                int actualDay = birthDate.Day;

                int expectedPeselYear = actualYear % 100;
                int expectedPeselMonth = actualMonth;
                
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
                    return (false, "PESEL number does not match the provided birth date!");
                }

                var peselGender = GetGender(pesel);
                if ((gender == Gender.Male && peselGender == Gender.Female) ||
                    (gender == Gender.Female && peselGender == Gender.Male))
                {
                    return (false, $"PESEL number indicates {peselGender} but you selected {gender}!");
                }

                int[] weights = { 1, 3, 7, 9, 1, 3, 7, 9, 1, 3, 1 };
                int sum = 0;
                for (int i = 0; i < 11; i++)
                {
                    sum += digits[i] * weights[i];
                }

                if (sum % 10 != 0)
                {
                    return (false, "Invalid PESEL number! Checksum verification failed.");
                }

                return (true, string.Empty);
            }
            catch
            {
                return (false, "Invalid PESEL number format!");
            }
        }

        public string Format(string pesel) => pesel;

        public string Format((string Pesel, DateTime BirthDate, Gender Gender) input) => input.Pesel;

        private static Gender GetGender(string pesel)
        {
            int genderDigit = pesel[9] - '0';
            return genderDigit % 2 == 0 ? Gender.Female : Gender.Male;
        }
    }
}
