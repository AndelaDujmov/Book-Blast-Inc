using BookBlastInc.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookBlastInc.Web.Pages.CategoryPages;

[BindProperties]
public class Categories : PageModel
{
    public IEnumerable<Core.Entities.Category> CategoryList { get; set; } = new List<Core.Entities.Category>();
    
    private readonly CategoryService _service;

    public Categories(CategoryService service)
    {
        _service = service;
    }
    
    public void OnGet()
    {
        CategoryList = _service.GetAll();
       
    }
}
