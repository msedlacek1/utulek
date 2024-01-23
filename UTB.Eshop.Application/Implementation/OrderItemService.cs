using System;
using Microsoft.EntityFrameworkCore;
using UTB.Eshop.Application.Abstraction;
using UTB.Eshop.Domain.Entities;
using UTB.Eshop.Infrastructure.Database;

namespace UTB.Eshop.Application.Implementation
{
    public class OrderItemService : IOrderItemService
    {
        EshopDbContext _eshopDbContext;

        public OrderItemService(EshopDbContext eshopDbContext)
        {
            _eshopDbContext = eshopDbContext;
        }

        public IList<OrderItem> Select()
        {
            return _eshopDbContext.OrderItems
                                  .Include(oi => oi.Zvire)
                                  .Include(oi => oi.Order)
                                  .ThenInclude(o => o.User)
                                  .OrderBy(oi => oi.Id)
                                  .ToList();
        }

        public void Create(OrderItem orderItem)
        {
            if (_eshopDbContext.OrderItems != null)
            {
                Order? order = _eshopDbContext.Orders.FirstOrDefault(o => o.Id == orderItem.OrderID);
                if (order != null)
                {
                    order.TotalPrice += orderItem.Price;
                    _eshopDbContext.OrderItems.Add(orderItem);
                    _eshopDbContext.SaveChanges();
                }
            }
        }

    }
}

