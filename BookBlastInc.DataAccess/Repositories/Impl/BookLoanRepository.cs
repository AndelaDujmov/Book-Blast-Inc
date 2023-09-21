using BookBlastInc.Core.Entities;

namespace BookBlastInc.DataAccess.Repositories.Impl;

public class BookLoanRepository : Repository<BookOnLoan>, IBookLoanReopsitory
{
    public BookLoanRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}