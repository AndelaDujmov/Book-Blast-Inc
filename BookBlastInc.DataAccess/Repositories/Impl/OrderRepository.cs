using BookBlastInc.Core.Entities;
using BookBlastInc.Core.Enums;
using OrderStatus = BookBlastInc.DataAccess.Migrations.OrderStatus;

namespace BookBlastInc.DataAccess.Repositories.Impl;

public class OrderRepository : Repository<Order>, IOrderRepository
{
    private readonly AppDbContext _dbContext;
    public OrderRepository(AppDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public void UpdateStatus(Guid id, Core.Enums.OrderStatus orderStatus, PaymentStatus? paymentStatus)
    {
        var order = _dbContext.Orders.FirstOrDefault(o => o.Id.Equals(id));

        if (order is not null)
        {
            order.OrderStatus = orderStatus;

            if (paymentStatus is not null)
            {
                order.PaymentStatus = paymentStatus;
            }
        }
    }

    public void UpdatePayment(Guid id, string paymentId, string sessionId)
    {
        var order = _dbContext.Orders.FirstOrDefault(o => o.Id.Equals(id));

        if (!string.IsNullOrEmpty(sessionId))
        {
            order.SessionId = sessionId; //kada korisnik pokuša napravit uplatu generira se ovaj sessid
            //ako je uspješan generira se payment id
        }
        if (!string.IsNullOrEmpty(sessionId))
        {
            order.PaymentId = paymentId;
        }

    }
}