using BookBlastInc.Core.Entities;
using BookBlastInc.Core.Enums;
using BookBlastInc.DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookBlastInc.DataAccess.DbInitalizer;

public class DbInitializer : IDbInitializer
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly AppDbContext _appDbContext;

    public DbInitializer(RoleManager<IdentityRole> roleManager,
                         UserManager<IdentityUser> userManager,
                         AppDbContext appDbContext)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _appDbContext = appDbContext;
    }

    private ApplicationUser? GetUserByUsername(string username)
    {
        return _appDbContext.ApplicationUsers.SingleOrDefault(x => x.UserName.Equals(username));
    }
    
    public void Initialize()
    {
        try
        {
            if (_appDbContext.Database.GetPendingMigrations().Count() > 0)
            {
                _appDbContext.Database.Migrate();
            }
        }
        catch(Exception e)
        {
            
        }
        
        if (!_roleManager.RoleExistsAsync(RoleName.Administrator.ToString()).GetAwaiter().GetResult()) 
            _roleManager.CreateAsync(new IdentityRole(RoleName.Administrator.ToString())).GetAwaiter().GetResult();
        if (!_roleManager.RoleExistsAsync(RoleName.User.ToString()).GetAwaiter().GetResult()) 
            _roleManager.CreateAsync(new IdentityRole(RoleName.User.ToString())).GetAwaiter().GetResult();
        if (!_roleManager.RoleExistsAsync(RoleName.Employee.ToString()).GetAwaiter().GetResult()) 
            _roleManager.CreateAsync(new IdentityRole(RoleName.Employee.ToString())).GetAwaiter().GetResult();

        _userManager.CreateAsync(new ApplicationUser
        {
            UserName = "andela_dujmov",
            Email = "andeladujmov3@gmail.com",
            PhoneNumber = "+3859176543",
            DateOfBirth = new DateTime(1999, 7, 4),
            FirstName = "Anđela",
            LastName = "Dujmov",
            Deleted = false,
            Gender = Gender.F
        }, "AdminA040799.").GetAwaiter().GetResult();

        var user = GetUserByUsername("andela_dujmov");
        _userManager.AddToRoleAsync(user, RoleName.Administrator.ToString()).GetAwaiter().GetResult();
    }
}