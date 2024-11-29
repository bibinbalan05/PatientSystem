using Patient.Domain.Entities.Models;

namespace Patient.API.Controllers.Auth.Responses
{
    public class UserResponse
    {
        public User User { get; set; }
        public string Token { get; set; }
    }
}
