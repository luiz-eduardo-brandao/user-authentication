using Users.API.Models.Base;

namespace Users.API.DTOs.Responses
{
    public class EmailConfirmationResponse : BaseResult
    {

        public EmailConfirmationResponse() { }

        public EmailConfirmationResponse(bool status) : base(status) 
        {
            
        }
    }
}