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
    public class ZvireAppDFService : IZvireAppService
    {
        IFileUploadService _fileUploadService;

        public ZvireAppDFService(IFileUploadService fileUploadService)
        {
            _fileUploadService = fileUploadService;
        }

        public IList<Zvire> Select()
        {
            return DatabaseFake.Zvirata;
        }

        public async Task Create(Zvire zvire)
        {
            if(DatabaseFake.Zvirata != null
                && DatabaseFake.Zvirata.Count > 0)
            {
                zvire.Id = DatabaseFake.Zvirata.Last().Id + 1;
            }
            else
                zvire.Id = 1;

            string imageSrc = await _fileUploadService.FileUploadAsync(zvire.Image, Path.Combine("img", "zvirata"));
            zvire.ImageSrc = imageSrc;

            if (DatabaseFake.Zvirata != null)
                DatabaseFake.Zvirata.Add(zvire);
        }

        public bool Delete(int id)
        {
            bool deleted = false;

            Zvire? zvire
                = DatabaseFake.Zvirata.FirstOrDefault(prod => prod.Id == id);

            if (zvire != null)
            {
                deleted = DatabaseFake.Zvirata.Remove(zvire);
            }
            return deleted;
        }
    }
}
