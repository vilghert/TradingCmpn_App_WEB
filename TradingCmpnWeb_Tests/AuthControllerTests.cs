using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using TradingCmpn_WEB.Controllers;
using TradingCmpn_WEB.Models;
using NUnit.Framework.Legacy;

namespace TradingCmpnWeb_Tests
{
    [TestFixture]
    public class AuthControllerTests
    {
        private Mock<IAuthService> _mockAuthService;
        private AuthController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockAuthService = new Mock<IAuthService>();
            _controller = new AuthController(_mockAuthService.Object);
        }

        [Test]
        public async Task Login_InvalidModel_ReturnsViewWithModel()
        {
            var model = new LoginModel { UserName = "", Password = "" };
            _controller.ModelState.AddModelError("UserName", "Required");

            var result = await _controller.Login(model);

            var viewResult = result as ViewResult;
            ClassicAssert.IsNotNull(viewResult);
            ClassicAssert.AreEqual(model, viewResult.Model);
        }

        [Test]
        public async Task Login_InvalidCredentials_ReturnsViewWithErrorMessage()
        {
            var model = new LoginModel { UserName = "test", Password = "wrongpassword" };

            _mockAuthService.Setup(s => s.AuthenticateAsync(model.UserName, model.Password))
                .ReturnsAsync((UserDto)null); // Імітуємо, що користувача не знайдено

            var result = await _controller.Login(model);
            var viewResult = result as ViewResult;
            ClassicAssert.IsNotNull(viewResult); // Перевіряємо, що результат - це ViewResult
            ClassicAssert.AreEqual(model, viewResult.Model); // Перевіряємо, що модель повертається такою самою
            ClassicAssert.AreEqual("Invalid username or password.", viewResult.ViewData["ErrorMessage"]);
        }

    }
}
