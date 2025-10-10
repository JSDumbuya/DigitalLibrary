using DigitalLibrary.API.DTOs;
using DigitalLibrary.API.Models;
using DigitalLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace DigitalLibrary.API.Controllers;

[ApiController]
[Route("api/users/{userId:int}")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;
    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<UserReadDTO>> GetUser([FromRoute] int userId)
    {
        var user = await _userService.GetUserByIdAsync(userId);
        if (user == null) return NotFound();

        var userDTO = MapperUserToReadDTO(user);
        return Ok(userDTO);

    }

    [HttpPost]
    public async Task<ActionResult<UserReadDTO>> CreateUser([FromBody] UserCreateDTO userCreateDTO)
    {
        var toUser = MapperUserCreateDtoToUser(userCreateDTO);
        var newUser = await _userService.AddUserAsync(toUser);
        var toDto = MapperUserToReadDTO(newUser);

        return CreatedAtAction(nameof(GetUser), new { userId = newUser.Id }, toDto);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateUser([FromBody] UserUpdateDTO userUpdateDTO, [FromRoute] int userId)
    {
        var toUser = MapperUserUpdateDtoToUser(userUpdateDTO, userId);
        var updatedUser = await _userService.UpdateUserAsync(toUser);
        if (!updatedUser) return NotFound();
        return NoContent();

    }

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