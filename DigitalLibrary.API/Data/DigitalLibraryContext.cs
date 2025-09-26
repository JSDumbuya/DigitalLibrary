using DigitalLibrary.API.Models;
using Microsoft.EntityFrameworkCore;

public class DigitalLibraryContext : DbContext
{

    public DigitalLibraryContext(DbContextOptions<DigitalLibraryContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Library> Libraries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Library>()
            .HasOne(library => library.User)
            .WithOne()
            .HasForeignKey<Library>(library => library.UserId);
    }

}