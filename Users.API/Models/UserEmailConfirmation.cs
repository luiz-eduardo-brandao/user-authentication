namespace Users.API.Models
{
    public class UserEmailConfirmation
    {
        public UserEmailConfirmation(string userId, string email, string code)
        {
            UserId = userId;
            Email = email;
            VerificationCode = code;
        }

        public string UserId { get; set; }
        public string Email { get; set; }
        public string VerificationCode { get; set; }
    }
}