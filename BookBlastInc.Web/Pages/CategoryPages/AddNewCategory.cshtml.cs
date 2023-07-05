using BookBlastInc.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookBlastInc.Web.Pages.CategoryPages;

[BindProperties]
public class AddNewCategory : PageModel
{
    private readonly CategoryService _service;
    public Core.Entities.Category Category { get; set; }

    public AddNewCategory(CategoryService service)
    {
        _service = service;
    }
    
    public void OnGet()
    {
       
    }

    public IActionResult OnPost(Core.Entities.Category category)
    {
        _service.Add(category);
        return RedirectToPage("Categories");
    }
}