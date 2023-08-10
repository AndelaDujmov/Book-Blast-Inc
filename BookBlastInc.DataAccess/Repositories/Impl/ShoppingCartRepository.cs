using BookBlastInc.Core.Entities;

namespace BookBlastInc.DataAccess.Repositories.Impl;

public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
{
    public ShoppingCartRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
    
    
}