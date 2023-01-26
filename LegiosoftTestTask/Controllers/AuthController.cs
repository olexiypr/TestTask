using LegiosoftTestTask.Models;
using LegiosoftTestTask.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LegiosoftTestTask.Controllers;

[ApiController, Route("api/[controller]")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    /// <summary>
    /// Get JWT token
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// POST api/auth
    /// {
    ///     login: "login",
    ///     password: "password"
    /// }
    /// </remarks>
    /// <param name="authModel">Login and password (can be any)</param>
    /// <returns>Jwt token</returns>
    /// <response code="200">Success</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> AuthenticateAsync(AuthModel authModel)
    {
        var token = _authService.AuthenticateAsync(authModel);
        return Ok(token);
    }
}