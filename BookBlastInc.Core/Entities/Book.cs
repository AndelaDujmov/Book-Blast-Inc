using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using BookBlastInc.Core.Entities.Base;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;


namespace BookBlastInc.Core.Entities;

public class Book : BaseEntity
{
    [DisplayName("Book Name")]
    public string Name { get; set; }
    public string About { get; set; }
    public Guid? CategoryId { get; set; }
    [ForeignKey("CategoryId")]
    [ValidateNever]
    public Category? Category { get; set; }
    [DisplayName("Release Date")]
    public DateTime? ReleaseDate { get; set; }
    public decimal Price { get; set; }
    [DisplayName("Book Image")]
    public string? PhotoUrl { get; set; }
    [NotMapped]
    public string CategoryName { get; set; }
    [NotMapped]
    public List<string>? AuthorNames { get; set; }
    public decimal Deposit { get; set; }
    
}