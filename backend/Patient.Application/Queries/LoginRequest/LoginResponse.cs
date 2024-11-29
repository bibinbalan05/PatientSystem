using Patient.Domain.Entities.Models;

namespace Patient.Application.Queries.LoginRequest
{
    public class LoginResponse
    {
        public bool Success { get; set; }
        public User? User { get; set; }
    }
}
