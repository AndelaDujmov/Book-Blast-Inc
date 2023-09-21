using System.Security.Claims;
using BookBlastInc.Application.Services;
using BookBlastInc.Application.Utils;
using BookBlastInc.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookBlastInc.Web.Pages;
[BindProperties]
public class IndexModel : PageModel
{
    public IEnumerable<Category> Categories { get; set; } = new List<Category>();
    public IEnumerable<Book> Books { get; set; } = new List<Book>();
    
    private readonly CategoryService _service;
    private readonly BookService _bookService;

    public IndexModel(CategoryService service, BookService bookService)
    {
        _service = service;
        _bookService = bookService;
    }
    
    public void OnGet()
    {
        Categories = _service.GetAll();

        Books = _bookService.GetBooks();
    }
}