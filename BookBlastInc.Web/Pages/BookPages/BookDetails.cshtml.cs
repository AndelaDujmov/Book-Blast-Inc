using System.Security.Claims;
using BookBlastInc.Application.Services;
using BookBlastInc.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookBlastInc.Web.Pages.BookPages;

public class BookDetails : PageModel
{
    [BindProperty]
    public Book Book { get; set; }
    [BindProperty]
    public int Count { get; set; }
    private readonly BookService _bookService;

    public BookDetails(BookService bookService)
    {
        _bookService = bookService;
    }

    [Authorize]
    [HttpGet]
    public IActionResult OnGet(Guid id)
    {
        Book = _bookService.GetBook(id);
        
        if (!User.Identity.IsAuthenticated)
            RedirectToRoute("/Identity/Account/Login");
        
        return Page();
      }
    
    [Authorize]
    [HttpPost]
    public IActionResult OnPost()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        
        _bookService.AddToCart(userId:claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value,
            count:Count, bookId:Book.Id);
        
        return RedirectToPage("AllBooks");
    }
}