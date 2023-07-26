using BookBlastInc.Application.Services;
using BookBlastInc.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookBlastInc.Web.Pages.CategoryPages;

public class EditCategory : PageModel
{
    private readonly CategoryService _service;
    public Category Category { get; set; } = new Category();
    
    public EditCategory(CategoryService service)
    {
        _service = service;
    }
    public void OnGet(Guid id)
    {
        Category = _service.GetById(id);
    }

    public IActionResult OnPost()
    {
        if (ModelState.IsValid)
        {
            _service.Update(Category);
            return RedirectToPage("Categories");
        }

        return Page();
    }
}