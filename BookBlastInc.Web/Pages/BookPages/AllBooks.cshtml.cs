using BookBlastInc.Application.Services;
using BookBlastInc.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookBlastInc.Web.Pages.BookPages;

[BindProperties]
public class AllBooks : PageModel
{

    private readonly BookService _bookService;
    
    public IEnumerable<Book> Books { get; set; }
    
    public AllBooks(BookService bookService)
    {
        _bookService = bookService;
    }
    
    public IActionResult OnGet()
    {
        Books = _bookService.GetBooks();

        return Page();
    }
    
}