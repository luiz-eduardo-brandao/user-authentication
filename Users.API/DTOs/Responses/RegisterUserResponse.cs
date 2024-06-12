using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace Users.API.DTOs.Responses
{
    public class RegisterUserResponse
    {
        public RegisterUserResponse() { }

        public RegisterUserResponse(bool status)
        {
            Success = status;
        }

        public RegisterUserResponse(bool status, IdentityUser user)
        {
            Success = status;
            User = user;
        }

        public void AddErrors(IEnumerable<string> errors) 
        {
            Errors = errors.ToList();
        }

        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> Errors { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public IdentityUser User { get; set; }
    }
}