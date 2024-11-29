using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Patient.API.Controllers.Auth.Requests;
using Patient.API.Controllers.Auth.Responses;
using Patient.Application.Queries;
using Patient.Application.Services;
using Patient.Domain.Entities.Models;

namespace API.Controllers.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        
        private readonly ITokenService _tokenService;
        private readonly IMediator _mediator;

        public AuthController(ITokenService tokenService, IMediator mediator)
        {
            _tokenService = tokenService;
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            try
            {
                var userResponse = await _mediator.Send(new LoginRequestQuery(loginRequest.UserName,loginRequest.PassWord));
                if (userResponse == null || userResponse.User == null)
                {                   
                    return Unauthorized("Invalid username or password");
                }
                var token = _tokenService.GenerateJwtToken(userResponse.User);

                return Ok(new UserResponse
                {
                    Token = token,
                    User = new User {
                        UserName = userResponse.User.UserName
                    }
                });                
            }
            catch (Exception ex)
            {

                throw;
            }
            

        }
    }
}