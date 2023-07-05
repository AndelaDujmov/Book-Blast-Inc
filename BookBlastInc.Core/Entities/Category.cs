using BookBlastInc.Core.Entities.Base;

namespace BookBlastInc.Core.Entities;

public class Category : BaseEntity
{
    public string Name { get; set; }
    public int DisplayNumber { get; set; }
}