using BookBlastInc.Core.Entities;
using BookBlastInc.Core.Enums;
using OrderStatus = BookBlastInc.DataAccess.Migrations.OrderStatus;

namespace BookBlastInc.DataAccess.Repositories.Impl;

public class OrderRepository : Repository<Order>, IOrderRepository
{
    private readonly AppDbContext _dbContext;
    public OrderRepository(AppDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
    
}