using System.Security.Claims;
using BookBlastInc.Application.Services;
using BookBlastInc.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookBlastInc.Web.Pages.ShoppingCartPages;

public class Index : PageModel
{
    private readonly BookService _service;

    public Index(BookService service)
    {
        _service = service;
    }

    [BindProperty] public IEnumerable<ShoppingCart> ShoppingCarts { get; set; } = new List<ShoppingCart>();
    [BindProperty] public Order Order { get; set; } = new Order();
    
    public void OnGet()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;

        ShoppingCarts =
            _service.GetAllPurchasesByUser(userId: claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value);
        
        
        ShoppingCarts.ToList().ForEach(x => Order.Total += _service.GetPriceByQuantity(x));
        
    }

    public IActionResult OnGetPlus(Guid cartId)
    {
        _service.ByOperationChangeCountOrRmv(cartId, "+");
        return RedirectToPage("Index");
    }
    
    public IActionResult OnGetMinus(Guid cartId)
    {
        _service.ByOperationChangeCountOrRmv(cartId, "-");
        return RedirectToPage("Index");
    }
    
    public IActionResult OnGetRemove(Guid cartId)
    {
        _service.ByOperationChangeCountOrRmv(cartId, "x");
        return RedirectToPage("Index");
    }
    
}