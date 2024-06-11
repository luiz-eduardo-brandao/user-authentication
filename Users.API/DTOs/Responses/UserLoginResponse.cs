using System.Text.Json.Serialization;
using Users.API.Models.Base;

namespace Users.API.DTOs.Responses
{
    public class UserLoginResponse : BaseResult
    {
        public DateTime ExpirationDate { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string AccessToken { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string RefreshToken { get; set; }

        public UserLoginResponse() {}

        public UserLoginResponse(bool status) : base(status)
        {
        }
    }
}