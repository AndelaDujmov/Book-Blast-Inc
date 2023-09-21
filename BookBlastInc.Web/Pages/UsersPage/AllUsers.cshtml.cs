using System.Security.Claims;
using BookBlastInc.Application.Services;
using BookBlastInc.Application.Utils;
using BookBlastInc.Core.Entities;
using BookBlastInc.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookBlastInc.Web.Pages.UsersPage;

[Authorize]
public class AllUsers : PageModel
{
    private readonly BookService _service;
    [BindProperty]
    public IEnumerable<ApplicationUser> Users { get; set; }
    public PaginatedList<ApplicationUser> PaginatedUsers { get; set; }

    public string CurrentFilter { get; set; }
    public AllUsers(BookService service)
    {
        _service = service;
    }
    
    public void OnGet(string sortOrder, string searchString, int? pageIndex)
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var users = (User.IsInRole(RoleName.Administrator.ToString()))
            ? _service.GetAllUsers(claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value)
            : _service.GetAllClients();
        
        CurrentFilter = searchString;
       
        switch (sortOrder)
        {
            case "fname_desc":
                users = users.OrderByDescending(x => x.FirstName);
                break;
            case "lname_desc":
                users = users.OrderByDescending(x => x.LastName);
                break;
            default:
                users =  users.OrderBy(x => x.Name);
                break;
        }
        
        var pageSize = 4;
        PaginatedUsers = PaginatedList<ApplicationUser>.CreateList(users, pageIndex ?? 1, pageSize);

        Users = PaginatedUsers.ToList();
    }
}