 using Microsoft.AspNetCore.Mvc;
using UTB.Eshop.Application.Abstraction;
using UTB.Eshop.Domain.Entities;
using UTB.Eshop.Infrastructure.Database;
using Microsoft.AspNetCore.Authorization;
using UTB.Eshop.Infrastructure.Identity.Enums;

namespace UTB.Eshop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = nameof(Roles.Admin) + ", " + nameof(Roles.Manager))]
    public class ZviratkoController : Controller
    {
        IZviratkoAppService _zviratkoAppService;
        public ZviratkoController(IZviratkoAppService zviratkoAppService)
        {
            _zviratkoAppService = zviratkoAppService;
        }

        public IActionResult Index()
        {
            IList<Zviratko> zviratka = _zviratkoAppService.Select();
            return View(zviratka);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Zviratko zviratko)
        {
            if (ModelState.IsValid)
            {
                await _zviratkoAppService.Create(zviratko);

                return RedirectToAction(nameof(ZviratkoController.Index));
            }
            else
            {
                return View(zviratko);
            }
        }

        public IActionResult Delete(int id)
        {
            bool deleted = _zviratkoAppService.Delete(id);

            if (deleted)
            {
                return RedirectToAction(nameof(ZviratkoController.Index));
            }
            else
                return NotFound();
        }
    }
}
