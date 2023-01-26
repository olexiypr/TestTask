
using LegiosoftTestTask.Models;

namespace LegiosoftTestTask.Services.Interfaces;

public interface IAuthService
{
    string AuthenticateAsync(AuthModel authModel);
}