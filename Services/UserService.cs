using Microsoft.EntityFrameworkCore;
using TxtCreatorBOT.Database;
using TxtCreatorBOT.Database.Models;

namespace TxtCreatorBot.Services;

public class UserService
{
    private readonly TxtCreatorDbContext _dbContext;

    public UserService(TxtCreatorDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UserModel> GetUserModelAsync(ulong id)
    {
        var receivedUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId == id);
        if (receivedUser != null) return receivedUser;
        var createdUser = await _dbContext.Users.AddAsync(new()
        {
            UserId = id,
        });
        await _dbContext.SaveChangesAsync();
        return createdUser.Entity;
    }
    
    public async Task<bool> AddWarnsAsync(ulong id, long value)
    {
        var receivedUser = await GetUserModelAsync(id);
        receivedUser.Warns += value;
        if (receivedUser.Warns < 0) return false;
        await _dbContext.SaveChangesAsync();
        return true;
    }
    
    public async Task RemoveUserModelAsync(ulong id)
    {
        var receivedUser = await GetUserModelAsync(id);
        _dbContext.Remove(receivedUser);
        await _dbContext.SaveChangesAsync();
    }
    
    public async Task<Dictionary<int, List<UserModel>>> GetAllUserModelsAsync()
    {
        var dictionary = new Dictionary<int, List<UserModel>>();
        var allProfiles = await _dbContext.Users.OrderByDescending(user => user.Warns).ToListAsync();
        var i = 0;
        while (allProfiles.Count > 0)
        {
            var tenProfiles = allProfiles.Take(10).ToList();
            tenProfiles.ForEach(profile => allProfiles.Remove(profile));
            dictionary.Add(i, tenProfiles);
            i++;
        }
        return dictionary;
    }
}