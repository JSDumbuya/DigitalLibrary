namespace DigitalLibrary.API.DTOs;

using System.ComponentModel.DataAnnotations;
using DigitalLibrary.API.Models;

public class BookReadDTO
{
    public int Id { get; set; }
    public string BookTitle { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public BookStatus BookStatus { get; set; }
    public string? Review { get; set; }
    public StarRating? StarRating { get; set; }
    public BookGenre? Genre { get; set; }
    public DateTime DateAdded { get; set; }
    public DateTime? DateFinished { get; set; }
}

public class BookCreateDTO
{
    [Required]
    public string BookTitle { get; set; } = string.Empty;
    [Required]
    public string Author { get; set; } = string.Empty;
    [Required]
    public BookStatus BookStatus { get; set; }
    public string? Review { get; set; }
    public StarRating? StarRating { get; set; }
    public BookGenre? Genre { get; set; }
    [Required]
    public DateTime DateAdded { get; set; }
}

public class BookUpdateDTO
{
    [Required]
    public string BookTitle { get; set; } = string.Empty;
    [Required]
    public string Author { get; set; } = string.Empty;
    [Required]
    public BookStatus BookStatus { get; set; }
    public string? Review { get; set; }
    public StarRating? StarRating { get; set; }
    public BookGenre? Genre { get; set; }
    public DateTime? DateFinished { get; set; }
}