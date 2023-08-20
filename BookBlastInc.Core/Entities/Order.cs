using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookBlastInc.Core.Entities.Base;
using BookBlastInc.Core.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace BookBlastInc.Core.Entities;

public class Order : BaseEntity
{
    public string UserId { get; set; }
    [ForeignKey("UserId")]
    [ValidateNever]
    public ApplicationUser? User { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime ShippedDate { get; set; }
    public decimal Total { get; set; }
    public PaymentStatus? PaymentStatus { get; set; }
    public OrderStatus OrderStatus { get; set; }
    [Required]
    public string PhoneNumber { get; set; }
    [Required]
    public string StreetAddress { get; set; }
    [Required]
    public string City { get; set; }
    [Required]
    public string PostalCode { get; set; }
    [Required]
    public string Country { get; set; }
    [Required]
    public string Name { get; set; }
    public string? PaymentId { get; set; }
    public string? SessionId { get; set; }
}