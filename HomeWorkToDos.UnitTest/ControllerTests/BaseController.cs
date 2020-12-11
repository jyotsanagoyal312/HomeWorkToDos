using HomeWorkToDos.API.Controllers;
using HomeWorkToDos.API.Controllers.v1;
using HomeWorkToDos.Business.Contract;
using HomeWorkToDos.UnitTest.Util;
using HomeWorkToDos.Util.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;

namespace HomeWorkToDos.UnitTest.ControllerTests
{
    /// <summary>
    /// Base class for controller tests.
    /// </summary>
    /// <seealso cref="HomeWorkToDos.UnitTest.Util.MapperInitiator" />
    public class BaseController : MapperInitiator
    {
        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <value>
        /// The context.
        /// </value>
        public ControllerContext Context { get; }
        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public ApiVersion Version { get; }

        /// <summary>
        /// Gets the label logger.
        /// </summary>
        /// <value>
        /// The label logger.
        /// </value>
        public Mock<ILogger<LabelController>> LabelLogger { get; }
        /// <summary>
        /// Converts to doitemlogger.
        /// </summary>
        /// <value>
        /// To do item logger.
        /// </value>
        public Mock<ILogger<ToDoItemController>> ToDoItemLogger { get; }
        /// <summary>
        /// Converts to dolistlogger.
        /// </summary>
        /// <value>
        /// To do list logger.
        /// </value>
        public Mock<ILogger<ToDoListController>> ToDoListLogger { get; }
        /// <summary>
        /// Gets the user logger.
        /// </summary>
        /// <value>
        /// The user logger.
        /// </value>
        public Mock<ILogger<UserController>> UserLogger { get; }

        /// <summary>
        /// Gets the label service.
        /// </summary>
        /// <value>
        /// The label service.
        /// </value>
        public Mock<ILabel> LabelService { get; }
        /// <summary>
        /// Converts to doitemservice.
        /// </summary>
        /// <value>
        /// To do item service.
        /// </value>
        public Mock<IToDoItem> ToDoItemService { get; }
        /// <summary>
        /// Converts to dolistservice.
        /// </summary>
        /// <value>
        /// To do list service.
        /// </value>
        public Mock<IToDoList> ToDoListService { get; }
        /// <summary>
        /// Gets the user service.
        /// </summary>
        /// <value>
        /// The user service.
        /// </value>
        public Mock<IUser> UserService { get; }

        /// <summary>
        /// To do item dto
        /// </summary>
        private readonly ToDoItemDto _toDoItemDto = new ToDoItemDto { ToDoItemId = 1 };
        /// <summary>
        /// The label dto
        /// </summary>
        private readonly LabelDto _labelDto = new LabelDto { LabelId = 1 };
        /// <summary>
        /// To do list dto
        /// </summary>
        private readonly ToDoListDto _toDoListDto = new ToDoListDto { ToDoListId = 1 };
        /// <summary>
        /// The user dto
        /// </summary>
        private readonly UserDto _userDto = new UserDto { UserId = 1, UserName = "Jyotsana" };

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseController"/> class.
        /// </summary>
        protected BaseController()
        {
            Context = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            Version = new ApiVersion(1, 0);
            LabelService = new Mock<ILabel>();
            ToDoItemService = new Mock<IToDoItem>();
            ToDoListService = new Mock<IToDoList>();
            UserService = new Mock<IUser>();
            LabelLogger = new Mock<ILogger<LabelController>>();
            ToDoItemLogger = new Mock<ILogger<ToDoItemController>>();
            ToDoListLogger = new Mock<ILogger<ToDoListController>>();
            UserLogger = new Mock<ILogger<UserController>>();

            Context.HttpContext.Items["UserId"] = 1;
            //Mock methods
            LabelService.Setup(p => p.Add(It.IsAny<CreateLabelDto>())).Returns(Task.FromResult(_labelDto));
            LabelService.Setup(p => p.Delete(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult(1));
            LabelService.Setup(p => p.GetById(1, 1)).Returns(Task.FromResult(_labelDto));
            ToDoItemService.Setup(p => p.Add(It.IsAny<CreateToDoItemDto>())).Returns(Task.FromResult(_toDoItemDto));
            ToDoItemService.Setup(p => p.Update(It.IsAny<UpdateToDoItemDto>())).Returns(Task.FromResult(_toDoItemDto));
            ToDoItemService.Setup(p => p.Delete(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult(1));
            ToDoItemService.Setup(p => p.GetById(1, 1)).Returns(Task.FromResult(_toDoItemDto));
            ToDoListService.Setup(p => p.GetById(1, 1)).Returns(Task.FromResult(_toDoListDto));
            ToDoListService.Setup(p => p.Add(It.IsAny<CreateToDoListDto>())).Returns(Task.FromResult(_toDoListDto));
            ToDoListService.Setup(p => p.Update(It.IsAny<UpdateToDoListDto>())).Returns(Task.FromResult(_toDoListDto));
            ToDoListService.Setup(p => p.Delete(It.IsAny<long>(), It.IsAny<long>())).Returns(Task.FromResult(1));
            UserService.Setup(p => p.UserLogin(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(_userDto));
            UserService.Setup(p => p.AddUser(It.IsAny<RegisterUserDto>())).Returns(Task.FromResult(1));
        }
    }
}

