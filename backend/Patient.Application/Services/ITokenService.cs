
using Patient.Domain.Entities.Models;

namespace Patient.Application.Services
{
    public interface ITokenService
    {
        string GenerateJwtToken(User user);
    }
}