using BookBlastInc.Core.Entities;

namespace BookBlastInc.DataAccess.Repositories.Impl;

public class BookAuthorRepository : Repository<BookAuthor>, IBookAuthorRepository
{
    public BookAuthorRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}