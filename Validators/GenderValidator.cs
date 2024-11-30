using System;

namespace UserInfoApp.Validators
{
    public class GenderValidator
    {
        public static bool Validate(string gender, string pesel, out string error)
        {
            error = string.Empty;

            if (string.IsNullOrWhiteSpace(gender))
            {
                error = "Gender cannot be empty!";
                return false;
            }

            if (gender != "M" && gender != "F")
            {
                error = "Gender must be either 'M' for Male or 'F' for Female!";
                return false;
            }

            string peselGender = PESELValidator.GetGender(pesel);
            bool genderMatch = (gender == "M" && peselGender == "Male") || 
                             (gender == "F" && peselGender == "Female");

            if (!genderMatch)
            {
                error = $"Gender does not match your PESEL number! PESEL indicates you are {peselGender}.";
                return false;
            }

            return true;
        }

        public static string Format(string gender)
        {
            return gender == "M" ? "Male" : "Female";
        }
    }
}
