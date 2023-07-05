namespace BookBlastInc.DataAccess.Repositories.Impl;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _dbContext;
    public ICategoryRepository CategoryRepository { get; private set; }
    public IAuthorRepository AuthorRepository { get; private set; }
    public IBookRepository BookRepository { get; private set; }

    public UnitOfWork(AppDbContext appDbContext)
    {
        _dbContext = appDbContext;
        CategoryRepository = new CategoryRepository(_dbContext);
        AuthorRepository = new AuthorRepository(_dbContext);
        BookRepository = new BookRepository(_dbContext);
    }
}