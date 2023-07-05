using BookBlastInc.Application.Services;
using BookBlastInc.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookBlastInc.Web.Pages.BookPages;

[BindProperties]
public class AddAuthor : PageModel
{
    private readonly BookService _service;
    public Author BookAuthor { get; set; }

    public AddAuthor(BookService service)
    {
        _service = service;
    }

    public void OnGet()
    {
        
    }

    public IActionResult OnPost()
    {
        _service.Add(BookAuthor);
        return RedirectToPage("");
    }
}