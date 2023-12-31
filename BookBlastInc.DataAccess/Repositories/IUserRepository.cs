using BookBlastInc.Core.Entities;

namespace BookBlastInc.DataAccess.Repositories;

public interface IUserRepository : IRepository<ApplicationUser>
{
    string GetRoleByUser(string userId);
}