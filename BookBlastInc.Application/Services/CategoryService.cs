using BookBlastInc.Core.Entities;
using BookBlastInc.DataAccess;
using BookBlastInc.DataAccess.Repositories;
using BookBlastInc.DataAccess.Repositories.Impl;

namespace BookBlastInc.Application.Services;

public class CategoryService
{
    private readonly IUnitOfWork _repository;

    public CategoryService(IUnitOfWork repository)
    {
        _repository = repository;
    }
    
    public IEnumerable<Category>? GetAll()
    {
        var categories =  _repository.CategoryRepository.GetAll() ?? null;

        return categories;
    }

    public void Add(Category category)
    {
        _repository.CategoryRepository.Add(category);
        _repository.CategoryRepository.Save();
    }

    public Category Return(Guid? id)
    {
        return _repository.CategoryRepository.Get(x => x.Id.Equals(id)) ?? new Category();
    }

    public bool Remove(Guid id)
    {
        var category = Return(id);

        if (category is null) return false;

        _repository.CategoryRepository.Delete(category);
        _repository.CategoryRepository.Save();
        return true;
    }
}