using BookBlastInc.Application.Services;
using BookBlastInc.Core.Entities;
using BookBlastInc.Core.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookBlastInc.Web.Pages.LoanPages;

public class EditStatus : PageModel
{
    private readonly BookService _service;
    
    [BindProperty]
    public BookOnLoan BookOnLoan { get; set; }

    public EditStatus(BookService service)
    {
        _service = service;
    }
    
    public void OnGet(Guid id)
    {
        BookOnLoan = _service.GetBookOnLoanById(id);
    }

    public IActionResult OnPost(Guid id)
    {
        var bookLoan = _service.GetBookOnLoanById(id);
        bookLoan.LoanStatus = BookOnLoan.LoanStatus;
      
        _service.UpdateLoanStatus(bookLoan);

        return RedirectToPage(nameof(EditStatus), new { id = bookLoan.Id });
    }
}