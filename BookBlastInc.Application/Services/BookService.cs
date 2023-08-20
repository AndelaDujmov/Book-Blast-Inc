using BookBlastInc.Core.Entities;
using BookBlastInc.Core.Enums;
using BookBlastInc.DataAccess.Repositories;
using BookBlastInc.DataAccess.Repositories.Impl;
using Microsoft.AspNetCore.Mvc.Rendering;
using MySqlX.XDevAPI;

namespace BookBlastInc.Application.Services;

public class BookService
{
    private readonly IUnitOfWork _repository;

    public BookService(IUnitOfWork repository)
    {
        _repository = repository;
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

    public void AddToCart(string userId, int count, Guid bookId)
    {
        var shoppingCard = new ShoppingCart();
        shoppingCard.BookId = bookId;
        shoppingCard.UserId = userId;
        shoppingCard.Count = count;

        var cartByDb = _repository.ShoppingCartRepository.Get(cart => cart.UserId.Equals(userId) && cart.BookId.Equals(bookId));

        if (cartByDb is not null)
        {
            cartByDb.Count += count;
            _repository.ShoppingCartRepository.Update(cartByDb);
        }
        else
        {
            _repository.ShoppingCartRepository.Add(shoppingCard);
        }
        
        _repository.ShoppingCartRepository.Save();
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

    public void UpdateOrderStatus(Guid id, PaymentStatus paymentStatus, OrderStatus orderStatus)
    {
        _repository.OrderRepository.UpdateStatus(id, orderStatus, paymentStatus);
        _repository.OrderRepository.Save();
    }
    public void UpdateStripePayment(Guid id, string sessionid, string paymentid)
    {
        _repository.OrderRepository.UpdatePayment(id, paymentid, sessionid);
     
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
}