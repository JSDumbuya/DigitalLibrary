using DigitalLibrary.API.DTOs;
using DigitalLibrary.API.Services;
using DigitalLibrary.API.Common;
using Microsoft.AspNetCore.Mvc;

namespace DigitalLibrary.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authService = authenticationService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDTO>> Register([FromBody] RegisterDTO registerDto)
    {
        var result =  await _authService.RegisterAsync(registerDto);
        if (!result.IsSuccess)
        {
            return result.Type switch
            {
                ErrorType.UserAlreadyExists => Conflict(result.Message),
                _ => StatusCode(StatusCodes.Status500InternalServerError)
            };
        }

        return Ok(result.Value);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDTO>> Login(LoginDTO loginDto)
    {
        var result = await _authService.LoginAsync(loginDto);
        if (!result.IsSuccess)
        {
            return result.Type switch
            {
                ErrorType.InvalidCredentials => Unauthorized(result.Message),
                _ => StatusCode(StatusCodes.Status500InternalServerError)
            };
        }

        return Ok(result.Value);
    }
}
