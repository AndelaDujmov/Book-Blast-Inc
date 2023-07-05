using BookBlastInc.Core.Entities;

namespace BookBlastInc.DataAccess.Repositories.Impl;

public class BookRepository : Repository<Book>, IBookRepository
{
    public BookRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}