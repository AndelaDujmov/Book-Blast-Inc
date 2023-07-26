using System.ComponentModel.DataAnnotations.Schema;
using BookBlastInc.Core.Entities.Base;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace BookBlastInc.Core.Entities;

public class Author : BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}