using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace BookBlastInc.DataAccess.Repositories.Impl;

public class Repository<T>  : IRepository<T> where T:class
{
    private readonly AppDbContext _dbContext;
    internal DbSet<T> _dbSet;
    
    public Repository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<T>();
    }

    public IEnumerable<T> GetAll()
    {
        IQueryable<T> query = _dbSet;

        return query.ToList();
    }

    public void Add(T obj)
    {
        _dbSet.Add(obj);
    }

    public T Get(Expression<Func<T, bool>> filter)
    {
        IQueryable<T> query = _dbSet;

        return query.Where(filter)
                    .FirstOrDefault();
    }

    public void Save()
    {
        _dbContext.SaveChanges();
    }

    public void Delete(T obj)
    {
        _dbSet.Remove(obj);
    }
}