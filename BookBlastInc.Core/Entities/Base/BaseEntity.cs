using System.ComponentModel.DataAnnotations;

namespace BookBlastInc.Core.Entities.Base;

public abstract class BaseEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
}