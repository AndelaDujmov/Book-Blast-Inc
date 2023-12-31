using System.ComponentModel.DataAnnotations.Schema;
using BookBlastInc.Core.Entities.Base;
using BookBlastInc.Core.Enums;

namespace BookBlastInc.Core.Entities;

public class User : BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public Guid RoleId { get; set; }
    [ForeignKey("RoleId")]
    public Role? Role { get; set; }
    public Gender Gender { get; set; }
}