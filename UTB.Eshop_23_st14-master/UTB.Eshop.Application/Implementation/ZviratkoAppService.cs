using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTB.Eshop.Application.Abstraction;
using UTB.Eshop.Domain.Entities;
using UTB.Eshop.Infrastructure.Database;

namespace UTB.Eshop.Application.Implementation
{
    public class ZviratkoAppService : IZviratkoAppService
    {
        IFileUploadService _fileUploadService;
        EshopDbContext _eshopDbContext;

        public ZviratkoAppService(IFileUploadService fileUploadService, EshopDbContext eshopDbContext)
        {
            _fileUploadService = fileUploadService;
            _eshopDbContext = eshopDbContext;
        }

        public IList<Zviratko> Select()
        {
            return _eshopDbContext.Zviratka.ToList();
        }

        public async Task Create(Zviratko zviratko)
        {
            string imageSrc = await _fileUploadService.FileUploadAsync(zviratko.Image, Path.Combine("img", "products"));
            zviratko.ImageSrc = imageSrc;

            if (_eshopDbContext.Zviratka != null)
            {
                _eshopDbContext.Zviratka.Add(zviratko);
                _eshopDbContext.SaveChanges();
            }
        }

        public bool Delete(int id)
        {
            bool deleted = false;

            Zviratko? zviratko
                = _eshopDbContext.Zviratka.FirstOrDefault(prod => prod.Id == id);

            if (zviratko != null)
            {
                _eshopDbContext.Zviratka.Remove(zviratko);
                _eshopDbContext.SaveChanges();
                deleted = true;
            }
            return deleted;
        }
    }
}
