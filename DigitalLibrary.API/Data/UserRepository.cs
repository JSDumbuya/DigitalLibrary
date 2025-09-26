namespace DigitalLibrary.API.Data;

using DigitalLibrary.API.Models;

public class UserRepository: IUserRepository
{
    private readonly DigitalLibraryContext _context;
    public UserRepository(DigitalLibraryContext context)
    {
        _context = context;
    }

    public async Task AddAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        //Add confirmation message, propagated to user
    }

    public async Task DeleteAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            //Add message
            return;
        }
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }
    public async Task UpdateAsync(User user)
    {
        var existingUser = await _context.Users.FindAsync(user.Id);
        if (existingUser == null)
        {
            //Add message
            return;
        }
        existingUser.UserName = user.UserName;
        await _context.SaveChangesAsync();
    }
}