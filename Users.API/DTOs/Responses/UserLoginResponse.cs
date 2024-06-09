namespace Users.API.DTOs.Responses
{
    public class UserLoginResponse
    {
        public UserLoginResponse() {}

        public UserLoginResponse(bool status)
        {
            Success = status;
        }

        public void AddError(string error) 
        {
            Error = error;
        }

        public bool Success { get; set; }
        public string Error { get; set; }
        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}