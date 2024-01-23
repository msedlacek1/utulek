using System;
using System.Diagnostics;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Moq;
using UTB.Eshop.Application.Abstraction;
using UTB.Eshop.Domain.Entities;
using UTB.Eshop.Infrastructure.Database;
using UTB.Eshop.Web.Areas.Admin.Controllers;

namespace UTB.Eshop.Tests.Admin.ProductController
{
    public class ZvireControllerCreateTests
    {
        [Fact]
        public async Task Create_success()
        {
            //Arrange
            DatabaseFake.Zvirata.Clear();
            Mock<IZvireAppService> zvireServiceMock = new Mock<IZvireAppService>();
            zvireServiceMock.Setup(zvireService => zvireService.Create(It.IsAny<Zvire>()))
                                        .Returns<Zvire>(zvire => Task.Run(() => { DatabaseFake.Zvirata.Add(zvire); }));

            var zvire = GetZvire();

            var zvireController = new Web.Areas.Admin.Controllers.ZvireController(zvireServiceMock.Object);


            //Act
            var actionResult = await zvireController.Create(zvire);


            //Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(actionResult);
            Assert.NotNull(redirectToActionResult.ActionName);
            Assert.Equal(nameof(Web.Areas.Admin.Controllers.ZvireController.Index), redirectToActionResult.ActionName);

            Assert.NotEmpty(DatabaseFake.Zvirata);
            Assert.Single(DatabaseFake.Zvirata);
        }

        Zvire GetZvire()
        {
            return new Zvire()
            {
                Id = 1,
                Name = "Zvire",
                Price = 1.0,
                Description = String.Empty,
                ImageSrc = "/superimage.jpeg",
                Image = new Mock<IFormFile>().Object
            };
        }
    }
}

