namespace BookBlastInc.DataAccess.Repositories.Impl;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _dbContext;
    public ICategoryRepository CategoryRepository { get; private set; }
    public IAuthorRepository AuthorRepository { get; private set; }
    public IBookRepository BookRepository { get; private set; }
    public IBookAuthorRepository BookAuthorRepository { get; private set; }
    public IShoppingCartRepository ShoppingCartRepository { get; private set; }

    public IUserRepository UserRepository { get; private set; }
    public IOrderRepository OrderRepository { get; }
    public IBookOrderRepository BookOrderRepository { get; }

    public UnitOfWork(AppDbContext appDbContext)
    {
        _dbContext = appDbContext;
        CategoryRepository = new CategoryRepository(_dbContext);
        AuthorRepository = new AuthorRepository(_dbContext);
        BookRepository = new BookRepository(_dbContext);
        BookAuthorRepository = new BookAuthorRepository(_dbContext);
        ShoppingCartRepository = new ShoppingCartRepository(_dbContext);
        UserRepository = new UserRepository(_dbContext);
        OrderRepository = new OrderRepository(_dbContext);
        BookOrderRepository = new BookOrderRepository(_dbContext);
    }
}