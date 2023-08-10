using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookBlastInc.Core.Entities.Base;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace BookBlastInc.Core.Entities;

public class ShoppingCart : BaseEntity
{
    public Guid BookId { get; set; }
    [ForeignKey("BookId")]
    [ValidateNever]
    public Book? Book { get; set; }
    [Range(1, 100, ErrorMessage = "Please enter a value between 1 and 100")]
    public int Count { get; set; }
    public string UserId { get; set; }
    [ForeignKey("UserId")]
    [ValidateNever]
    public ApplicationUser? ApplicationUser { get; set; }
}