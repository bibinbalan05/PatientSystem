using MediatR;
using Microsoft.Data.SqlClient;
using Patient.Application.Queries.LoginRequest;
using Patient.Domain.Entities.Models;
using Patient.Infrastructure.Data;
using System.Data;


namespace Patient.Application.Queries
{
    public class LoginRequestQuery : IRequest<LoginResponse>
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public LoginRequestQuery(string userName,string password)
        {
            Username = userName;
            Password = password;
        }
        public class LoginRequestQueryHandler : IRequestHandler<LoginRequestQuery, LoginResponse>
        {
            private readonly IDbHelper _dbHelper;
            public LoginRequestQueryHandler(IDbHelper dbHelper)
            {
                _dbHelper = dbHelper;
            }

            public async Task<LoginResponse> Handle(LoginRequestQuery request, CancellationToken cancellationToken)
            {
                await using var conn = _dbHelper.GetConnection();

                await using var command = conn.CreateCommand();
                command.CommandText = "SELECT PasswordHash  FROM Users WHERE Email = @Username";

                command.Parameters.Add(new SqlParameter("@Username", SqlDbType.NVarChar) { Value = request.Username });
                var storedPasswordHash = await command.ExecuteScalarAsync(cancellationToken) as string;

                if (storedPasswordHash != null && BCrypt.Net.BCrypt.Verify(request.Password, storedPasswordHash))
                {
                    return new LoginResponse
                    {
                        Success = true,
                        User = new User
                        {
                            UserName=request.Username
                        }
                    };
                }

                return new LoginResponse
                {
                    Success = false,                    
                };
            }

        }
    }
}
