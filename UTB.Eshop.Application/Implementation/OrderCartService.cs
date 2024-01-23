using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UTB.Eshop.Application.Abstraction;
using UTB.Eshop.Domain.Entities;
using UTB.Eshop.Infrastructure.Database;
using UTB.Eshop.Infrastructure.Identity;

namespace UTB.Eshop.Application.Implementation
{
    public class OrderCartService : IOrderCartService
    {
        const string totalPriceString = "TotalPrice";
        const string orderItemsString = "OrderItems";


        EshopDbContext _eshopDbContext;

        public OrderCartService(EshopDbContext eshopDbContext)
        {
            _eshopDbContext = eshopDbContext;
        }


        public double AddOrderItemsToSession(int? zvireId, ISession session)
        {
            //get total price from session
            double totalPrice = session.GetDouble(totalPriceString).GetValueOrDefault();


            //geet product which should be added to cart/session
            Zvire? zvire = _eshopDbContext.Zvirata.FirstOrDefault(prod => prod.Id == zvireId);

            if (zvire != null)
            {
                //get list of order items from session
                List<OrderItem> orderItems = session.GetObject<List<OrderItem>>(orderItemsString);
                OrderItem? orderItemInSession = null;
                //if the list is already in the session, find the order item with the ProductID, otherwise, create new list
                if (orderItems != null)
                    orderItemInSession = orderItems.Find(oi => oi.ZvireID == zvire.Id);
                else
                    orderItems = new List<OrderItem>();


                //if there is order item with ProductID, increase amount and price, otherwise, add new OrderItem
                if (orderItemInSession != null)
                {
                    ++orderItemInSession.Amount;
                    orderItemInSession.Price += zvire.Price;
                }
                else
                {
                    //create order item with connected product and add it to the list
                    OrderItem orderItem = new OrderItem()
                    {
                        ZvireID = zvire.Id,
                        Zvire = zvire,
                        Amount = 1,
                        Price = zvire.Price
                    };

                    orderItems.Add(orderItem);
                }

                //set the list to the session
                session.SetObject(orderItemsString, orderItems);

                //increase the total price and set it to the session
                totalPrice += zvire.Price;
                session.SetDouble(totalPriceString, totalPrice);
            }

            //return total price
            return totalPrice;
        }


        public bool ApproveOrderInSession(int userId, ISession session)
        {
            //get order items from the session
            List<OrderItem> orderItems = session.GetObject<List<OrderItem>>(orderItemsString);
            if (orderItems != null)
            {

                //get total price from session
                double totalPrice = session.GetDouble(totalPriceString).GetValueOrDefault();

                //reference to the product must be null; otherwise, it tries to add it to the database again
                orderItems.ForEach(orderItem => orderItem.Zvire = null);

                ////alternate option for the problem above
                //double totalPrice = 0;
                //foreach (OrderItem orderItem in orderItems)
                //{
                //    //recalculate total price (just to be sure; the total price is in the session too and the value should be same)
                //    totalPrice += orderItem.Product.Price * orderItem.Amount;
                //    //reference to the product must be null; otherwise, it tries to add it to the database again
                //    orderItem.Product = null;
                //}


                //create new order and connect it with the list of the order items
                Order order = new Order()
                {
                    OrderNumber = Convert.ToBase64String(Guid.NewGuid().ToByteArray()),
                    TotalPrice = totalPrice,
                    OrderItems = orderItems,
                    UserId = userId
                };



                //We can add just the order; connected order items will be added automatically to the database.
                _eshopDbContext.Add(order);
                _eshopDbContext.SaveChanges();


                //remove the informations from the session
                session.Remove(orderItemsString);
                session.Remove(totalPriceString);


                //success
                return true;

            }


            //failure
            return false;
        }

    }
}

