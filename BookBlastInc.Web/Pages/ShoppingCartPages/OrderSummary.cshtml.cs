using System.Security.Claims;
using BookBlastInc.Application.Services;
using BookBlastInc.Core.Entities;
using BookBlastInc.Core.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stripe;
using Stripe.Checkout;
using ShoppingCart = BookBlastInc.DataAccess.Migrations.ShoppingCart;

namespace BookBlastInc.Web.Pages.ShoppingCartPages;

public class OrderSummary : PageModel
{
    private readonly BookService _service;

    public OrderSummary(BookService service)
    {
        _service = service;
    }
    
    [BindProperty] public IEnumerable<Core.Entities.ShoppingCart> ShoppingCarts { get; set; } = new List<Core.Entities.ShoppingCart>();
    [BindProperty] public Order Order { get; set; } = new Order();
    
    public void OnGet()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        
        Order.UserId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        ShoppingCarts =
            _service.GetAllPurchasesByUser(userId: Order.UserId);

        Order.User = _service.GetUserId(id: Order.UserId);
        
        Order.Name = Order.User.FirstName + " " + Order.User.LastName;
        Order.PhoneNumber = Order.User.PhoneNumber ?? string.Empty;
        
        ShoppingCarts.ToList().ForEach(x => Order.Total += _service.GetPriceByQuantity(x));

    }

    public IActionResult OnPost()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;

        Order.UserId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        ShoppingCarts =
            _service.GetAllPurchasesByUser(userId: Order.UserId);

        ShoppingCarts.ToList().ForEach(x => Order.Total += _service.GetPriceByQuantity(x)); 
        
        Order.OrderDate = DateTime.Now;
        Order.PaymentStatus = PaymentStatus.PENDING;
        Order.OrderStatus = OrderStatus.PENDING;
        _service.AddNewOrder(Order);

        ShoppingCarts.ToList().ForEach(el => _service.CreateOrderForBook(el, Order.Id));

        var domain = "https://localhost:7089";
        
        StripeConfiguration.ApiKey = "sk_test_51NhBpqIkrMP3p30PcNbRMtxmKiIMBpu7mIUJl6yBmYQ27OD0SjLJx0WnkWT6KZOGr5Xf6fK13xwDB3Pit2SUhFV700ccLcoOjF";

        var options = new SessionCreateOptions
        {
            SuccessUrl = domain+$"/ShoppingCartPages/OrderConfirmation?id={Order.Id}",
            CancelUrl = domain+$"/",
            LineItems = new List<SessionLineItemOptions>(),
            Mode = "payment",
        };
        
        ShoppingCarts.ToList().ForEach(cart =>
        {
            var sessionLineItem = new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = (long)(cart.Count * cart.Book.Price * 100),
                    Currency = "eur",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = cart.Book.Name
                    }
                },
                Quantity = cart.Count
            };
            options.LineItems.Add(sessionLineItem);
        }
        );
        
        var service = new SessionService();
        Session stripeSession = service.Create(options);

        _service.UpdateStripePayment( Order, sessionid: stripeSession.Id, paymentid: stripeSession.PaymentIntentId);
        Response.Headers.Add("Location", stripeSession.Url);

        return new StatusCodeResult(303);
    }
    
}