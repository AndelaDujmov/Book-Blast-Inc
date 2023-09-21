namespace BookBlastInc.DataAccess.Repositories;

public interface IUnitOfWork
{
    ICategoryRepository CategoryRepository { get; }
    IAuthorRepository AuthorRepository { get; }
    IBookRepository BookRepository { get;}
    IBookAuthorRepository BookAuthorRepository { get; }
    IShoppingCartRepository ShoppingCartRepository { get; }
    IUserRepository UserRepository { get; }
    IOrderRepository OrderRepository { get; }
    IBookOrderRepository BookOrderRepository { get; }
    IBookLoanReopsitory BookLoanReopsitory { get; }
    void SaveAll();
}