using HomeWorkToDos.API.Controllers.v1;
using HomeWorkToDos.Util.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Threading.Tasks;

namespace HomeWorkToDos.UnitTest.ControllerTests
{
    /// <summary>
    /// Label controller tests.
    /// </summary>
    /// <seealso cref="HomeWorkToDos.UnitTest.ControllerTests.BaseController" />
    public class LabelControllerTest : BaseController
    {
        /// <summary>
        /// The controller
        /// </summary>
        private LabelController controller;

        /// <summary>
        /// Setup.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            controller = new LabelController(LabelService.Object, Mapper)
            {
                ControllerContext = Context
            };
        }

        /// <summary>
        /// Add label test.
        /// </summary>
        [Test]
        public async Task AddTest()
        {
            IActionResult result = await controller.Add(new CreateLabelDto { Description = "test" }, Version);
            CreatedAtActionResult response = result as CreatedAtActionResult;
            Assert.AreEqual(StatusCodes.Status201Created, (int)response.StatusCode);
        }

        /// <summary>
        /// Delete label test.
        /// </summary>
        [Test]
        public async Task DeleteLabelTest()
        {
            IActionResult result = await controller.DeleteLabel(1);
            OkObjectResult response = result as OkObjectResult;
            Assert.AreEqual(StatusCodes.Status200OK, (int)response.StatusCode);
        }

        /// <summary>
        /// Get label test.
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
