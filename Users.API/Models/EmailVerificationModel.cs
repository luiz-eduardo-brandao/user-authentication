using Users.API.Models.Base;

namespace Users.API.Models
{
    public class EmailVerificationModel : BaseResult
    {
        public EmailVerificationModel() { }

        public EmailVerificationModel(string userId, string email, string code)
        {
            Success = true;
            UserId = userId;
            Email = email;
            VerificationCode = code;
        }

        public string UserId { get; set; }
        public string Email { get; set; }
        public string VerificationCode { get; set; }
    }
}