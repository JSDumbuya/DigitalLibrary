using System.ComponentModel.DataAnnotations;

namespace DigitalLibrary.API.DTOs;

public class UserReadDTO
{ 
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
}

/*public class UserCreateDTO
{ 
    
    [Required]
    public string UserName { get; set; } = string.Empty;
}*/

public class UserUpdateDTO
{
    public string? UserName { get; set; }
    public string? Password { get; set; }

}

//Replaces createDTO
public class RegisterDTO
{
    [Required]
    public required string UserName { get; set; } = string.Empty;
    [Required]
    public required string Password { get; set; } = string.Empty;
}

public class LoginDTO
{
    [Required]
    public required string UserName { get; set; } = string.Empty;
    [Required]
    public required string Password { get; set; } = string.Empty;
}

public class AuthResponseDTO
{
    public string Token { get; set; } = string.Empty;
    public UserReadDTO User {get; set; } = null!;
}