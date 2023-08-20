using BookBlastInc.Application.Services;
using BookBlastInc.Core.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stripe.Checkout;
using Stripe.FinancialConnections;
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
                _service.UpdateStripePayment(id: id, sessionid: stripeSession.Id, paymentid: stripeSession.PaymentIntentId);
                _service.UpdateOrderStatus(id, PaymentStatus.COMPLETED, OrderStatus.COMPLETED);
            }
        }
        
        OrderNumber = id;
    }
}