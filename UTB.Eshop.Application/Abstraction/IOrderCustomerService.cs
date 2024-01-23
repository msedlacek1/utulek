using System;
using UTB.Eshop.Domain.Entities;

namespace UTB.Eshop.Application.Abstraction
{
    public interface IOrderCustomerService
    {
        IList<Order> GetOrdersForUser(int userId);
    }
}

