using System;
using UTB.Eshop.Domain.Entities;

namespace UTB.Eshop.Application.Abstraction
{
    public interface IOrderItemService
    {
        IList<OrderItem> Select();
        void Create(OrderItem orderItem);
    }
}

