using System.IdentityModel.Tokens.Jwt;
using DigitalLibrary.API.DTOs;
using DigitalLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;
using DigitalLibrary.API.Common;

namespace DigitalLibrary.API.Controllers;

/// <summary>
/// Handles operations related to users.
/// </summary>
/// <remarks>
/// Provides endpoints for managing users in the system.  
/// Each user can have exactly one associated library.
/// </remarks>
[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Retrieves the currently authenticated user's details.
    /// </summary>
    /// <response code="200">Successfully retrieved the user.</response>
    /// <response code="401">Invalid or missing JWT token.</response>
    /// <response code="404">User not found.</response>
    [HttpGet("me")]
    public async Task<ActionResult<UserReadDTO>> GetMe()
    {
        var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        if (userIdClaim == null) return Unauthorized();
        if (!int.TryParse(userIdClaim, out int userId)) return Unauthorized();

        var result = await _userService.GetUserByIdAsync(userId);
         if (!result.IsSuccess)
        {
            return result.Type switch
            {
                ErrorType.UserNotFound => NotFound(result.Message),
                _ => StatusCode(StatusCodes.Status500InternalServerError)
            };
        }

        return Ok(result.Value);
    }
}