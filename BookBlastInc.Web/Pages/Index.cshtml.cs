using BookBlastInc.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookBlastInc.Web.Pages;
[BindProperties]
public class IndexModel : PageModel
{
    public IEnumerable<Core.Entities.Category> Categories { get; set; } = new List<Core.Entities.Category>();
    
    private readonly CategoryService _service;

    public IndexModel(CategoryService service)
    {
        _service = service;
    }
    
    public void OnGet()
    {
        Categories = _service.GetAll();
       
    }
}