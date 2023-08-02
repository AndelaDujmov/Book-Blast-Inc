using BookBlastInc.Application.Services;
using BookBlastInc.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookBlastInc.Web.Pages.BookPages;

public class GetBooksByCategory : PageModel
{

    private readonly BookService _service;
    [BindProperty]
    public IEnumerable<Book> Books { get; set; }

    public GetBooksByCategory(BookService service)
    {
        _service = service;
    }
    
    public void OnGet(Guid id)
    {
        Books = _service.GetAllBooksByCategory(id);
    }
}