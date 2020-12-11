using HomeWorkToDos.DataAccess.Contract;
using HomeWorkToDos.DataAccess.Models;
using HomeWorkToDos.DataAccess.Repository;
using HomeWorkToDos.UnitTest.Util;
using HomeWorkToDos.Util.Dtos;
using HomeWorkToDos.Util.Models;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeWorkToDos.UnitTest.DataAccessTests
{
    /// <summary>
    /// ToDoListRepositoryTest
    /// </summary>
    /// <seealso cref="HomeWorkToDos.UnitTest.Util.ToDoDbContextInitiator" />
    public class ToDoListRepositoryTest : ToDoDbContextInitiator
    {
        /// <summary>
        /// To do list repository
        /// </summary>
        private ToDoListRepository _toDoListRepository;
        /// <summary>
        /// The repository
        /// </summary>
        private IRepository<ToDoList> _repository;

        /// <summary>
        /// The pagination parameters
        /// </summary>
        readonly PaginationParameters paginationParameters = new PaginationParameters()
        {
            PageNumber = 1,
            PageSize = 10
        };

        /// <summary>
        /// Setups this instance.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _repository = new Repository<ToDoList>(DBContext);
            _toDoListRepository = new ToDoListRepository(_repository, Mapper);
        }

        /// <summary>
        /// Get ToDoList test.
        /// </summary>
        [Test, Order(1)]
        public async Task GetToDoLists()
        {
            List<ToDoListDto> ToDoListList = await _toDoListRepository.GetAllByUser(1);
            int count = ToDoListList.Count;
            Assert.IsNotNull(ToDoListList);
            Assert.IsTrue(count >= 1);
        }

        /// <summary>
        /// Add ToDoList test.
        /// </summary>
        [Test, Order(2)]
        public async Task AddToDoList()
        {
            ToDoListDto addedtoDoList = await _toDoListRepository.Add(new CreateToDoListDto { Description = "buy phone", CreatedBy = 1, UserId = 1, LabelId = 1, IsActive = true });
            Assert.IsNotNull(addedtoDoList);
            Assert.AreEqual("buy phone", addedtoDoList.Description);
        }

        /// <summary>
        /// Test to update existing ToDoItem record.
        /// </summary>
        [Test, Order(3)]
        public async Task UpdateToDoList()
        {
            ToDoListDto updatedToDoList = await _toDoListRepository.Update(new UpdateToDoListDto { ToDoListId = 2, Description = "sell phone", UserId = 1, LabelId = 1 });
            Assert.IsNotNull(updatedToDoList);
            Assert.AreEqual("sell phone", updatedToDoList.Description);
        }

        /// <summary>
        /// test to delete existing ToDoItem record.
        /// </summary>
        [Test, Order(4)]
        public async Task DeleteToDoList()
        {
            int deleteResult = await _toDoListRepository.Delete(1, 1);
            Assert.IsNotNull(deleteResult);
        }
    }
}
