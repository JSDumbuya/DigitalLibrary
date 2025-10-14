using DigitalLibrary.API.DTOs;
using DigitalLibrary.API.Models;
using DigitalLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace DigitalLibrary.API.Controllers;

/// <summary>
/// Handles operations related to users.
/// </summary>
/// <remarks>
/// Provides endpoints for managing users in the system.  
/// Each user can have exactly one associated library.
/// </remarks>
[ApiController]
[Route("api/users/{userId:int}")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Retrieves a specific user by their ID.
    /// </summary>
    /// <param name="userId">The unique identifier of the user to retrieve.</param>
    /// <returns>
    /// A <see cref="UserReadDTO"/> containing user details.
    /// </returns>
    /// <response code="200">Successfully retrieved the user.</response>
    /// <response code="404">No user found with the specified ID.</response>
    [HttpGet]
    public async Task<ActionResult<UserReadDTO>> GetUser([FromRoute] int userId)
    {
        var user = await _userService.GetUserByIdAsync(userId);
        if (user == null) return NotFound();

        var userDTO = MapperUserToReadDTO(user);
        return Ok(userDTO);

    }

    /// <summary>
    /// Creates a new user in the system.
    /// </summary>
    /// <param name="userCreateDTO">The data required to create a new user.</param>
    /// <returns>
    /// The newly created <see cref="UserReadDTO"/> object.
    /// </returns>
    /// <response code="201">User successfully created.</response>
    /// <response code="400">Invalid data supplied for user creation.</response>
    [HttpPost]
    public async Task<ActionResult<UserReadDTO>> CreateUser([FromBody] UserCreateDTO userCreateDTO)
    {
        var toUser = MapperUserCreateDtoToUser(userCreateDTO);
        var newUser = await _userService.AddUserAsync(toUser);
        var toDto = MapperUserToReadDTO(newUser);

        return CreatedAtAction(nameof(GetUser), new { userId = newUser.Id }, toDto);
    }

    /// <summary>
    /// Updates an existing user.
    /// </summary>
    /// <param name="userUpdateDTO">The updated user data.</param>
    /// <param name="userId">The ID of the user to update.</param>
    /// <response code="204">User successfully updated.</response>
    /// <response code="404">No user found with the specified ID.</response>
    [HttpPut]
    public async Task<IActionResult> UpdateUser([FromBody] UserUpdateDTO userUpdateDTO, [FromRoute] int userId)
    {
        var toUser = MapperUserUpdateDtoToUser(userUpdateDTO, userId);
        var updatedUser = await _userService.UpdateUserAsync(toUser);
        if (!updatedUser) return NotFound();
        return NoContent();

    }

    /// <summary>
    /// Deletes a user by their ID.
    /// </summary>
    /// <param name="userId">The unique identifier of the user to delete.</param>
    /// <response code="204">User successfully deleted.</response>
    /// <response code="404">No user found with the specified ID.</response>
    [HttpDelete]
    public async Task<IActionResult> DeleteUser([FromRoute] int userId)
    {
        var deleted = await _userService.DeleteUserAsync(userId);
        if (!deleted) return NotFound();
        return NoContent();
    }

    private UserReadDTO MapperUserToReadDTO(User user)
    {
        return new UserReadDTO
        {
            Id = user.Id,
            UserName = user.UserName
        };
    }

    private User MapperUserCreateDtoToUser(UserCreateDTO userCreateDTO)
    {
        return new User
        {
            UserName = userCreateDTO.UserName,
        };
    }

    private User MapperUserUpdateDtoToUser(UserUpdateDTO userUpdateDTO, int id)
    {
        return new User
        {
            Id = id,
            UserName = userUpdateDTO.UserName
        };
    }
}