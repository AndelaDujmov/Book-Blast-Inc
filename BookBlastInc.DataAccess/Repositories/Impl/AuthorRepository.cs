using BookBlastInc.Core.Entities;

namespace BookBlastInc.DataAccess.Repositories.Impl;

public class AuthorRepository : Repository<Author>, IAuthorRepository
{
    public AuthorRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public Author GetById(Guid id)
    {
        return base.Get(x => x.Id.Equals(id)) ?? null;
    }
}