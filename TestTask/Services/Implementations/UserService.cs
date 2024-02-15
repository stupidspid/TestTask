using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Enums;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _db;

    public UserService(ApplicationDbContext db)
    {
        _db = db;
    }
    public async Task<User> GetUser()
    {
        var userWithMostOrders = await _db.Orders
            .GroupBy(order => order.UserId) 
            .Select(group => new
            {
                UserId = group.Key,
                TotalOrders = group.Count()
            })
            .OrderByDescending(group => group.TotalOrders)
            .FirstOrDefaultAsync();

        var userValue = await _db.Users.FirstOrDefaultAsync(u=> userWithMostOrders.UserId == u.Id);
        return userValue;
    }

    public async Task<List<User>> GetUsers()
    {
        return await _db.Users
            .Where(u => u.Status == UserStatus.Inactive)
            .ToListAsync();
    }
}