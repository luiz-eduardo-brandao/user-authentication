using System.Text.Json.Serialization;
using Users.API.Models;

namespace Users.API.DTOs.Responses
{
    public class RegisterUserResponse
    {
        public RegisterUserResponse() { }

        public RegisterUserResponse(bool status)
        {
            Success = status;
        }

        public void AddErrors(IEnumerable<string> errors) 
        {
            Errors = errors.ToList();
        }

        public bool Success { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public UserEmailConfirmation EmailConfirmation { get; set; }
    }
}