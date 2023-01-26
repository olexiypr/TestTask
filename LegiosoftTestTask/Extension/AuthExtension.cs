using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace LegiosoftTestTask.Extension;

public static class AuthExtension
{
    public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetSection("AuthOptions");
        var issuer = options.GetSection("Issuer").Value;
        var audience = options.GetSection("Audience").Value;
        var key = options.GetSection("Key").Value;
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateLifetime = true,
                    IssuerSigningKey = GetSymmetricSecurityKey(configuration),
                    ValidateIssuerSigningKey = true
                };
            });
        return services;
    }

    public static SymmetricSecurityKey GetSymmetricSecurityKey(IConfiguration configuration)
    {
        var options = configuration.GetSection("AuthOptions");
        var key = options.GetSection("Key").Value;
        return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
    }
}