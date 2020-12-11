using HomeWorkToDos.DataAccess.Contract;
using HomeWorkToDos.DataAccess.Models;
using HomeWorkToDos.DataAccess.Repository;
using HomeWorkToDos.UnitTest.Util;
using HomeWorkToDos.Util.Dtos;
using HomeWorkToDos.Util.Models;
using NUnit.Framework;
using System.Threading.Tasks;

namespace HomeWorkToDos.UnitTest.DataAccessTests
{
    /// <summary>
    /// LabelRepositoryTest
    /// </summary>
    /// <seealso cref="HomeWorkToDos.UnitTest.Util.ToDoDbContextInitiator" />
    public class LabelRepositoryTest : ToDoDbContextInitiator
    {
        /// <summary>
        /// The label repository
        /// </summary>
        private LabelRepository _labelRepository;
        /// <summary>
        /// The repository
        /// </summary>
        private IRepository<Label> _repository;

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
            _repository = new Repository<Label>(DBContext);
            _labelRepository = new LabelRepository(_repository, Mapper);
        }

        /// <summary>
        /// Add label test.
        /// </summary>
        [Test, Order(1)]
        public async Task AddLabel()
        {
            LabelDto addedLabel = await _labelRepository.Add(new CreateLabelDto { Description = "buy phone", CreatedBy = 1 });
            Assert.IsNotNull(addedLabel);
            Assert.AreEqual("buy phone", addedLabel.Description);
        }

        /// <summary>
        /// Get labels test.
        /// </summary>
        [Test, Order(2)]
        public async Task GetLabels()
        {
            PagedList<LabelDto> LabelList = await _labelRepository.GetByUser(paginationParameters, 1);
            int count = LabelList.Count;
            Assert.IsNotNull(LabelList);
            Assert.IsTrue(count >= 1);
        }

        /// <summary>
        /// test to delete existing Label record.
        /// </summary>
        [Test, Order(3)]
        public async Task DeleteLabel()
        {
            int deleteResult = await _labelRepository.Delete(1, 1);
            Assert.IsNotNull(deleteResult);
            Assert.AreEqual(1, deleteResult);
        }
    }
}