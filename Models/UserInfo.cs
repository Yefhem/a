namespace UserInfoApp.Models
{
    public class UserInfo
    {
        public string PESEL { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Birthday { get; set; }
        public string Gender { get; set; }

        public UserInfo()
        {
            PESEL = string.Empty;
            Email = string.Empty;
            Name = string.Empty;
            Phone = string.Empty;
            Birthday = string.Empty;
            Gender = string.Empty;
        }
    }
}
