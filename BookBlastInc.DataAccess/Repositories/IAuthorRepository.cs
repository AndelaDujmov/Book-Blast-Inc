using BookBlastInc.Core.Entities;

namespace BookBlastInc.DataAccess.Repositories;

public interface IAuthorRepository : IRepository<Author>
{
    public Author GetById(Guid id);
}