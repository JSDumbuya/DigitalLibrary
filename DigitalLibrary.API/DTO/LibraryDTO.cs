using System.ComponentModel.DataAnnotations;

namespace DigitalLibrary.API.DTOs;

public class LibraryReadDTO
{
    public int Id { get; set; }
    public string LibraryName { get; set; } = string.Empty;
    public string? LibraryDescription { get; set; }
}

public class LibraryCreateDTO
{
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