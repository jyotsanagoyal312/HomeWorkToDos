using HomeWorkToDos.DataAccess.Contract;
using HomeWorkToDos.DataAccess.Models;
using HomeWorkToDos.DataAccess.Repository;
using HomeWorkToDos.UnitTest.Util;
using HomeWorkToDos.Util.Dtos;
using NUnit.Framework;
using System.Threading.Tasks;

namespace HomeWorkToDos.UnitTest.DataAccessTests
{
    /// <summary>
    /// UserRepositoryTest
    /// </summary>
    /// <seealso cref="HomeWorkToDos.UnitTest.Util.ToDoDbContextInitiator" />
    public class UserRepositoryTest : ToDoDbContextInitiator
    {
        /// <summary>
        /// The user repository
        /// </summary>
        private UserRepository _userRepository;
        /// <summary>
        /// The repository
        /// </summary>
        private IRepository<User> _repository;

        /// <summary>
        /// Setups this instance.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _repository = new Repository<User>(DBContext);
            _userRepository = new UserRepository(_repository, Mapper);
        }

        /// <summary>
        /// Test for registering users with invalid values.
        /// </summary>
        [Test]
        public async Task Valid_RegisterUser()
        {
            int result = await _userRepository.AddUser(new RegisterUserDto { FirstName = "John", LastName = "lewis", Password = "123", UserName = "John" });

            Assert.IsTrue(result > 0);
        }
        /// <summary>
        /// Test for regisering user with valid values.
        /// </summary>
        [Test]
        public async Task Invalid_RegisterUser()
        {
            int result = await _userRepository.AddUser(new RegisterUserDto { FirstName = "Jyotsana", LastName = "Goyal", Password = "123", UserName = "Jyotsana" });

            Assert.IsTrue(result == 0);
        }
        /// <summary>
        /// Test for authentication of user.
        /// </summary>
        [Test]
        public async Task AuthenticateUser()
        {
            UserDto entity = await _userRepository.UserLogin("Jyotsana", "123");

            Assert.NotNull(entity);
        }

    }
}