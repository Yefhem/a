using System;
using UserInfoApp.Models;
using UserInfoApp.Validators;

namespace UserInfoApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to User Information Collection System");
            bool isValid = false;
            var userInfo = new UserInfo();

            do
            {
                // Collect all information first
                Console.Write("\nPlease enter your PESEL number (11 digits): ");
                userInfo.PESEL = Console.ReadLine()?.Trim() ?? "";

                Console.Write("Please enter your email address: ");
                userInfo.Email = Console.ReadLine()?.Trim().ToLower() ?? "";

                Console.Write("Please enter your full name: ");
                userInfo.Name = Console.ReadLine()?.Trim() ?? "";

                Console.Write("Please enter your Polish phone number (format: +48 XXX XXX XXX): ");
                userInfo.Phone = Console.ReadLine()?.Trim() ?? "";

                Console.Write("Please enter your birthday (format: YY/MM/DD): ");
                userInfo.Birthday = Console.ReadLine()?.Trim() ?? "";

                Console.Write("Please enter your gender (M/F): ");
                userInfo.Gender = Console.ReadLine()?.Trim().ToUpper() ?? "";

                // Validate all inputs and show responses
                Console.WriteLine("\nValidating your information...");
                bool hasErrors = false;

                // Validate PESEL
                if (!PESELValidator.Validate(userInfo.PESEL, out string peselError))
                {
                    Console.WriteLine($"Error: {peselError}");
                    hasErrors = true;
                }

                // Validate Email
                if (!EmailValidator.Validate(userInfo.Email, out string emailError))
                {
                    Console.WriteLine($"Error: {emailError}");
                    hasErrors = true;
                }

                // Validate Name
                if (string.IsNullOrWhiteSpace(userInfo.Name))
                {
                    Console.WriteLine("Error: Name cannot be empty!");
                    hasErrors = true;
                }
                else if (userInfo.Name.Length > 50)
                {
                    Console.WriteLine("Error: Name cannot be longer than 50 characters!");
                    hasErrors = true;
                }

                // Validate Phone Number
                if (!PhoneValidator.Validate(userInfo.Phone, out string phoneError))
                {
                    Console.WriteLine($"Error: {phoneError}");
                    hasErrors = true;
                }

                // Validate Birthday
                if (!BirthdayValidator.Validate(userInfo.Birthday, out string birthdayError))
                {
                    Console.WriteLine($"Error: {birthdayError}");
                    hasErrors = true;
                }

                // Validate Gender
                if (!GenderValidator.Validate(userInfo.Gender, userInfo.PESEL, out string genderError))
                {
                    Console.WriteLine($"Error: {genderError}");
                    hasErrors = true;
                }

                // If all validations pass, show success message and collected information
                if (!hasErrors)
                {
                    Console.WriteLine("\nAll information is valid!");
                    Console.WriteLine("\nCollected Information:");
                    Console.WriteLine($"PESEL: {userInfo.PESEL} ({PESELValidator.GetGender(userInfo.PESEL)})");
                    Console.WriteLine($"Email: {userInfo.Email}");
                    Console.WriteLine($"Name: {userInfo.Name}");
                    Console.WriteLine($"Phone: {PhoneValidator.Format(userInfo.Phone)}");
                    Console.WriteLine($"Birthday: {BirthdayValidator.Format(userInfo.Birthday)}");
                    Console.WriteLine($"Gender: {GenderValidator.Format(userInfo.Gender)}");
                    isValid = true;
                }
                else
                {
                    Console.WriteLine("\nPlease correct the errors and try again.");
                }

            } while (!isValid);
        }
    }
}
