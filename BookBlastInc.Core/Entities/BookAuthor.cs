using System.ComponentModel.DataAnnotations.Schema;
using BookBlastInc.Core.Entities.Base;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace BookBlastInc.Core.Entities;

public class BookAuthor : BaseEntity
{
    public Guid BookId { get; set; }
    [ForeignKey("BookId")]
    [ValidateNever]
    public Book? Book { get; set; }
    public Guid? Authorid { get; set; }
    [ForeignKey("Authorid")]
    [ValidateNever]
    public Author? Author { get; set; }
}