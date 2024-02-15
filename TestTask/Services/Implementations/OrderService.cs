using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Enums;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services;

public class OrderService : IOrderService
{
    private readonly ApplicationDbContext _db;

    public OrderService(ApplicationDbContext db)
    {
        _db = db;
    }
    
    public async Task<Order> GetOrder()
    {
        var orderWithHighestCost = await _db.Orders
            .OrderByDescending(o => o.Quantity * o.Price)
            .FirstOrDefaultAsync();

        return orderWithHighestCost;
    }

    public async Task<List<Order>> GetOrders()
    {
        return await _db.Orders
            .Where(o => o.Quantity > 10)
            .ToListAsync();
    }
}