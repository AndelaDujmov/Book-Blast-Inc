using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookBlastInc.Web.Pages.BookPages;

public class TestAddBook : PageModel
{
    public List<SelectListItem> AvailableOptions { get; set; }
    [BindProperty]
    public List<string> SelectedOptions { get; set; }

    public void OnGet()
    {
        // Populate the available options (you can retrieve these from a database or other data source)
        AvailableOptions = new List<SelectListItem>
        {
            new SelectListItem { Value = "1", Text = "Option 1" },
            new SelectListItem { Value = "2", Text = "Option 2" },
            new SelectListItem { Value = "3", Text = "Option 3" },
            // Add more options here as needed
        };
    }

    public IActionResult OnPost()
    {
        // Process the selected options from the SelectedOptions list
        foreach (var selectedOption in SelectedOptions)
        {
            // Do something with the selected option(s)
        }

        // You can return a view or redirect to another page as needed
        return Page();
    }
}