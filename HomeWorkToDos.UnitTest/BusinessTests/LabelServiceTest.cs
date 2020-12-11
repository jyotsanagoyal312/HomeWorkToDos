using HomeWorkToDos.Business.Contract;
using HomeWorkToDos.Business.Service;
using HomeWorkToDos.DataAccess.Contract;
using HomeWorkToDos.UnitTest.Util;
using HomeWorkToDos.Util.Dtos;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace HomeWorkToDos.UnitTest.BusinessTests
{
    /// <summary>
    /// Label service tests.
    /// </summary>
    /// <seealso cref="HomeWorkToDos.UnitTest.Util.MapperInitiator" />
    public class LabelServiceTest : MapperInitiator
    {
        /// <summary>
        /// The label repository
        /// </summary>
        private Mock<ILabelRepo> _labelRepository;
        /// <summary>
        /// The label service
        /// </summary>
        private ILabel _labelService;
        /// <summary>
        /// The label dto
        /// </summary>
        private readonly LabelDto _labelDto = new LabelDto { LabelId = 1, Description = "test" };

        /// <summary>
        /// Setup.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _labelRepository = new Mock<ILabelRepo>();
            _labelService = new LabelService(_labelRepository.Object, Mapper);
            _labelRepository.Setup(p => p.Add(It.IsAny<CreateLabelDto>())).Returns(Task.FromResult(_labelDto));
            _labelRepository.Setup(p => p.Delete(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult(1));
            _labelRepository.Setup(p => p.GetById(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult(_labelDto));
        }

        /// <summary>
        /// Add label test.
        /// </summary>
        [Test]
        public async Task AddLabelTest()
        {
            LabelDto result = await _labelService.Add(new CreateLabelDto() { Description = "test", CreatedBy = 1 });
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.LabelId);
        }

        /// <summary>
        /// Add label test.
        /// </summary>
        [Test]
        public async Task DeleteLabelTest()
        {
            int result = await _labelService.Delete(1, 1);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result);
        }

        /// <summary>
        /// Get label by id test.
        /// </summary>
        [Test]
        public async Task GetByIdTest()
        {
            LabelDto result = await _labelService.GetById(1, 1);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.LabelId);
        }
    }
}

