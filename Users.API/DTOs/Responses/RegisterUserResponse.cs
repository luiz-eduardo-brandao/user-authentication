namespace Users.API.DTOs.Responses
{
    public class RegisterUserResponse
    {
        public RegisterUserResponse(bool status)
        {
            Success = status;
        }

        public void AddErrors(IEnumerable<string> errors) 
        {
            Errors = errors.ToList();
        }

        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }
}