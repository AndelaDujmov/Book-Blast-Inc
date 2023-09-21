using BookBlastInc.Application.Services;
using BookBlastInc.Core.Entities;
using BookBlastInc.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stripe;

namespace BookBlastInc.Web.Pages.ShoppingCartPages;

[Authorize]
public class OrderDetails : PageModel
{
    private readonly BookService _service;
    [BindProperty]
    public Order OrderDetail { get; set; }
    [BindProperty]
    public IEnumerable<OrderBook> OrderBook { get; set; }

    public OrderDetails(BookService service)
    {
        _service = service;
    }
    
  
    public void OnGet(Guid id)
    {
        OrderDetail = _service.GetOrderById(id);
        OrderBook = _service.GetAllBooksByOrder(id);
    }

    public IActionResult OnPost()
    {
        _service.UpdateOrder(OrderDetail);
        
        return RedirectToPage(nameof(OrderDetails), new { id = OrderDetail.Id });
    }

    public IActionResult OnGetShip(Guid id)
    {
        var order = _service.GetOrderById(id);
        order.ShippedDate = DateTime.Now;
        _service.UpdateOrderStatus(order, PaymentStatus.COMPLETED, OrderStatus.SHIPPED);
       
        return RedirectToPage(nameof(OrderDetails), new { id = order.Id });
    }
    
    public IActionResult OnGetCancel(Guid id)
    {
        var order = _service.GetOrderById(id);

        if (order.PaymentStatus.Equals(PaymentStatus.COMPLETED))
        {
            var options = new RefundCreateOptions
            {
                Reason = RefundReasons.RequestedByCustomer,
                PaymentIntent = order.PaymentId
            };

            var refundService = new RefundService();
            var refund = refundService.Create(options);
            
            _service.UpdateOrderStatus(order, PaymentStatus.REFUND, OrderStatus.CANCELLED);
        }
        
        _service.UpdateOrderStatus(order, PaymentStatus.DELAYED, OrderStatus.CANCELLED);
       
        return RedirectToPage(nameof(OrderDetails), new { id = order.Id });
    }
}