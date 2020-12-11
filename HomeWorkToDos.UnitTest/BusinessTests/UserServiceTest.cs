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
    /// User service tests.
    /// </summary>
    /// <seealso cref="HomeWorkToDos.UnitTest.Util.MapperInitiator" />
    public class UserServiceTests : MapperInitiator
    {
        /// <summary>
        /// The user repository
        /// </summary>
        private Mock<IUserRepo> _userRepository;
        /// <summary>
        /// The user service
        /// </summary>
        private IUser _userService;
        /// <summary>
        /// The user dto
        /// </summary>
        readonly RegisterUserDto userDto = new RegisterUserDto
        {
            FirstName = "Jyotsana",
            LastName = "Goyal",
            UserName = "Jyotsana",
            Password = "123",
            Contact = "1111111111",
            Email = "test@mail.com"
        };

        /// <summary>
        /// Set up.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _userRepository = new Mock<IUserRepo>();
            _userService = new UserService(_userRepository.Object, Mapper);
            _userRepository.Setup(p => p.UserLogin(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(new UserDto { UserId = 1 }));
            _userRepository.Setup(p => p.UserLogin(string.Empty, string.Empty)).Returns(Task.FromResult(new UserDto { }));
            _userRepository.Setup(p => p.AddUser(userDto)).Returns(Task.FromResult(1));
        }

        /// <summary>
        /// Auth valid test.
        /// </summary>
        [Test]
        public async Task Authenticate_ValidUserNameAndPassword()
        {
            UserDto user = await _userService.UserLogin("Jyotsana", "123");
            Assert.IsTrue(user.UserId == 1);
            Assert.AreEqual(1, user.UserId);
        }

        /// <summary>
        /// Auth invalid test.
        /// </summary>
        [Test]
        public async Task Authenticate_InvalidUserNameAndPassword()
        {
            UserDto user = await _userService.UserLogin(string.Empty, string.Empty);
            Assert.IsTrue(user.UserId != 1);
        }
        /// <summary>
        /// Registers the user.
        /// </summary>
        [Test]
        public async Task RegisterUser()
        {
            int result = await _userService.AddUser(userDto);
            Assert.AreEqual(1, result);
        }
    }
}
