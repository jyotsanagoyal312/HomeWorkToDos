using HomeWorkToDos.API.Controllers.v1;
using HomeWorkToDos.Util.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Threading.Tasks;

namespace HomeWorkToDos.UnitTest.ControllerTests
{
    /// <summary>
    /// Items controller test.
    /// </summary>
    /// <seealso cref="HomeWorkToDos.UnitTest.ControllerTests.BaseController" />
    public class ToDoItemControllerTest : BaseController
    {
        /// <summary>
        /// The controller
        /// </summary>
        private ToDoItemController controller;

        /// <summary>
        /// Setup.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            controller = new ToDoItemController(ToDoItemService.Object, Mapper)
            {
                ControllerContext = Context
            };
        }

        /// <summary>
        /// Add item test.
        /// </summary>
        [Test]
        public async Task AddTest()
        {
            IActionResult result = await controller.Add(new CreateToDoItemDto { Description = "test", ToDoListId = 1 }, Version);
            CreatedAtActionResult response = result as CreatedAtActionResult;
            Assert.AreEqual(StatusCodes.Status201Created, (int)response.StatusCode);
        }

        /// <summary>
        /// Update item test.
        /// </summary>
        [Test]
        public async Task UpdateItemTest()
        {
            IActionResult result = await controller.Update(1, new UpdateToDoItemDto { ToDoItemId = 1, Description = "test" });
            OkObjectResult response = result as OkObjectResult;
            Assert.AreEqual(StatusCodes.Status200OK, (int)response.StatusCode);
        }

        /// <summary>
        /// Delete item test.
        /// </summary>
        [Test]
        public async Task DeleteToDoItemTest()
        {
            IActionResult result = await controller.DeleteToDoItem(1);
            OkObjectResult response = result as OkObjectResult;
            Assert.AreEqual(StatusCodes.Status200OK, (int)response.StatusCode);
        }

        /// <summary>
        /// Get item test.
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
