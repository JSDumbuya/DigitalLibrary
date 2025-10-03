namespace DigitalLibrary.API.Data;

using DigitalLibrary.API.Models;

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

    public async Task<bool> DeleteAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return false;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
        
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }
    public async Task<bool> UpdateAsync(User user)
    {
        var existingUser = await _context.Users.FindAsync(user.Id);
        if (existingUser == null) return false;

        existingUser.UserName = user.UserName;
        await _context.SaveChangesAsync();
        return true;
    }
}