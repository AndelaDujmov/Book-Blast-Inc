using System.Security.Claims;
using BookBlastInc.Application.Services;
using BookBlastInc.Application.Utils;
using BookBlastInc.Core.Enums;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Order = BookBlastInc.Core.Entities.Order;

namespace BookBlastInc.Web.Pages.ShoppingCartPages;

public class OrderManagement : PageModel
{

    private readonly BookService _service;
    [BindProperty]
    public IEnumerable<Order> Orders { get; set; }
    public string CurrentFilter { get; set; }
    public string CurrentSort { get; set; }
    public PaginatedList<Order> PaginatedOrders { get; set; }
    
    public OrderManagement(BookService service)
    {
        _service = service;
    }
    
    public void OnGet(string sortOrder, string searchString, int? pageIndex)
    {
        ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
        
        
        CurrentFilter = searchString;

        IEnumerable<Order> allOrders = (User.IsInRole(RoleName.Administrator.ToString()) || 
                                        User.IsInRole(RoleName.Employee.ToString())) ?
            _service.GetAllOrders() : 
            _service.GetOrdersByUser(((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value);


        if (!string.IsNullOrEmpty(searchString))
        {
            allOrders = allOrders.Where(o => o.OrderDate.ToString().Contains(searchString) || o.Name.Contains(searchString));
        }
        
        switch (sortOrder)
        {
            case "name_desc":
                allOrders = allOrders.OrderByDescending(x => x.Name);
                break;
            default:
                allOrders =  allOrders.OrderBy(x => x.Name);
                break;
        }
        
        var pageSize = 4;
        PaginatedOrders = PaginatedList<Order>.CreateList(allOrders, pageIndex ?? 1, pageSize);

        Orders = PaginatedOrders.ToList();
    }

    public void OnGetStatus(string sortOrder, int? pageIndex, string status)
    {
        IEnumerable<Order> allOrders = (User.IsInRole(RoleName.Administrator.ToString()) || 
                                        User.IsInRole(RoleName.Employee.ToString())) ?
                                        _service.GetAllOrders() : 
                                        _service.GetOrdersByUser(((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value);
        
        switch (status)
        {
            case "pending":
                allOrders = allOrders.Where(x => x.OrderStatus.Equals(OrderStatus.PENDING));
                break;
            case "completed":
                allOrders = allOrders.Where(x => x.OrderStatus.Equals(OrderStatus.COMPLETED));
                break;
            case "cancelled":
                allOrders = allOrders.Where(x => x.OrderStatus.Equals(OrderStatus.CANCELLED));
                break;
            default:
               
                break;
        }
        
        switch (sortOrder)
        {
            case "name_desc":
                allOrders = allOrders.OrderByDescending(x => x.Name);
                break;
            default:
                allOrders =  allOrders.OrderBy(x => x.Name);
                break;
        }
        
        var pageSize = 4;
        PaginatedOrders = PaginatedList<Order>.CreateList(allOrders, pageIndex ?? 1, pageSize);

        Orders = PaginatedOrders.ToList();
    }
    
    public IActionResult OnGetInvoiceGeneration(Guid id)
    {
        var orderStat = _service.GetOrderById(id);
        var booksByOrder = _service.GetAllBooksByOrder(id);
        
        using (MemoryStream ms = new MemoryStream())
        {
            var document = new Document(PageSize.A4, 25, 25, 30, 30);
            document.AddTitle("Receipt");
            var writer = PdfWriter.GetInstance(document, ms);
            document.Open();
            
            var paragraph = new Paragraph($"RECEIPT NR. {orderStat.Id}", new Font(Font.FontFamily.HELVETICA, 20));
            paragraph.Alignment = Element.ALIGN_LEFT;
            document.Add(paragraph);
            var paragraph2 = new Paragraph($"{orderStat.OrderDate}", new Font(Font.FontFamily.HELVETICA, 15));
            paragraph2.Alignment = Element.ALIGN_LEFT;
            document.Add(paragraph2);

            var receiptTable = new PdfPTable(4);

            var cell1 = new PdfPCell(new Phrase("Book", new Font(Font.FontFamily.HELVETICA, 10)));
            cell1.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER;
            cell1.HorizontalAlignment = Element.ALIGN_CENTER;
            cell1.VerticalAlignment = Element.ALIGN_CENTER;
            receiptTable.AddCell(cell1);
            var cell2 = new PdfPCell(new Phrase("Unit Price", new Font(Font.FontFamily.HELVETICA, 10)));
            cell2.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER;
            cell2.HorizontalAlignment = Element.ALIGN_CENTER;
            cell2.VerticalAlignment = Element.ALIGN_CENTER;
            receiptTable.AddCell(cell2);
            var cell3 = new PdfPCell(new Phrase("Count", new Font(Font.FontFamily.HELVETICA, 10)));
            cell3.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER;
            cell3.HorizontalAlignment = Element.ALIGN_CENTER;
            cell3.VerticalAlignment = Element.ALIGN_CENTER;
            receiptTable.AddCell(cell3);
            var cell4 = new PdfPCell(new Phrase("Total/book", new Font(Font.FontFamily.HELVETICA, 10)));
            cell4.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER;
            cell4.HorizontalAlignment = Element.ALIGN_CENTER;
            cell4.VerticalAlignment = Element.ALIGN_CENTER;
            receiptTable.AddCell(cell4);
            
            booksByOrder.ToList().ForEach(bo =>
            {
                var cell1 = new PdfPCell(new Phrase(bo.Book.Name));
                var cell2 = new PdfPCell(new Phrase(string.Format("€{0:N2}", bo.Book.Price)));
                var cell3 = new PdfPCell(new Phrase(bo.Count.ToString()));
                var cell4 = new PdfPCell(new Phrase(string.Format("€{0:N2}", bo.Book.Price * bo.Count)));

                receiptTable.AddCell(cell1);
                receiptTable.AddCell(cell2);
                receiptTable.AddCell(cell3);
                receiptTable.AddCell(cell4);
            });
            document.Add(receiptTable);
            document.Close();
            writer.Close();
            var constant = ms.ToArray();
            return File(constant, "application/vnd", "File.pdf");
        }
    }
}