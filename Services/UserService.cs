using System;
using UserInfoApp.Models;
using UserInfoApp.Validators;

namespace UserInfoApp.Services
{
    public class UserService
    {
        private readonly IValidator<string> _birthdayValidator;
        private readonly IValidator<string> _emailValidator;
        private readonly IValidator<string> _phoneValidator;
        private readonly IValidator<(string Pesel, DateTime BirthDate, Gender Gender)> _peselValidator;

        public UserService(
            IValidator<string> birthdayValidator,
            IValidator<string> emailValidator,
            IValidator<string> phoneValidator,
            IValidator<(string Pesel, DateTime BirthDate, Gender Gender)> peselValidator)
        {
            _birthdayValidator = birthdayValidator;
            _emailValidator = emailValidator;
            _phoneValidator = phoneValidator;
            _peselValidator = peselValidator;
        }

        public (bool IsValid, string Error, DateTime? ParsedDate) ValidateBirthday(string birthday)
        {
            var (isValid, error) = _birthdayValidator.Validate(birthday);
            var parsedDate = ((BirthdayValidator)_birthdayValidator).ParseBirthday(birthday);
            return (isValid, error, parsedDate);
        }

        public (bool IsValid, string Error) ValidateEmail(string email)
        {
            return _emailValidator.Validate(email);
        }

        public (bool IsValid, string Error) ValidatePhone(string phone)
        {
            return _phoneValidator.Validate(phone);
        }

        public (bool IsValid, string Error) ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return (false, "Name cannot be empty!");
            }

            if (name.Length > 50)
            {
                return (false, "Name cannot be longer than 50 characters!");
            }

            return (true, string.Empty);
        }

        public (bool IsValid, string Error) ValidatePESEL(string pesel, DateTime birthDate, Gender gender)
        {
            return _peselValidator.Validate((pesel, birthDate, gender));
        }

        public string FormatBirthday(string birthday) => _birthdayValidator.Format(birthday);
        public string FormatPhone(string phone) => _phoneValidator.Format(phone);
    }
}
