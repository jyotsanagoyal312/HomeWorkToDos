using HomeWorkToDos.API.Controllers.v1;
using HomeWorkToDos.Util.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Threading.Tasks;

namespace HomeWorkToDos.UnitTest.ControllerTests
{
    /// <summary>
    /// Lists controller test.
    /// </summary>
    /// <seealso cref="HomeWorkToDos.UnitTest.ControllerTests.BaseController" />
    public class ToDoListControllerTest : BaseController
    {
        /// <summary>
        /// The controller
        /// </summary>
        private ToDoListController controller;

        /// <summary>
        /// Setup.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            controller = new ToDoListController(ToDoListService.Object, Mapper)
            {
                ControllerContext = Context
            };
        }

        /// <summary>
        /// Add list test.
        /// </summary>
        [Test]
        public async Task AddTest()
        {
            IActionResult result = await controller.Add(new CreateToDoListDto { Description = "test" }, Version);
            CreatedAtActionResult response = result as CreatedAtActionResult;
            Assert.AreEqual(StatusCodes.Status201Created, (int)response.StatusCode);
        }

        /// <summary>
        /// Update list test.
        /// </summary>
        [Test]
        public async Task UpdateTest()
        {
            IActionResult result = await controller.Update(1, new UpdateToDoListDto { ToDoListId = 1, Description = "test" });
            OkObjectResult response = result as OkObjectResult;
            Assert.AreEqual(StatusCodes.Status200OK, (int)response.StatusCode);
        }

        /// <summary>
        /// Delete list test.
        /// </summary>
        [Test]
        public async Task DeleteTest()
        {
            IActionResult result = await controller.Delete(1);
            OkObjectResult response = result as OkObjectResult;
            Assert.AreEqual(StatusCodes.Status200OK, (int)response.StatusCode);
        }

        /// <summary>
        /// Get list by id test.
        /// </summary>
        [Test]
        public async Task GetByIdTest()
        {
            IActionResult result = await controller.GetById(1);
            OkObjectResult response = result as OkObjectResult;
            Assert.AreEqual(StatusCodes.Status200OK, (int)response.StatusCode);
        }
    }
}

