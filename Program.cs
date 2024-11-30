using System;
using UserInfoApp.Models;
using UserInfoApp.Services;
using UserInfoApp.Validators;

namespace UserInfoApp
{
    class Program
    {
        private const int MaxAttempts = 3;

        static void ShowInformation(User user, string failedField = null)
        {
            Console.WriteLine("\nInformation Summary:");
            Console.WriteLine($"Birthday: {user.Birthday:yyyy-MM-dd}{(failedField == "Birthday" ? " ❌" : " ✓")}");
            Console.WriteLine($"Gender: {user.Gender}{(failedField == "Gender" ? " ❌" : " ✓")}");
            Console.WriteLine($"Email: {user.Email}{(failedField == "Email" ? " ❌" : " ✓")}");
            Console.WriteLine($"Name: {user.Name}{(failedField == "Name" ? " ❌" : " ✓")}");
            Console.WriteLine($"Phone: {user.Phone}{(failedField == "Phone" ? " ❌" : " ✓")}");
            Console.WriteLine($"PESEL: {user.PESEL}{(failedField == "PESEL" ? " ❌" : " ✓")}");
        }

        static void ExitWithError(User user, string failedField)
        {
            ShowInformation(user, failedField);
            Console.WriteLine($"\nValidation failed for {failedField} after {MaxAttempts} attempts.");
            Console.WriteLine("Thank you for trying. Goodbye!");
            Environment.Exit(0);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to User Information Collection System");
            Console.WriteLine($"You have {MaxAttempts} attempts for each field.\n");

            var userService = new UserService(
                new BirthdayValidator(),
                new EmailValidator(),
                new PhoneValidator(),
                new PESELValidator()
            );

            var user = User.Create();
            DateTime? birthDate = null;

            // Birthday validation
            int attempts = 0;
            bool isValid = false;
            do
            {
                attempts++;
                Console.Write($"Please enter your birthday (format: YY/MM/DD) [Attempt {attempts}/{MaxAttempts}]: ");
                string birthday = Console.ReadLine()?.Trim() ?? "";
                
                var (valid, error, parsedDate) = userService.ValidateBirthday(birthday);
                if (valid && parsedDate.HasValue)
                {
                    birthDate = parsedDate.Value;
                    user = user.WithBirthday(parsedDate.Value);
                    isValid = true;
                }
                else
                {
                    Console.WriteLine($"Error: {error}");
                    if (attempts >= MaxAttempts)
                        ExitWithError(user, "Birthday");
                }
            } while (!isValid);

            // Gender validation
            attempts = 0;
            isValid = false;
            do
            {
                attempts++;
                Console.Write($"Please enter your gender (M/F) [Attempt {attempts}/{MaxAttempts}]: ");
                string genderInput = Console.ReadLine()?.Trim().ToUpper() ?? "";

                if (genderInput == "M")
                {
                    user = user.WithGender(Gender.Male);
                    isValid = true;
                }
                else if (genderInput == "F")
                {
                    user = user.WithGender(Gender.Female);
                    isValid = true;
                }
                else
                {
                    Console.WriteLine("Error: Gender must be either 'M' for Male or 'F' for Female!");
                    if (attempts >= MaxAttempts)
                        ExitWithError(user, "Gender");
                }
            } while (!isValid);

            // Email validation
            attempts = 0;
            isValid = false;
            do
            {
                attempts++;
                Console.Write($"Please enter your email address [Attempt {attempts}/{MaxAttempts}]: ");
                string email = Console.ReadLine()?.Trim().ToLower() ?? "";

                var (valid, error) = userService.ValidateEmail(email);
                if (valid)
                {
                    user = user.WithEmail(email);
                    isValid = true;
                }
                else
                {
                    Console.WriteLine($"Error: {error}");
                    if (attempts >= MaxAttempts)
                        ExitWithError(user, "Email");
                }
            } while (!isValid);

            // Name validation
            attempts = 0;
            isValid = false;
            do
            {
                attempts++;
                Console.Write($"Please enter your full name [Attempt {attempts}/{MaxAttempts}]: ");
                string name = Console.ReadLine()?.Trim() ?? "";

                var (valid, error) = userService.ValidateName(name);
                if (valid)
                {
                    user = user.WithName(name);
                    isValid = true;
                }
                else
                {
                    Console.WriteLine($"Error: {error}");
                    if (attempts >= MaxAttempts)
                        ExitWithError(user, "Name");
                }
            } while (!isValid);

            // Phone validation
            attempts = 0;
            isValid = false;
            do
            {
                attempts++;
                Console.Write($"Please enter your Polish phone number (format: +48 XXX XXX XXX) [Attempt {attempts}/{MaxAttempts}]: ");
                string phone = Console.ReadLine()?.Trim() ?? "";

                var (valid, error) = userService.ValidatePhone(phone);
                if (valid)
                {
                    user = user.WithPhone(userService.FormatPhone(phone));
                    isValid = true;
                }
                else
                {
                    Console.WriteLine($"Error: {error}");
                    if (attempts >= MaxAttempts)
                        ExitWithError(user, "Phone");
                }
            } while (!isValid);

            // PESEL validation
            attempts = 0;
            isValid = false;
            do
            {
                attempts++;
                Console.Write($"\nPlease enter your PESEL number (11 digits) [Attempt {attempts}/{MaxAttempts}]: ");
                string pesel = Console.ReadLine()?.Trim() ?? "";

                var (valid, error) = userService.ValidatePESEL(pesel, birthDate ?? DateTime.MinValue, user.Gender);
                if (valid)
                {
                    user = user.WithPESEL(pesel);
                    isValid = true;
                }
                else
                {
                    Console.WriteLine($"Error: {error}");
                    if (attempts >= MaxAttempts)
                        ExitWithError(user, "PESEL");
                }
            } while (!isValid);

            // Show success message and collected information
            Console.WriteLine("\n🎉 Congratulations! All information has been successfully validated! 🎉");
            ShowInformation(user);
            Console.WriteLine("\nThank you for providing your information. Have a great day! 👋");
        }
    }
}
