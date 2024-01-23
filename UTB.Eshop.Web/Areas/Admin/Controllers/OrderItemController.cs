using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UTB.Eshop.Application.Abstraction;
using UTB.Eshop.Domain.Entities;
using UTB.Eshop.Infrastructure.Identity.Enums;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UTB.Eshop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = nameof(Roles.Admin) + ", " + nameof(Roles.Manager))]
    public class OrderItemController : Controller
    {
        IOrderItemService _orderItemService;
        IZvireAppService _zvireService;
        IOrderService _orderService;
        public OrderItemController(IOrderItemService orderItemService, IZvireAppService zvireService, IOrderService orderService)
        {
            _orderItemService = orderItemService;
            _zvireService = zvireService;
            _orderService = orderService;
        }

        public IActionResult Index()
        {
            IList<OrderItem> orderItems = _orderItemService.Select();
            return View(orderItems);
        }

        [HttpGet]
        public IActionResult Create()
        {
            SetOrderAndZvireSelectLists();
            return View();
        }

        [HttpPost]
        public IActionResult Create(OrderItem orderItem)
        {
            if (ModelState.IsValid)
            {
                _orderItemService.Create(orderItem);
                return RedirectToAction(nameof(OrderItemController.Index));
            }
            else
            {
                SetOrderAndZvireSelectLists();
                return View(orderItem);
            }
        }

        void SetOrderAndZvireSelectLists()
        {
            IList<Zvire> zvirata = _zvireService.Select();
            ViewData[nameof(OrderItem.ZvireID)] = new SelectList(zvirata, nameof(Zvire.Id), nameof(Zvire.Name));

            IList<Order> orders = _orderService.Select();
            ViewData[nameof(OrderItem.OrderID)] = new SelectList(orders, nameof(Order.Id), nameof(Order.OrderNumber));
        }
    }
}

