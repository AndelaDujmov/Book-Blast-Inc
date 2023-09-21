using System.Security.Claims;
using BookBlastInc.Application.Services;
using BookBlastInc.Application.Utils;
using BookBlastInc.Core.Entities;
using BookBlastInc.Core.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookBlastInc.Web.Pages.LoanPages;

public class Index : PageModel
{
    private readonly BookService _service;
    [BindProperty]
    public IEnumerable<BookOnLoan> BooksOnLoans { get; set; }
    [BindProperty]
    public string CurrentFilter { get; set; }
    [BindProperty]
    public PaginatedList<BookOnLoan> PaginatedListLoans { get; set; }

    public Index(BookService service)
    {
        _service = service;
    }
    
    public void OnGet(string sortOrder, string searchString, int? pageIndex)
    {
        ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
        
        CurrentFilter = searchString;

        
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var loans = User.IsInRole(RoleName.Administrator.ToString()) || User.IsInRole(RoleName.Employee.ToString())
            ? _service.GetAllLoans()
            : _service.GetLoans(claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value);
        
        if (!string.IsNullOrEmpty(searchString))
        {
            loans = loans.Where(o => o.Book.Name.ToString().Contains(searchString) || o.User.FirstName.Contains(searchString) || o.User.LastName.Contains(searchString));
        }
        
        switch (sortOrder)
        {
            case "name_desc":
                loans = loans.OrderByDescending(x => x.Book.Name);
                break;
            default:
                loans =  loans.OrderBy(x => x.Book.Name);
                break;
        }
        
        var pageSize = 4;
        var PaginatedListLoans = PaginatedList<BookOnLoan>.CreateList(loans, pageIndex ?? 1, pageSize);

        BooksOnLoans = PaginatedListLoans.ToList();
    }
}