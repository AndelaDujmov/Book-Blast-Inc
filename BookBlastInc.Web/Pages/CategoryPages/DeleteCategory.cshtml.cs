using BookBlastInc.Application.Services;
using BookBlastInc.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookBlastInc.Web.Pages.CategoryPages;

[BindProperties]
public class DeleteCategory : PageModel
{
    private readonly CategoryService _service;
    public Category Category { get; set; }

    public DeleteCategory(CategoryService service)
    {
        _service = service;
    }
    
    public void OnGet(Guid? id)
    {
        if (id is not null)
            Category = _service.Return(id);
    }

    public IActionResult OnPost(Guid id)
    {
        return _service.Remove(id) ? RedirectToPage("Categories") : NotFound();
    }
}