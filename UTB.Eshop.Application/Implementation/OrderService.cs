using System;
using Microsoft.EntityFrameworkCore;
using UTB.Eshop.Application.Abstraction;
using UTB.Eshop.Domain.Entities;
using UTB.Eshop.Infrastructure.Database;

namespace UTB.Eshop.Application.Implementation
{
    public class OrderService : IOrderService
    {
        EshopDbContext _eshopDbContext;

        public OrderService(EshopDbContext eshopDbContext)
        {
            _eshopDbContext = eshopDbContext;
        }

        public IList<Order> Select()
        {
            return _eshopDbContext.Orders
                                  .Include(oi => oi.User)
                                  .ToList();
        }
    }
}

