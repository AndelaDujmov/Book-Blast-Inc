using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookBlastInc.Core.Enums;
using Microsoft.AspNetCore.Identity;

namespace BookBlastInc.Core.Entities;

public class ApplicationUser: IdentityUser
{
    [Required]
    public int Name { get; set; }
    
    [Required(ErrorMessage = "First name field is required!")]
    [StringLength(maximumLength: 30, MinimumLength = 3)]
    public string FirstName { get; set; }
    [Required(ErrorMessage = "Last name field is required")]
    [StringLength(maximumLength: 30, MinimumLength = 2)]
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public bool Deleted { get; set; } = false;
    [NotMapped]
    public string Role { get; set; }
}