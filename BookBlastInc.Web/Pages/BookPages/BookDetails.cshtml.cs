using System.Security.Claims;
using BookBlastInc.Application.Services;
using BookBlastInc.Core.Entities;
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
    
    public IActionResult OnGet(Guid id)
    {
        Book = _bookService.GetBook(id);
        
        return Page();
    }

    public IActionResult OnPost()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var shoppingCard = new ShoppingCart();
        shoppingCard.BookId = Book.Id;
        shoppingCard.UserId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        shoppingCard.Count = Count;

        return RedirectToPage("");
    }
}