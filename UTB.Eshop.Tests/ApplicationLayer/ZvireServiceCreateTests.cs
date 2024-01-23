using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

using Moq;

using UTB.Eshop.Application.Abstraction;
using UTB.Eshop.Infrastructure.Database;
using UTB.Eshop.Domain.Entities;
using UTB.Eshop.Application.Implementation;

namespace UTB.Eshop.Tests.ApplicationLayer
{
    public class ZvireServiceCreateTests
    {
        [Fact]
        public async Task Create_Success()
        {
            // Arrange

            //nastaveni falesne sluzby pro produkty
            var pathToImage = "img/product/UploadImageFile.png";
            var fileUpload = new Mock<IFileUploadService>();
            fileUpload.Setup(fu => fu.FileUploadAsync(It.IsAny<IFormFile>(), It.IsAny<string>())).Returns(() => Task.Run<string>(() => pathToImage));


            //nastaveni falesne IFormFile
            Mock<IFormFile> iffMock = new Mock<IFormFile>();
            iffMock.Setup(iff => iff.CopyToAsync(It.IsAny<Stream>(), CancellationToken.None))
                                    .Callback<Stream, CancellationToken>((stream, token) =>
                                    {
                                        return;
                                    })
                                    .Returns(Task.CompletedTask);



            //Nainstalovan Nuget package: Microsoft.EntityFrameworkCore.InMemory
            //databazi vytvori v pameti
            DbContextOptions options = new DbContextOptionsBuilder<EshopDbContext>()
                                       .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                                       .Options;
            var databaseContext = new EshopDbContext(options);
            databaseContext.Database.EnsureCreated();
            //smazani inicializacnich dat, pokud existuji
            databaseContext.Zvirata.RemoveRange(databaseContext.Zvirata);
            databaseContext.SaveChanges();



            ZvireAppService service = new ZvireAppService(fileUpload.Object, databaseContext);


            Zvire testZvire = GetTestZvire(iffMock.Object);



            //Act
            await service.Create(testZvire);



            // Assert
            Assert.Single(databaseContext.Zvirata);

            Zvire addedZvire = databaseContext.Zvirata.First();
            Assert.Equal(testZvire.Id, addedZvire.Id);
            Assert.NotNull(addedZvire.Name);
            Assert.Matches(testZvire.Name, addedZvire.Name);
            Assert.Equal(testZvire.Price, addedZvire.Price);
            Assert.NotNull(addedZvire.ImageSrc);
            Assert.Matches(pathToImage, addedZvire.ImageSrc);

        }



        Zvire GetTestZvire(IFormFile iff)
        {
            return new Zvire()
            {
                Id = 1,
                Name = "zvire",
                Price = 10,
                ImageSrc = String.Empty,
                Image = iff
            };
        }

    }
}
