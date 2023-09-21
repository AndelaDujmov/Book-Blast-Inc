using System.Security.Claims;
using BookBlastInc.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stripe;
using Stripe.Checkout;

namespace BookBlastInc.Web.Pages.LoanPages;

public class GetTheLoan : PageModel
{
    private readonly BookService _bookService;

    public GetTheLoan(BookService bookService)
    {
        _bookService = bookService;
    }
    
    public IActionResult OnGet(Guid BookId)
    {
        var book = _bookService.GetBook(BookId);
        
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        
        var domain = "https://localhost:7089";
        
        StripeConfiguration.ApiKey = "sk_test_51NhBpqIkrMP3p30PcNbRMtxmKiIMBpu7mIUJl6yBmYQ27OD0SjLJx0WnkWT6KZOGr5Xf6fK13xwDB3Pit2SUhFV700ccLcoOjF";

        var bookOnLoan = _bookService.LoanABook(bookId: BookId, userId: claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value);
        
        var options = new SessionCreateOptions
        {
            SuccessUrl = domain+$"/BookPages/SuccessfulLoan?id={bookOnLoan.Id}",
            CancelUrl = domain+$"/",
            LineItems = new List<SessionLineItemOptions>(),
            Mode = "payment",
        };
        
        var sessionLineItem = new SessionLineItemOptions
        {
            PriceData = new SessionLineItemPriceDataOptions
            {
                UnitAmount = (long)(book.Deposit*100),
                Currency = "eur",
                ProductData = new SessionLineItemPriceDataProductDataOptions
                {
                    Name = book.Name
                }
            },
            Quantity = 1
        };
        options.LineItems.Add(sessionLineItem);
        
        var service = new SessionService();
        Session stripeSession = service.Create(options);
        
        _bookService.UpdateStripeDeposit(book: bookOnLoan, paymentId: stripeSession.PaymentIntentId, sessionId: stripeSession.Id);
        
        Response.Headers.Add("Location", stripeSession.Url);

        return new StatusCodeResult(303);
        
    } 
}