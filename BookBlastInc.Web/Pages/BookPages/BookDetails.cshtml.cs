using System.Security.Claims;
using BookBlastInc.Application.Services;
using BookBlastInc.Application.Utils;
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
        
        return Page();
    }
    
    [Authorize]
    [HttpPost]
    public IActionResult OnPost()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        
        var shoppingCard = new ShoppingCart();
        shoppingCard.BookId = Book.Id;
        shoppingCard.UserId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        shoppingCard.Count = Count;

        var cartByDb =
            _bookService.GetShoppingCartIfExists(userId: claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value,
                bookId: Book.Id);

        if (cartByDb is not null)
        {
            cartByDb.Count += Count;
            _bookService.UpdateCart(cartByDb);
        }
        else
        {
            _bookService.AddtoCart(shoppingCard);
        }

        return RedirectToPage("BookDetails", new { id = Book.Id });
    }
    
}