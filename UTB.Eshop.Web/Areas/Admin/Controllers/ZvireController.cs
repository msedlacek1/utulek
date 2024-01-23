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
    public class ZvireController : Controller
    {
        IZvireAppService _zvireAppService;
        public ZvireController(IZvireAppService zvireAppService)
        {
            _zvireAppService = zvireAppService;
        }

        public IActionResult Index()
        {
            IList<Zvire> zvirata = _zvireAppService.Select();
            return View(zvirata);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Zvire zvire)
        {
            if (ModelState.IsValid)
            {
                await _zvireAppService.Create(zvire);

                return RedirectToAction(nameof(ZvireController.Index));
            }
            else
            {
                return View(zvire);
            }
        }

        public IActionResult Delete(int id)
        {
            bool deleted = _zvireAppService.Delete(id);

            if (deleted)
            {
                return RedirectToAction(nameof(ZvireController.Index));
            }
            else
                return NotFound();
        }
    }
}
