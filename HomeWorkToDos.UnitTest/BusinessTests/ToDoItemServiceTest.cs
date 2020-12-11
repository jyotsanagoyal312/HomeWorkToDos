using HomeWorkToDos.Business.Contract;
using HomeWorkToDos.Business.Service;
using HomeWorkToDos.DataAccess.Contract;
using HomeWorkToDos.UnitTest.Util;
using HomeWorkToDos.Util.Dtos;
using HomeWorkToDos.Util.Models;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeWorkToDos.UnitTest.BusinessTests
{
    /// <summary>
    /// ToDoItemServiceTest
    /// </summary>
    /// <seealso cref="HomeWorkToDos.UnitTest.Util.MapperInitiator" />
    public class ToDoItemServiceTest : MapperInitiator
    {
        /// <summary>
        /// To do item repository
        /// </summary>
        private Mock<IToDoItemRepo> _ToDoItemRepository;
        /// <summary>
        /// To do item service
        /// </summary>
        private IToDoItem _ToDoItemService;
        /// <summary>
        /// To do item dto
        /// </summary>
        private readonly ToDoItemDto _toDoItemDto = new ToDoItemDto
        {
            ToDoItemId = 1,
            Description = "test"
        };
        /// <summary>
        /// To do item dtos
        /// </summary>
        readonly PagedList<ToDoItemDto> _toDoItemDtos = new PagedList<ToDoItemDto>(new List<ToDoItemDto>(), 0, 1, 10);
        /// <summary>
        /// The pagination parameters
        /// </summary>
        readonly PaginationParameters paginationParameters = new PaginationParameters()
        {
            PageNumber = 1,
            PageSize = 10,
            SearchText = "Something"
        };
        /// <summary>
        /// Setup tests for todoitems.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _ToDoItemRepository = new Mock<IToDoItemRepo>();
            _ToDoItemService = new ToDoItemService(_ToDoItemRepository.Object, Mapper);
            _ToDoItemRepository.Setup(p => p.Add(It.IsAny<CreateToDoItemDto>())).Returns(Task.FromResult(_toDoItemDto));
            _ToDoItemRepository.Setup(p => p.Update(It.IsAny<UpdateToDoItemDto>())).Returns(Task.FromResult(_toDoItemDto));
            _ToDoItemRepository.Setup(p => p.Delete(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult(1));
            _ToDoItemRepository.Setup(p => p.GetById(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult(_toDoItemDto));
            _ToDoItemRepository.Setup(p => p.GetAllByUser(It.IsAny<PaginationParameters>(), It.IsAny<int>())).Returns(Task.FromResult(_toDoItemDtos));
        }

        /// <summary>
        /// Test to add ToDoItem record.
        /// </summary>
        [Test]
        public async Task AddTest()
        {
            ToDoItemDto result = await _ToDoItemService.Add(new CreateToDoItemDto());
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.ToDoItemId);
        }

        /// <summary>
        /// test to Update ToDoItem.
        /// </summary>
        [Test]
        public async Task UpdateTest()
        {
            ToDoItemDto result = await _ToDoItemService.Update(new UpdateToDoItemDto());
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.ToDoItemId);
        }
        /// <summary>
        /// test to delte ToDoItem record.
        /// </summary>
        [Test]
        public async Task DeleteTest()
        {
            int result = await _ToDoItemService.Delete(1, 1);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result);
        }
        /// <summary>
        /// test to get ToDoItem record by id.
        /// </summary>
        [Test]
        public async Task GetByIdTest()
        {
            ToDoItemDto result = await _ToDoItemService.GetById(1, 1);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.ToDoItemId);
        }
        /// <summary>
        /// Test to get all todoitem records.
        /// </summary>
        [Test]
        public async Task GetAllTest()
        {
            PagedList<ToDoItemDto> result = await _ToDoItemService.GetAllByUser(paginationParameters, 1);
            Assert.IsNotNull(result);
        }
    }
}