using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using BookBlastInc.Core.Entities.Base;


namespace BookBlastInc.Core.Entities;

public class Book : BaseEntity
{
    [DisplayName("Book Name")]
    public string Name { get; set; }
    public string About { get; set; }
    public Guid CategoryId { get; set; }
    [ForeignKey("CategoryId")]
    public Category? Category { get; set; }
    [DisplayName("Release Date")]
    public DateOnly ReleaseDate { get; set; }
    public decimal Price { get; set; }
}