using Moq;
using Xunit;
using Patient.Application.Queries;
using Patient.Application.Services;
using Patient.Domain.Entities.Models;
using MediatR;
using API.Controllers.Controllers;
using Patient.API.Controllers.Auth.Requests;
using Microsoft.AspNetCore.Mvc;
using Patient.Application.Queries.LoginRequest;
using Patient.API.Controllers.Auth.Responses;


namespace Patient.Test.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _tokenServiceMock = new Mock<ITokenService>();
            _mediatorMock = new Mock<IMediator>();
            _controller = new AuthController(_tokenServiceMock.Object, _mediatorMock.Object);
        }
        [Fact]
        public async Task Login_ReturnsOkResult_WhenCredentialsAreValid()
        {
            // Arrange
            var loginRequest = new LoginRequest { UserName = "validuser", PassWord = "validpassword" };
           
            var userResponse = new LoginResponse
            {
                User = new User { UserName = "validuser" }
            };
            
            _mediatorMock.Setup(m => m.Send(It.IsAny<LoginRequestQuery>(), default))
                    .ReturnsAsync(userResponse);

            _tokenServiceMock.Setup(t => t.GenerateJwtToken(It.IsAny<User>()))
                             .Returns("valid_jwt_token");

            // Act
            var result = await _controller.Login(loginRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<UserResponse>(okResult.Value);
            Assert.Equal("valid_jwt_token", returnValue.Token);
            Assert.Equal("validuser", returnValue.User.UserName);
        }

        [Fact]
        public async Task Login_ThrowsException_WhenMediatorThrowsException()
        {
            // Arrange
            var loginRequest = new LoginRequest { UserName = "validuser", PassWord = "validpassword" };
            _mediatorMock.Setup(m => m.Send(It.IsAny<LoginRequestQuery>(), default))
                .ThrowsAsync(new Exception("Mediator exception"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _controller.Login(loginRequest));
            Assert.Equal("Mediator exception", exception.Message);
        }
        [Fact]
        public async Task Login_ReturnsUnauthorized_WhenCredentialsAreInvalid()
        {
            // Arrange
            var loginRequest = new LoginRequest { UserName = "invaliduser", PassWord = "invalidpassword" };
            
            _mediatorMock.Setup(m => m.Send(It.IsAny<LoginRequestQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync((LoginResponse)null); 

            // Act
            var result = await _controller.Login(loginRequest);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Invalid username or password", unauthorizedResult.Value);
        }

    }
}
