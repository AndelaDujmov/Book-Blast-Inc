using BookBlastInc.Core.Enums;
using Mysqlx.Crud;
using Order = BookBlastInc.Core.Entities.Order;
using OrderStatus = BookBlastInc.DataAccess.Migrations.OrderStatus;

namespace BookBlastInc.DataAccess.Repositories;

public interface IOrderRepository : IRepository<Order>
{
    void UpdateStatus(Guid id, Core.Enums.OrderStatus orderStatus, PaymentStatus? paymentStat = null);
    void UpdatePayment(Guid id, string paymentId, string sessionId);
}