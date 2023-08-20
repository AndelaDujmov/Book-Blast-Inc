using System.ComponentModel.DataAnnotations.Schema;
using BookBlastInc.Core.Entities.Base;
using BookBlastInc.Core.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Azure.Cosmos;

namespace BookBlastInc.Core.Entities;

public class OrderBook : BaseEntity
{
    public Guid OrderId { get; set; }
    [ForeignKey("OrderId")]
    [ValidateNever]
    public Order Order { get; set; }
    public Guid BookId { get; set; }
    [ForeignKey("BookId")]
    [ValidateNever]
    public Book Book { get; set; }
    public int Count { get; set; }
    public decimal Price { get; set; }
    
}