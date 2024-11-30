using System;
using UserInfoApp.Models;
using UserInfoApp.Validators;

namespace UserInfoApp
{
    class Program
    {
        private const int MaxAttempts = 3;

        static void ShowInformation(UserInfo userInfo, string failedField = null)
        {
            Console.WriteLine("\nInformation Summary:");
            if (!string.IsNullOrEmpty(userInfo.Birthday))
                Console.WriteLine($"Birthday: {BirthdayValidator.Format(userInfo.Birthday)}{(failedField == "Birthday" ? " ❌" : " ✓")}");
            if (!string.IsNullOrEmpty(userInfo.Gender))
                Console.WriteLine($"Gender: {GenderValidator.Format(userInfo.Gender)}{(failedField == "Gender" ? " ❌" : " ✓")}");
            if (!string.IsNullOrEmpty(userInfo.Email))
                Console.WriteLine($"Email: {userInfo.Email}{(failedField == "Email" ? " ❌" : " ✓")}");
            if (!string.IsNullOrEmpty(userInfo.Name))
                Console.WriteLine($"Name: {userInfo.Name}{(failedField == "Name" ? " ❌" : " ✓")}");
            if (!string.IsNullOrEmpty(userInfo.Phone))
                Console.WriteLine($"Phone: {PhoneValidator.Format(userInfo.Phone)}{(failedField == "Phone" ? " ❌" : " ✓")}");
            if (!string.IsNullOrEmpty(userInfo.PESEL))
                Console.WriteLine($"PESEL: {userInfo.PESEL}{(failedField == "PESEL" ? " ❌" : " ✓")}");
        }

        static void ExitWithError(UserInfo userInfo, string failedField)
        {
            ShowInformation(userInfo, failedField);
            Console.WriteLine($"\nValidation failed for {failedField} after {MaxAttempts} attempts.");
            Console.WriteLine("Thank you for trying. Goodbye!");
            Environment.Exit(0);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to User Information Collection System");
            Console.WriteLine($"You have {MaxAttempts} attempts for each field.\n");
            var userInfo = new UserInfo();
            DateTime? birthDate = null;

            // Birthday validation
            int attempts = 0;
            bool isValid = false;
            do
            {
                attempts++;
                Console.Write($"Please enter your birthday (format: YY/MM/DD) [Attempt {attempts}/{MaxAttempts}]: ");
                userInfo.Birthday = Console.ReadLine()?.Trim() ?? "";
                
                if (BirthdayValidator.Validate(userInfo.Birthday, out string birthdayError))
                {
                    var parts = userInfo.Birthday.Split('/');
                    int twoDigitYear = int.Parse(parts[0]);
                    int year = twoDigitYear >= 0 && twoDigitYear < 30 ? 2000 + twoDigitYear : 1900 + twoDigitYear;
                    birthDate = new DateTime(year, int.Parse(parts[1]), int.Parse(parts[2]));
                    isValid = true;
                }
                else
                {
                    Console.WriteLine($"Error: {birthdayError}");
                    if (attempts >= MaxAttempts)
                        ExitWithError(userInfo, "Birthday");
                }
            } while (!isValid);

            // Gender validation
            attempts = 0;
            isValid = false;
            do
            {
                attempts++;
                Console.Write($"Please enter your gender (M/F) [Attempt {attempts}/{MaxAttempts}]: ");
                userInfo.Gender = Console.ReadLine()?.Trim().ToUpper() ?? "";

                if (!string.IsNullOrWhiteSpace(userInfo.Gender) && (userInfo.Gender == "M" || userInfo.Gender == "F"))
                {
                    isValid = true;
                }
                else
                {
                    Console.WriteLine("Error: Gender must be either 'M' for Male or 'F' for Female!");
                    if (attempts >= MaxAttempts)
                        ExitWithError(userInfo, "Gender");
                }
            } while (!isValid);

            // Email validation
            attempts = 0;
            isValid = false;
            do
            {
                attempts++;
                Console.Write($"Please enter your email address [Attempt {attempts}/{MaxAttempts}]: ");
                userInfo.Email = Console.ReadLine()?.Trim().ToLower() ?? "";

                if (EmailValidator.Validate(userInfo.Email, out string emailError))
                {
                    isValid = true;
                }
                else
                {
                    Console.WriteLine($"Error: {emailError}");
                    if (attempts >= MaxAttempts)
                        ExitWithError(userInfo, "Email");
                }
            } while (!isValid);

            // Name validation
            attempts = 0;
            isValid = false;
            do
            {
                attempts++;
                Console.Write($"Please enter your full name [Attempt {attempts}/{MaxAttempts}]: ");
                userInfo.Name = Console.ReadLine()?.Trim() ?? "";

                if (!string.IsNullOrWhiteSpace(userInfo.Name) && userInfo.Name.Length <= 50)
                {
                    isValid = true;
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(userInfo.Name))
                        Console.WriteLine("Error: Name cannot be empty!");
                    else
                        Console.WriteLine("Error: Name cannot be longer than 50 characters!");

                    if (attempts >= MaxAttempts)
                        ExitWithError(userInfo, "Name");
                }
            } while (!isValid);

            // Phone validation
            attempts = 0;
            isValid = false;
            do
            {
                attempts++;
                Console.Write($"Please enter your Polish phone number (format: +48 XXX XXX XXX) [Attempt {attempts}/{MaxAttempts}]: ");
                userInfo.Phone = Console.ReadLine()?.Trim() ?? "";

                if (PhoneValidator.Validate(userInfo.Phone, out string phoneError))
                {
                    isValid = true;
                }
                else
                {
                    Console.WriteLine($"Error: {phoneError}");
                    if (attempts >= MaxAttempts)
                        ExitWithError(userInfo, "Phone");
                }
            } while (!isValid);

            // PESEL validation
            attempts = 0;
            isValid = false;
            do
            {
                attempts++;
                Console.Write($"\nPlease enter your PESEL number (11 digits) [Attempt {attempts}/{MaxAttempts}]: ");
                userInfo.PESEL = Console.ReadLine()?.Trim() ?? "";

                if (PESELValidator.Validate(userInfo.PESEL, birthDate ?? DateTime.MinValue, userInfo.Gender, out string peselError))
                {
                    isValid = true;
                }
                else
                {
                    Console.WriteLine($"Error: {peselError}");
                    if (attempts >= MaxAttempts)
                        ExitWithError(userInfo, "PESEL");
                }
            } while (!isValid);

            // Show success message and collected information
            Console.WriteLine("\n🎉 Congratulations! All information has been successfully validated! 🎉");
            ShowInformation(userInfo);
            Console.WriteLine("\nThank you for providing your information. Have a great day! 👋");
        }
    }
}
