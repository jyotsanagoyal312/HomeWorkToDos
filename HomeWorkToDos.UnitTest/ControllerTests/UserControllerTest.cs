using HomeWorkToDos.API.Controllers;
using HomeWorkToDos.Util.ConfigModels;
using HomeWorkToDos.Util.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System.Threading.Tasks;

namespace HomeWorkToDos.UnitTest.ControllerTests
{
    /// <summary>
    /// User controller test.
    /// </summary>
    /// <seealso cref="HomeWorkToDos.UnitTest.ControllerTests.BaseController" />
    public class UserControllerTest : BaseController
    {
        /// <summary>
        /// The controller
        /// </summary>
        private UserController controller;
        /// <summary>
        /// The options
        /// </summary>
        private IOptions<JwtTokenConfig> options;

        /// <summary>
        /// Setup.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            options = Options.Create(new JwtTokenConfig { Key = "test@myPrivateKey@123456789", Issuer = "test" });
            controller = new UserController(UserLogger.Object, options, UserService.Object, Mapper)
            {
                ControllerContext = Context
            };
        }

        /// <summary>
        /// Login test.
        /// </summary>
        [Test]
        public async Task LoginTest()
        {
            IActionResult result = await controller.Login(new LoginDto { UserName = "Jyotsana", Password = "123" });
            OkObjectResult response = result as OkObjectResult;
            Assert.AreEqual(StatusCodes.Status200OK, (int)response.StatusCode);
        }


        /// <summary>
        /// Authentication test.
        /// </summary>
        [Test]
        public async Task RegistrationTest()
        {
            IActionResult result = await controller.Register(new RegisterUserDto { FirstName = "Jyotsana", LastName = "Goyal", UserName = "Jyotsana", Password = "123", Contact = "1111111111", Email = "test@mail.com" });
            OkObjectResult response = result as OkObjectResult;
            Assert.AreEqual(StatusCodes.Status200OK, (int)response.StatusCode);
        }

    }
}

