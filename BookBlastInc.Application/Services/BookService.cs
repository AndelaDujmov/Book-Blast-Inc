using BookBlastInc.Application.Utils;
using BookBlastInc.Core.Entities;
using BookBlastInc.Core.Enums;
using BookBlastInc.DataAccess.Repositories;
using BookBlastInc.DataAccess.Repositories.Impl;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using MySqlX.XDevAPI;
using Stripe;

namespace BookBlastInc.Application.Services;

public class BookService
{
    private readonly IUnitOfWork _repository;
    private readonly UserManager<IdentityUser> _userManager;

    public BookService(IUnitOfWork repository, UserManager<IdentityUser> userManager)
    {
        _repository = repository;
        _userManager = userManager;
    }

    public void Add(Author author)
    {
        _repository.AuthorRepository.Add(author);
        _repository.AuthorRepository.Save();
    }

    public void Add(Book book)
    {
        _repository.BookRepository.Add(book);
        _repository.BookRepository.Save();
    }


    public IEnumerable<SelectListItem> GetSelectList()
    {
        var items = _repository.AuthorRepository.GetAll();

        return items.Select(x =>
            new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.FirstName + " " + x.LastName
            }
        );
    }

    public IEnumerable<SelectListItem> GetCategories()
    {
        var items = _repository.CategoryRepository.GetAll();

        return items.Select(x =>
            new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }
        );
    }

    public void AddBookToAuthor(BookAuthor bookAuthor, Book book)
    {

        bookAuthor.BookId = book.Id;
        _repository.BookAuthorRepository.Add(bookAuthor);
        _repository.BookAuthorRepository.Save();
    }


    public Author GetById(Guid? id)
    {
        return _repository.AuthorRepository.Get(x => x.Id.Equals(id));
    }

    public Book GetBook(Guid? id)
    {
        var book =  _repository.BookRepository.Get(x => x.Id.Equals(id));

        book.AuthorNames = GetAuthors(book.Id);
        book.CategoryName = SetCategoryName(book);

        return book;
    }

    public void AddBook(Book book)
    {
        _repository.BookRepository.Add(book);
        _repository.BookRepository.Save();
        
    }

    public IEnumerable<Book> GetBooks()
    {
        var books =  _repository.BookRepository.GetAll();

        books.ToList().ForEach(book =>
        {
            book.CategoryName = SetCategoryName(book);
            book.AuthorNames = GetAuthors(book.Id);
        }); 

        return books;
    }

    public bool IfBookExists(Book book)
    {
        return GetBooks().Contains(book) ? true : false;
    }

    public void UpdateBook(Book book)
    {
        _repository.BookRepository.Update(book);
        _repository.BookRepository.Save();
    }
    
    public void UpdateBookAuthor(BookAuthor bookauthor, Book bookAuthorBook)
    {
        bookauthor.BookId = bookAuthorBook.Id;
        _repository.BookAuthorRepository.Update(bookauthor);
        _repository.BookRepository.Save();
    }

    public IEnumerable<Book> GetAllBooksByCategory(Guid id)
    {
        var books = GetBooks();
        
        if(books!= null)
           books = books.Where(x => x.CategoryId.Equals(id));

        return books;
    }
    
    private string SetCategoryName(Book book)
    {
       return _repository.CategoryRepository.Get(x => x.Id.Equals(book.CategoryId)).Name;
    }

    private List<string>? GetAuthors(Guid bookId)
    {
        var bookAuthor = _repository.BookAuthorRepository.GetAll().Where(x => x.BookId.Equals(bookId));

        var authorsList = new List<string>();
        
        bookAuthor.ToList().ForEach(ba => authorsList.Add(GetById(ba.Authorid).FirstName + " " + GetById(ba.Authorid).LastName));

        return authorsList;
    }

    public void UpdateCart(ShoppingCart cartByDb)
    {
        _repository.ShoppingCartRepository.Update(cartByDb);
        _repository.ShoppingCartRepository.Save();
    }

    public void AddtoCart(ShoppingCart cardByDb)
    {
        _repository.ShoppingCartRepository.Add(cardByDb);
        _repository.ShoppingCartRepository.Save();
    }

    public ShoppingCart GetShoppingCartIfExists(string userId, Guid bookId)
    {
        return _repository.ShoppingCartRepository.Get(cart => cart.UserId.Equals(userId) && cart.BookId.Equals(bookId));
    }

    public int GetNumberOfCardsByUser(string userId)
    {
        return _repository.ShoppingCartRepository.Get(u => u.UserId.Equals(userId)).Count;
    }

    public IEnumerable<ShoppingCart> GetAllPurchasesByUser(string userId)
    {
        var booksCart = _repository.ShoppingCartRepository.GetAll()
            .Where(x => x.UserId.Equals(userId));
        
        booksCart.ToList().ForEach(bc => bc.Book = _repository.BookRepository.Get(x => x.Id.Equals(bc.BookId)));

        return booksCart;
    }

    public decimal GetPriceByQuantity(ShoppingCart shoppingCart)
    {
        var bookPrice = _repository.BookRepository.Get(x => x.Id.Equals(shoppingCart.BookId)).Price;

        return bookPrice * shoppingCart.Count;
    }

    public ApplicationUser GetUserId(string id)
    {
        return _repository.UserRepository.Get(x => x.Id.Equals(id));
    }
    
    public void AddNewOrder(Order order)
    {
        _repository.OrderRepository.Add(order);
        _repository.OrderRepository.Save();
    }
    
    public IEnumerable<Order> GetAllOrders()
    {
        var orders = _repository.OrderRepository.GetAll();
        
        orders.ToList().ForEach(x => x.User = GetUserId(x.UserId));

        return orders;
    }

    public Book GetBookById(Guid id)
    {
        return _repository.BookRepository.Get(x => x.Id.Equals(id));

    }
    
    public IEnumerable<OrderBook> GetAllBooksByOrder(Guid id)
    {
        var booksPerOrder = _repository.BookOrderRepository.GetAll().Where(x => x.OrderId.Equals(id));
        
        booksPerOrder.ToList().ForEach(b => b.Book = GetBookById(b.BookId));

        return booksPerOrder;
    }
    
    public void CreateOrderForBook(ShoppingCart el, Guid orderId)
    {
        var orderForBook = new OrderBook();
        orderForBook.BookId = el.BookId;
        orderForBook.OrderId = orderId;
        orderForBook.Count = el.Count;
        orderForBook.Price = el.Count * el.Book.Price;
        _repository.BookOrderRepository.Add(orderForBook);
        _repository.BookOrderRepository.Save();
    }

    public void UpdateOrderStatus(Order order, PaymentStatus paymentStatus, OrderStatus orderStatus)
    {
        
        if (order is not null)
        {
            order.OrderStatus = orderStatus;

            if (paymentStatus != null)
            {
                order.PaymentStatus = paymentStatus;
            }
        }
        _repository.OrderRepository.Update(order);
        _repository.OrderRepository.Save();
    }
    public void UpdateStripePayment(Order order, string sessionid, string paymentid)
    {
        if (!string.IsNullOrEmpty(sessionid))
        {
            order.SessionId = sessionid; 
        }
        if (!string.IsNullOrEmpty(sessionid))
        {
            order.PaymentId = paymentid;
        } 
        
        _repository.OrderRepository.Update(order);
        _repository.SaveAll();
    }
    
    public Order GetOrderById(Guid id)
    {
        return _repository.OrderRepository.Get(x => x.Id.Equals(id));
    }
    
    public void ByOperationChangeCountOrRmv(Guid cartId, string operation)
    {
        var cart = _repository.ShoppingCartRepository.Get(x => x.Id.Equals(cartId));

        switch (operation)
        {
            case "+":
                Add(cart);
                break;
            case "-":
                Reduce(cart);
                break;
            case "x":
                Remove(cartId);
                break;
            default:
                break;
        }
    }

    private void Add(ShoppingCart cart)
    {
        cart.Count+=1;
        
        _repository.ShoppingCartRepository.Update(cart);
        _repository.ShoppingCartRepository.Save();
    }

    private void Reduce(ShoppingCart cart)
    {
        if (cart.Count <= 1)
        {
            _repository.ShoppingCartRepository.Delete(cart);
            _repository.ShoppingCartRepository.Save();
        }
        else
        {
            cart.Count-=1;
        
            _repository.ShoppingCartRepository.Update(cart);
            _repository.ShoppingCartRepository.Save();
        }
    }

    private void Remove(Guid cartId)
    {
        var cart = _repository.ShoppingCartRepository.Get(x => x.Id.Equals(cartId));


        _repository.ShoppingCartRepository.Delete(cart);
        _repository.ShoppingCartRepository.Save();
    }

    public BookOnLoan LoanABook(Guid bookId, string userId)
    {
        var bookLoan = new BookOnLoan();
        bookLoan.BookId = bookId;
        bookLoan.UserId = userId; ;
        bookLoan.DateOfBorrowing = DateTime.Now;
        bookLoan.LoanStatus = LoanStatus.OnLoan;
        bookLoan.ReturnDate = bookLoan.DateOfBorrowing.AddMonths(1);
        
        _repository.BookLoanReopsitory.Add(bookLoan);
        _repository.BookLoanReopsitory.Save();

        return bookLoan;
    }

    public IEnumerable<BookOnLoan>? GetLoans(string? userId)
    {
        var allLoans = _repository.BookLoanReopsitory.GetAll().Where(x => x.UserId.Equals(userId));

        if (allLoans != null)
        {

            allLoans.ToList().ForEach(a =>
            {
                a.User = GetUserId(a.UserId);
                a.Book = GetBookById((Guid)a.BookId);
            });
        }

        return allLoans;
    }
    
    public void UpdateOrder(Order orderDetail)
    {
        var order = GetOrderById(orderDetail.Id);
        order.Name = orderDetail.Name;
        order.PhoneNumber = orderDetail.PhoneNumber;
        order.StreetAddress = orderDetail.StreetAddress;
        order.City = orderDetail.City;
        order.Country = orderDetail.Country;
        order.PostalCode = orderDetail.PostalCode;
        
        _repository.OrderRepository.Update(order);
        _repository.SaveAll();
    }

    public IEnumerable<Order> GetOrdersByUser(string id)
    {
        var ordersByUser = _repository.OrderRepository.GetAll()
                                                      .Where(x => x.UserId.Equals(id));

        if (ordersByUser != null || !ordersByUser.Any())
        {
            ordersByUser.ToList().ForEach(o => o.User = GetUserId(o.UserId));
        }

        return ordersByUser;
    }

    public IEnumerable<BookOnLoan> GetAllLoans()
    {
        var loans = _repository.BookLoanReopsitory.GetAll();
        
        loans.ToList().ForEach(l =>
        {
            l.Book = GetBookById((Guid)l.BookId);
            l.User = GetUserId(l.UserId);
        });

        return loans;
    }

    public BookOnLoan GetBookOnLoanById(Guid id)
    {
        var bookLoan = _repository.BookLoanReopsitory.Get(x => x.Id.Equals(id));

        bookLoan.User = GetUserId(bookLoan.UserId);
        bookLoan.Book = GetBookById((Guid)bookLoan.BookId);

        return bookLoan;
    }

    public void UpdateLoanStatus(BookOnLoan bookOnLoan)
    {
        bookOnLoan.Book = GetBookById((Guid)bookOnLoan.BookId);
        
        if (bookOnLoan.LoanStatus.Equals(LoanStatus.Returned))
        {
            var options = new RefundCreateOptions
            {
                Reason = RefundReasons.RequestedByCustomer,
                PaymentIntent = bookOnLoan.PaymentId,
                Amount = (bookOnLoan.ReturnDate < DateTime.Now) ? (long?)(bookOnLoan.Book.Deposit / 2) : 
                    (long?)bookOnLoan.Book.Deposit
            };

            var refundService = new RefundService();
            var refund = refundService.Create(options);
        }
        
        
        _repository.BookLoanReopsitory.Update(bookOnLoan);
        _repository.SaveAll();
    }

    public void UpdateStripeDeposit(BookOnLoan book, string paymentId, string sessionId)
    {
        if (!string.IsNullOrEmpty(sessionId))
        {
            book.SessionId = sessionId;
            book.PaymentId = paymentId;
        }
        
        _repository.BookLoanReopsitory.Update(book);
        _repository.SaveAll();
    }

    public IEnumerable<ApplicationUser>? GetAllUsers(string id)
    {
        var users = _repository.UserRepository.GetAll();

        if (users is not null)
        {
            users = users.Where(x => !x.Id.Equals(id));
            users.ToList().ForEach(u => u.Role = CheckIfUserInRole(u.Id));
        }

        return users;
    }

    public IEnumerable<ApplicationUser>? GetAllClients()
    {
        var users = _repository.UserRepository.GetAll();
     

        if (users is not null)
        {
            users.ToList().ForEach(u => u.Role = CheckIfUserInRole(u.Id));
            users = users.Where(x => x.Role.Equals(RoleName.User.ToString()));
        }

        return users;
    }
    
    private string CheckIfUserInRole(string userId)
    {
        var role = _repository.UserRepository.GetRoleByUser(userId);

        return role;
    }
}