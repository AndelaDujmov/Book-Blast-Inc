using BookBlastInc.Application.Services;
using BookBlastInc.Core.Dto;
using BookBlastInc.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BookBlastInc.Web.Pages.BookPages;

public class AddBook : PageModel
{
    private readonly BookService _service;
    public BookAuthorDto BookAuthor{ get; set; } = new BookAuthorDto();
    public Guid CategoryId { get; set; } = Guid.NewGuid();
    public Guid Author { get; set; } = Guid.NewGuid();

    public AddBook(BookService service)
    {
        _service = service;
    }
    
    public void OnGet()
    {
        BookAuthor.SelectListItems = _service.GetSelectList();
        BookAuthor.Categories = _service.GetCategories();
        BookAuthor.Book = new Book();
    }

    public IActionResult OnPost()
    {
        if (ModelState.IsValid)
        {
            try
            {
                _service.AddBookToAuthor(id: BookAuthor.Author, book: BookAuthor.Book);
                _service.AddCategoryToBook(id: BookAuthor.Book.Id, category: CategoryId);

                return RedirectToPage("AllBooks");
            }
            catch (Exception e)
            {
                RedirectToPage("");
            }
        }

        return RedirectToPage("");
    }
}