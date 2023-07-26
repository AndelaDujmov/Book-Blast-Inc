using BookBlastInc.Core.Entities;
using BookBlastInc.DataAccess.Repositories;
using BookBlastInc.DataAccess.Repositories.Impl;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookBlastInc.Application.Services;

public class BookService
{
    private readonly IUnitOfWork _repository;

    public BookService(IUnitOfWork repository)
    {
        _repository = repository;
    }

    public void Add(Author author)
    {
        _repository.AuthorRepository.Add(author);
        _repository.AuthorRepository.Save();
    }

    public void Add(Book book)
    {
        _repository.BookRepository.Add(book);
        _repository.BookRepository.Save();
    }


    public IEnumerable<SelectListItem> GetSelectList()
    {
        var items = _repository.AuthorRepository.GetAll();

        return items.Select(x =>
            new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.FirstName + " " + x.LastName
            }
        );
    }

    public IEnumerable<SelectListItem> GetCategories()
    {
        var items = _repository.CategoryRepository.GetAll();

        return items.Select(x =>
            new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }
        );
    }

    public void AddBookToAuthor(BookAuthor bookAuthor, Book book)
    {

        bookAuthor.BookId = book.Id;
        _repository.BookAuthorRepository.Add(bookAuthor);
        _repository.BookAuthorRepository.Save();
    }


    public Author GetById(Guid id)
    {
        return _repository.AuthorRepository.Get(x => x.Id.Equals(id));
    }

    public void AddCategoryToBook(Book book, Guid category)
    {
        if (!book.CategoryId.Equals(null))
        {
            book.CategoryId = category;
            _repository.BookRepository.Add(book);
            _repository.BookRepository.Save();
        }
    }

}