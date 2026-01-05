using DigitalLibrary.API.DTOs;
using DigitalLibrary.API.Services;
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
        try
        {
            var authResponseDTO = await _authService.RegisterAsync(registerDto);
            return Ok(authResponseDTO);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDTO>> Login(LoginDTO loginDto)
    {
        try
        {
            var authResponseDTO = await _authService.LoginAsync(loginDto);
            return Ok(authResponseDTO);
        }
        catch (Exception ex)
        {
            
            return NotFound(ex.Message);
        }
    }
}
