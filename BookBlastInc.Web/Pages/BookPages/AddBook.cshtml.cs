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
    private readonly IWebHostEnvironment _webHostEnvironment;
    public BookAuthorDto BookAuthor{ get; set; } = new BookAuthorDto();
    public Guid CategoryId { get; set; } = Guid.NewGuid();
    public Guid Author { get; set; } = Guid.NewGuid();


    public AddBook(BookService service, IWebHostEnvironment webHostEnvironment)
    {
        _service = service;
        _webHostEnvironment = webHostEnvironment;
    }
    
    public IActionResult OnGet(Guid? id)
    {
        var authorIds = new List<Guid>();
        BookAuthor.SelectListItems = _service.GetSelectList();
        BookAuthor.Categories = _service.GetCategories();
        BookAuthor.Book = id.Equals(null) ? new Book() : _service.GetBook(id);
        
        return Page();
    }

    public IActionResult OnPost(IFormFile? file)
    {
        
        if (!_service.IfBookExists(BookAuthor.Book))
        {
            string wwwrootPath = _webHostEnvironment.WebRootPath;

            if (file is not null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string prodPath = Path.Combine(wwwrootPath, @"books");

                if (!string.IsNullOrEmpty(BookAuthor.Book.PhotoUrl))
                {
                    var oldImgPath = Path.Combine(wwwrootPath, BookAuthor.Book.PhotoUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(oldImgPath)) System.IO.File.Delete(oldImgPath);
                }

                using (var filestream = new FileStream(Path.Combine(prodPath, fileName), FileMode.Create))
                {
                    file.CopyTo(filestream);
                }

                BookAuthor.Book.PhotoUrl = "/books/" + fileName;
            }

            BookAuthor.Book.CategoryId = CategoryId;

            _service.AddBook(book: BookAuthor.Book);
            BookAuthor.AuthorIds.ToList().ForEach(x =>
            {
                var bookauthor = new BookAuthor();
                bookauthor.Authorid = x;
                bookauthor.BookId = BookAuthor.Book.Id;

                _service.AddBookToAuthor(bookauthor, BookAuthor.Book);
            });
            
            
        }
        else
        {
            _service.UpdateBook(book: BookAuthor.Book);
            BookAuthor.AuthorIds.ToList().ForEach(x =>
            {
                var bookauthor = new BookAuthor();
                bookauthor.Authorid = x;
                bookauthor.BookId = BookAuthor.Book.Id;

                _service.UpdateBookAuthor(bookauthor, BookAuthor.Book);
            });
        }

        return RedirectToPage("AllBooks");
    }
}