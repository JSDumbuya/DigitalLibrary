using System.ComponentModel.DataAnnotations;

namespace DigitalLibrary.API.DTOs;

public class UserReadDTO
{ 
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
}

public class UserCreateDTO
{ 
    //Update when implementing auth, potentially more fields needed
    [Required]
    public string UserName { get; set; } = string.Empty;
}

public class UserUpdateDTO
{
    [Required]
    public string UserName { get; set; } = string.Empty; 
}