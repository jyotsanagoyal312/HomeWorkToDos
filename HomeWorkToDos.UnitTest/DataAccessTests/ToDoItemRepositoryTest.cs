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
    /// ToDoItemRepositoryTest
    /// </summary>
    /// <seealso cref="HomeWorkToDos.UnitTest.Util.ToDoDbContextInitiator" />
    public class ToDoItemRepositoryTest : ToDoDbContextInitiator
    {
        /// <summary>
        /// To do item repository
        /// </summary>
        private ToDoItemRepository _toDoItemRepository;
        /// <summary>
        /// The repository
        /// </summary>
        private IRepository<ToDoItem> _repository;

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
            _repository = new Repository<ToDoItem>(DBContext);
            _toDoItemRepository = new ToDoItemRepository(_repository, Mapper);
        }

        /// <summary>
        /// Get ToDoItems test.
        /// </summary>
        [Test, Order(1)]
        public async Task GetToDoItems()
        {
            List<ToDoItemDto> toDoItemList = await _toDoItemRepository.GetAllByUser(1);
            int count = toDoItemList.Count;
            Assert.IsNotNull(toDoItemList);
            Assert.IsTrue(count >= 1);
        }

        /// <summary>
        /// Add ToDoItem test.
        /// </summary>
        [Test, Order(2)]
        public async Task AddToDoItem()
        {
            ToDoItemDto addedToDoItem = await _toDoItemRepository.Add(new CreateToDoItemDto { Description = "buy phone", CreatedBy = 1, ToDoListId = 1, UserId = 1, LabelId = 1, IsActive = true });
            Assert.IsNotNull(addedToDoItem);
            Assert.AreEqual("buy phone", addedToDoItem.Description);
        }

        /// <summary>
        /// Test to update existing ToDoItem record.
        /// </summary>
        [Test, Order(3)]
        public async Task UpdateToDoItem()
        {
            ToDoItemDto updatedToDoItem = await _toDoItemRepository.Update(new UpdateToDoItemDto { ToDoItemId = 2, Description = "sell phone", UserId = 1, LabelId = 1, ToDoListId = 1 });
            Assert.IsNotNull(updatedToDoItem);
            Assert.AreEqual("sell phone", updatedToDoItem.Description);
        }

        /// <summary>
        /// test to delete existing ToDoItem record.
        /// </summary>
        [Test, Order(4)]
        public async Task DeleteToDoItem()
        {
            int deleteResult = await _toDoItemRepository.Delete(1, 1);
            Assert.IsNotNull(deleteResult);
        }
    }
}
