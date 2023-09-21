using BookBlastInc.Core.Entities;

namespace BookBlastInc.DataAccess.Repositories.Impl;

public class UserRepository : Repository<ApplicationUser>, IUserRepository
{
    private readonly AppDbContext _dbContext;
    public UserRepository(AppDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public string GetRoleByUser(string userId)
    {
        var userrole = (from roleuser in _dbContext.UserRoles
            where roleuser.UserId.Equals(userId) 
            select roleuser.RoleId).FirstOrDefault();

        var role = (from r in _dbContext.Roles
            where r.Id.Equals(userrole) 
            select r.Name).FirstOrDefault();

        return role;
    }
}