using System;
using UTB.Eshop.Domain.Entities;

namespace UTB.Eshop.Application.Abstraction
{
    public interface IOrderService
    {
        IList<Order> Select();
    }
}

