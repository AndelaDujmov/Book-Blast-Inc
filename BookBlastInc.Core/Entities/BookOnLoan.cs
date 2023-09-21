using System.ComponentModel.DataAnnotations.Schema;
using BookBlastInc.Core.Entities.Base;
using BookBlastInc.Core.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace BookBlastInc.Core.Entities;

public class BookOnLoan : BaseEntity
{
    public Guid? BookId { get; set; }
    [ForeignKey("BookId")]
    [ValidateNever]
    public Book? Book { get; set; }
    public string UserId { get; set; }
    [ForeignKey("UserId")]
    [ValidateNever]
    public ApplicationUser User { get; set; }
    public DateTime DateOfBorrowing { get; set; }
    public DateTime? ReturnDate { get; set; }
    public LoanStatus LoanStatus { get; set; }
    public string? PaymentId { get; set; }
    public string? SessionId { get; set; }
}