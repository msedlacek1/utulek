using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Moq;

using UTB.Eshop.Web.Areas.Security.Controllers;
using UTB.Eshop.Web.Controllers;
using UTB.Eshop.Tests.Helpers;
using UTB.Eshop.Application.Abstraction;
using UTB.Eshop.Application.ViewModels;

namespace UTB.Eshop.Tests.Security.Account
{
    public class AccountControllerLoginTests
    {
        [Fact]
        public async Task Login_ValidSuccess()
        {
            // Arrange
            var mockISecurityApplicationService = new Mock<IAccountService>();
            mockISecurityApplicationService.Setup(security => security.Login(It.IsAny<LoginViewModel>()))
                                                                      //prvni verze, kdy rekneme, ze login projde
                                                                      .Returns(() => Task<bool>.Run(() => true));


            LoginViewModel loginViewModel = new LoginViewModel()
            {
                Username = "a",
                Password = "a"
            };


            AccountController controller = new AccountController(mockISecurityApplicationService.Object);

            //Act
            IActionResult iActionResult = await controller.Login(loginViewModel);


            // Assert
            RedirectToActionResult redirect = Assert.IsType<RedirectToActionResult>(iActionResult);
            Assert.Matches(nameof(HomeController.Index), redirect.ActionName);
            Assert.Matches(nameof(HomeController).Replace(nameof(Controller), String.Empty), redirect.ControllerName);
            Assert.Matches(String.Empty, redirect.RouteValues.Single((pair) => pair.Key == "area").Value.ToString());

        }


        [Fact]
        public async Task Login_InvalidFailure()
        {
            // Arrange
            var mockISecurityApplicationService = new Mock<IAccountService>();
            mockISecurityApplicationService.Setup(security => security.Login(It.IsAny<LoginViewModel>()))
                                                                      //druha verze, kdy si muzeme testovat, co je v LoginViewModel (toto delame spis jen tehdy, kdy mame setup v samostatne metode -> tzn., ze se pouziva ve vice testech, kde potrebujeme nekdy vratit true a nekdy false)
                                                                      //je zde mozne pouzit i prvni verzi vyse
                                                                      .Returns<LoginViewModel>((loginVM) =>
                                                                      {
                                                                          return Task<bool>.Run(() =>
                                                                      {
                                                                          if (loginVM.Username == "a" && loginVM.Password == "a")
                                                                          { return true; }
                                                                          else
                                                                          { return false; }
                                                                      });
                                                                      });


            LoginViewModel loginViewModel = new LoginViewModel()
            {
                Username = "a",
                Password = ""
            };


            AccountController controller = new AccountController(mockISecurityApplicationService.Object);


            //u testovani nevalidniho vstupu (pro unit testy) se pro oddeleni logiky validace a controlleru pridava primo chyba validace do ModelState
            controller.ModelState.AddModelError(nameof(LoginViewModel.Password), $"{nameof(LoginViewModel.Password)} was not set");


            //Act
            IActionResult iActionResult = await controller.Login(loginViewModel);


            // Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(iActionResult);

            Assert.NotNull(viewResult.Model);
            LoginViewModel? logingVM = viewResult.Model as LoginViewModel;
            Assert.NotNull(logingVM);
            Assert.NotNull(logingVM.Username);
            Assert.Matches(loginViewModel.Username, logingVM.Username);
            Assert.NotNull(logingVM.Password);
            Assert.Matches(loginViewModel.Password, logingVM.Password);

        }


        [Fact]
        public async Task LoginValidation_InvalidFailure()
        {
            // Arrange
            var mockISecurityApplicationService = new Mock<IAccountService>();
            mockISecurityApplicationService.Setup(security => security.Login(It.IsAny<LoginViewModel>()))
                                                                      .Returns(() => Task<bool>.Run(() => true));


            LoginViewModel loginViewModel = new LoginViewModel()
            {
                Username = "a",
                Password = ""
            };


            AccountController controller = new AccountController(mockISecurityApplicationService.Object);

            //pokud pouzivate TryValidateModel(model), tak musite nastavit ObjectValidator. Tato varianta ale testuje nejen samotnou metodu, ale i implementaci validace => jedna se o integracni test.
            //(na vstup ObjectValidator muzete nastavit true, aby se validace neprovedla).
            controller.ObjectValidator = new ObjectValidator(false);

            
            //Act
            controller.TryValidateModel(loginViewModel);
            IActionResult iActionResult = await controller.Login(loginViewModel);


            // Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(iActionResult);

            Assert.NotNull(viewResult.Model);
            LoginViewModel? logingVM = viewResult.Model as LoginViewModel;
            Assert.NotNull(logingVM);
            Assert.NotNull(logingVM.Username);
            Assert.Matches(loginViewModel.Username, logingVM.Username);
            Assert.NotNull(logingVM.Password);
            Assert.Matches(loginViewModel.Password, logingVM.Password);

        }
    }
}
