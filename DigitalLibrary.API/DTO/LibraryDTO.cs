using System.ComponentModel.DataAnnotations;

namespace DigitalLibrary.API.DTOs;

public class LibraryReadDTO
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string LibraryName { get; set; } = string.Empty;
    public string? LibraryDescription { get; set; }
}

public class LibraryCreateDTO
{
    //Update when adding auth: get it from the logged in user, instead of from the frontend
    public int UserId { get; set; }
    [Required]
    public string LibraryName { get; set; } = string.Empty;
    public string? LibraryDescription { get; set; }
}

public class LibraryUpdateDTO
{
    [Required]
    public string LibraryName { get; set; } = string.Empty;
    public string? LibraryDescription { get; set; }
}