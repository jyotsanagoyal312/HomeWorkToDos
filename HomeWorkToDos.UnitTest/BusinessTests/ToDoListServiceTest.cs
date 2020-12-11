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
    /// ToDoListServiceTest
    /// </summary>
    /// <seealso cref="HomeWorkToDos.UnitTest.Util.MapperInitiator" />
    public class ToDoListServiceTest : MapperInitiator
    {
        /// <summary>
        /// To do list repository
        /// </summary>
        private Mock<IToDoListRepo> _toDoListRepository;
        /// <summary>
        /// To do list service
        /// </summary>
        private IToDoList _toDoListService;
        /// <summary>
        /// To do list dto
        /// </summary>
        private readonly ToDoListDto _toDoListDto = new ToDoListDto { ToDoListId = 1, Description = "test" };
        /// <summary>
        /// To do list dtos
        /// </summary>
        readonly PagedList<ToDoListDto> _toDoListDtos = new PagedList<ToDoListDto>(new List<ToDoListDto>(), 0, 1, 10);
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
        /// Setup.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _toDoListRepository = new Mock<IToDoListRepo>();
            _toDoListService = new ToDoListService(_toDoListRepository.Object, Mapper);
            _toDoListRepository.Setup(p => p.Add(It.IsAny<CreateToDoListDto>())).Returns(Task.FromResult(_toDoListDto));
            _toDoListRepository.Setup(p => p.Update(It.IsAny<UpdateToDoListDto>())).Returns(Task.FromResult(_toDoListDto));
            _toDoListRepository.Setup(p => p.Delete(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult(1));
            _toDoListRepository.Setup(p => p.GetById(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult(_toDoListDto));
            _toDoListRepository.Setup(p => p.GetAllByUser(It.IsAny<PaginationParameters>(), It.IsAny<int>())).Returns(Task.FromResult(_toDoListDtos));
        }

        /// <summary>
        /// Add ToDoList test.
        /// </summary>
        [Test]
        public async Task AddToDoListTest()
        {
            ToDoListDto result = await _toDoListService.Add(new CreateToDoListDto());
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.ToDoListId);
        }
        /// <summary>
        /// Updates to do list test.
        /// </summary>
        [Test]
        public async Task UpdateToDoListTest()
        {
            ToDoListDto result = await _toDoListService.Update(new UpdateToDoListDto());
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.ToDoListId);
        }
        /// <summary>
        /// Deletes to do list test.
        /// </summary>
        [Test]
        public async Task DeleteToDoListTest()
        {
            int result = await _toDoListService.Delete(1, 1);
            Assert.IsNotNull(result);
        }
        /// <summary>
        /// Gets to do list by identifier.
        /// </summary>
        [Test]
        public async Task GetToDoListById()
        {
            ToDoListDto result = await _toDoListService.GetById(1, 1);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.ToDoListId);
        }
        /// <summary>
        /// Gets to do lists.
        /// </summary>
        [Test]
        public async Task GetToDoLists()
        {
            PagedList<ToDoListDto> result = await _toDoListService.GetAllByUser(paginationParameters, 1);
            Assert.IsNotNull(result);
        }
    }
}
