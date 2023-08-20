using BookBlastInc.Core.Entities;

namespace BookBlastInc.DataAccess.Repositories.Impl;

public class BookOrderRepository : Repository<OrderBook>, IBookOrderRepository
{
    public BookOrderRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
    
}