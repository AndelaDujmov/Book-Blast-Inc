namespace BookBlastInc.DataAccess.Repositories;

public interface IUnitOfWork
{
    ICategoryRepository CategoryRepository { get; }
    IAuthorRepository AuthorRepository { get; }
    IBookRepository BookRepository { get;}
    IBookAuthorRepository BookAuthorRepository { get; }
    IShoppingCartRepository ShoppingCartRepository { get; }
    IUserRepository UserRepository { get; }
}