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
    public class ZvireAppService : IZvireAppService
    {
        IFileUploadService _fileUploadService;
        EshopDbContext _eshopDbContext;

        public ZvireAppService(IFileUploadService fileUploadService, EshopDbContext eshopDbContext)
        {
            _fileUploadService = fileUploadService;
            _eshopDbContext = eshopDbContext;
        }

        public IList<Zvire> Select()
        {
            return _eshopDbContext.Zvirata.ToList();
        }

        public async Task Create(Zvire zvire)
        {
            string imageSrc = await _fileUploadService.FileUploadAsync(zvire.Image, Path.Combine("img", "zvirata"));
            zvire.ImageSrc = imageSrc;

            if (_eshopDbContext.Zvirata != null)
            {
                _eshopDbContext.Zvirata.Add(zvire);
                _eshopDbContext.SaveChanges();
            }
        }

        public bool Delete(int id)
        {
            bool deleted = false;

            Zvire? zvire
                = _eshopDbContext.Zvirata.FirstOrDefault(prod => prod.Id == id);

            if (zvire != null)
            {
                _eshopDbContext.Zvirata.Remove(zvire);
                _eshopDbContext.SaveChanges();
                deleted = true;
            }
            return deleted;
        }
    }
}
