namespace Users.API.Models.Base
{
    public class BaseResult
    {
        public bool Success { get; set; }
        public string Error { get; set; }

        public BaseResult()
        {
            
        }

        public BaseResult(bool status)
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