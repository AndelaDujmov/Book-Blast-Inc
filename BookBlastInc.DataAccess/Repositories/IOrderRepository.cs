using BookBlastInc.Core.Enums;
using Mysqlx.Crud;
using Order = BookBlastInc.Core.Entities.Order;
using OrderStatus = BookBlastInc.DataAccess.Migrations.OrderStatus;

namespace BookBlastInc.DataAccess.Repositories;

public interface IOrderRepository : IRepository<Order>
{

}