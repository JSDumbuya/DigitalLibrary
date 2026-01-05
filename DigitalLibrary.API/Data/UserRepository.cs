namespace DigitalLibrary.API.Data;

using DigitalLibrary.API.Models;
using Microsoft.EntityFrameworkCore;

public class UserRepository: IUserRepository
{
    private readonly DigitalLibraryContext _context;
    public UserRepository(DigitalLibraryContext context)
    {
        _context = context;
    }

    public async Task<User> AddAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<User?> GetUserByUserNameAsync(string username)
    {
        return await _context.Users.FirstOrDefaultAsync(user => user.UserName == username);
    }

    public async Task<bool> UserExitsAsync(string username)
    {
        var user = await _context.Users.FirstOrDefaultAsync(user => user.UserName == username);
        if (user == null) return false;
        return true;
    }
}