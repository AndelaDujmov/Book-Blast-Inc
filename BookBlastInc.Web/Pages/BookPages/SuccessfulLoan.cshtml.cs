using BookBlastInc.Application.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stripe.Checkout;

namespace BookBlastInc.Web.Pages.BookPages;

public class SuccessfulLoan : PageModel
{
    private readonly BookService _service;
    
    public SuccessfulLoan(BookService service)
    {
        _service = service;
    }
    public void OnGet(Guid id)
    {
        var bookLoan = _service.GetBookOnLoanById(id);
       
        var service = new SessionService();
        Session stripeSession = service.Get(bookLoan.SessionId);

        if (stripeSession.PaymentStatus.ToLower() == "paid")
        {
            var bookOnLoan = _service.GetBookOnLoanById(id);
            _service.UpdateStripeDeposit(book: bookOnLoan, sessionId: stripeSession.Id, paymentId: stripeSession.PaymentIntentId);
           
        }
    }
}