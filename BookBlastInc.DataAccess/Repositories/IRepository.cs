using System.Linq.Expressions;

namespace BookBlastInc.DataAccess.Repositories;

public interface IRepository<T> where T:class
{
    IEnumerable<T> GetAll();
    void Add(T obj);
    T Get(Expression<Func<T, bool>> filter);
    void Save();
    public void Update(T obj);
    void Delete(T obj);
}