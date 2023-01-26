using System.IdentityModel.Tokens.Jwt;
using System.Text;
using LegiosoftTestTask.Extension;
using LegiosoftTestTask.Models;
using LegiosoftTestTask.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace LegiosoftTestTask.Services.Implementation;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;

    public AuthService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string AuthenticateAsync(AuthModel authModel)
    {
       var encodedJwt = new JwtSecurityTokenHandler().WriteToken(GetToken());
        return encodedJwt;
    }

    private JwtSecurityToken GetToken()
    {
        var options = _configuration.GetSection("AuthOptions");
        var issuer = options.GetSection("Issuer").Value;
        var audience = options.GetSection("Audience").Value;
        var now = DateTime.Now;
        var jwt = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            notBefore:now,
            expires: now.Add(TimeSpan.FromHours(10)),
            signingCredentials: new SigningCredentials(AuthExtension.GetSymmetricSecurityKey(_configuration), SecurityAlgorithms.HmacSha256));
        return jwt;
    }
}