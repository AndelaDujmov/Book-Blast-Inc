using System.ComponentModel;
using BookBlastInc.Core.Entities.Base;

namespace BookBlastInc.Core.Entities;

public class Genre : BaseEntity
{
    [DisplayName("Genre")]
    public string Name { get; set; }
}