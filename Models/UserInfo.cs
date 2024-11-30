using System;

namespace UserInfoApp.Models
{
    public class User
    {
        public string PESEL { get; private set; }
        public string Email { get; private set; }
        public string Name { get; private set; }
        public string Phone { get; private set; }
        public DateTime Birthday { get; private set; }
        public Gender Gender { get; private set; }

        private User() 
        {
            PESEL = string.Empty;
            Email = string.Empty;
            Name = string.Empty;
            Phone = string.Empty;
            Birthday = DateTime.MinValue;
            Gender = Gender.Unspecified;
        }

        public static User Create() => new User();

        public User WithPESEL(string pesel)
        {
            PESEL = pesel;
            return this;
        }

        public User WithEmail(string email)
        {
            Email = email;
            return this;
        }

        public User WithName(string name)
        {
            Name = name;
            return this;
        }

        public User WithPhone(string phone)
        {
            Phone = phone;
            return this;
        }

        public User WithBirthday(DateTime birthday)
        {
            Birthday = birthday;
            return this;
        }

        public User WithGender(Gender gender)
        {
            Gender = gender;
            return this;
        }
    }

    public enum Gender
    {
        Unspecified,
        Male,
        Female
    }
}
