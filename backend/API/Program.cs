using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Patient.API.Middleware;
using Patient.Application.Commands;
using Patient.Application.Services;
using Patient.Infrastructure.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;
var key = Encoding.UTF8.GetBytes(config["JwtSettings:SecretKey"]);
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(opt =>
    {
        opt.RequireHttpsMetadata = false;
        opt.SaveToken = true;
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateLifetime = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = config["JwtSettings:Issuer"],
            ValidAudience = config["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key),
        };
});

builder.Services.AddCors();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Name = "Authorization",
        Scheme = "bearer",
        In = ParameterLocation.Header,
        Description = "Type into the textbox: Bearer {your JWT token}."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
});
builder.Services.AddScoped<IDbHelper,DbHelper>();
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddMediatR(typeof(AddPatientCommand));
builder.Services.AddControllers();


var app = builder.Build();
app.UseMiddleware<LoggingMiddleware>();
app.UseAuthorization();
app.UseSwagger(c =>
{
    c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
    {
        swaggerDoc.Servers = new List<OpenApiServer>
                    {
                        new OpenApiServer { Url = $"https://{httpReq.Host.Value}" }
                    };
    });
});
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
});
app.UseHttpsRedirection();
app.UseCors(builder => builder
                .SetIsOriginAllowedToAllowWildcardSubdomains()
                .WithOrigins(
                    
                    "http://localhost:3000")
                .AllowAnyMethod()
                .AllowAnyHeader());

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
