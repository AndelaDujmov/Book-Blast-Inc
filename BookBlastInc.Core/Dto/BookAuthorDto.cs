using System.ComponentModel.DataAnnotations;
using BookBlastInc.Core.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookBlastInc.Core.Dto;

public class BookAuthorDto
{
    public Book Book { get; set; }
    [ValidateNever]
    public IEnumerable<SelectListItem>? SelectListItems { get; set; }

    [Display(Name = "Authors")] public Guid[] AuthorIds { get; set; } = new Guid[]{};

    [ValidateNever]
    public IEnumerable<SelectListItem>? Categories { get; set; }

}