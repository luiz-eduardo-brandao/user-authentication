using System.Text.Json.Serialization;

namespace Users.API.DTOs.Responses
{
    public class UserLoginResponse
    {
        public bool Success { get; set; }
        public string Error { get; set; }
        public DateTime ExpirationDate { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string AccessToken { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string RefreshToken { get; set; }

        public UserLoginResponse() {}

        public UserLoginResponse(bool status)
        {
            Success = status;
        }

        public void AddError(string error) 
        {
            Success = false;
            Error = error;
        }
    }
}