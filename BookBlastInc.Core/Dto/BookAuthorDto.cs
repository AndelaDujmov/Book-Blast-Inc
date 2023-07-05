using BookBlastInc.Core.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookBlastInc.Core.Dto;

public class BookAuthorDto
{
    public Book Book { get; set; }
    public Guid Author { get; set; }
    [ValidateNever]
    public IEnumerable<SelectListItem>? SelectListItems { get; set; }
    public IEnumerable<SelectListItem>? Categories { get; set; }

}