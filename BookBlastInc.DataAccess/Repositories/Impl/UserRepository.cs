using BookBlastInc.Core.Entities;

namespace BookBlastInc.DataAccess.Repositories.Impl;

public class UserRepository : Repository<ApplicationUser>, IUserRepository
{
    public UserRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}