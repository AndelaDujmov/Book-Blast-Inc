using BookBlastInc.Application.Services;
using BookBlastInc.Core.Dto;
using BookBlastInc.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BookBlastInc.Web.Pages.BookPages;
[BindProperties]
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
    
    public IActionResult OnGet()
    {
        var authorIds = new List<Guid>();
        BookAuthor.SelectListItems = _service.GetSelectList();
        BookAuthor.Categories = _service.GetCategories();
        BookAuthor.Book = new Book();
        return Page();
    }

    public IActionResult OnPost()
    {
        if (ModelState.IsValid)
        {
            try
            {
                _service.Add(BookAuthor.Book);
                _service.AddCategoryToBook(book: BookAuthor.Book, category: CategoryId);
                
                BookAuthor.AuthorIds.ToList().ForEach(x =>
                {
                    var bookauthor = new BookAuthor();
                    bookauthor.Authorid = x;
                    bookauthor.BookId = BookAuthor.Book.Id;
                    _service.AddBookToAuthor(bookauthor, BookAuthor.Book);
                });
                

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