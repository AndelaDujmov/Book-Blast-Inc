using BookBlastInc.Application.Services;
using BookBlastInc.Core.Enums;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Session = Stripe.Checkout.Session;
using SessionService = Stripe.Checkout.SessionService;

namespace BookBlastInc.Web.Pages.ShoppingCartPages;

public class OrderConfirmation : PageModel
{
    private readonly BookService _service;
    
    [BindProperty]
    public Guid OrderNumber { get; set; }
    
    public OrderConfirmation(BookService service)
    {
        _service = service;
    }
    
    public void OnGet(Guid id)
    {
        var orderStat = _service.GetOrderById(id);
        
        if (orderStat.PaymentStatus != PaymentStatus.DELAYED)
        {
            var service = new SessionService();
            Session stripeSession = service.Get(orderStat.SessionId);

            if (stripeSession.PaymentStatus.ToLower() == "paid")
            {
                var order = _service.GetOrderById(id);
                _service.UpdateStripePayment(order, sessionid: stripeSession.Id, paymentid: stripeSession.PaymentIntentId);
                _service.UpdateOrderStatus(order, PaymentStatus.COMPLETED, OrderStatus.COMPLETED);
            }
        }
        
        OrderNumber = id;
    }
    
}