using AutoMapper;
using HomeWorkToDos.Business.Contract;
using HomeWorkToDos.DataAccess.Contract;
using HomeWorkToDos.Util.Dtos;
using System.Threading.Tasks;

namespace HomeWorkToDos.Business.Service
{
    /// <summary>
    /// UserService
    /// </summary>
    /// <seealso cref="HomeWorkToDos.Business.Contract.IUser" />
    public class UserService : IUser
    {
        /// <summary>
        /// The user repository
        /// </summary>
        private readonly IUserRepo _userRepository;
        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="mapper">The mapper.</param>
        public UserService(IUserRepo userRepository, IMapper mapper)
        {
            this._userRepository = userRepository;
            this._mapper = mapper;
        }

        /// <summary>
        /// Adds the user.
        /// </summary>
        /// <param name="userDto">The user dto.</param>
        /// <returns></returns>
        public async Task<int> AddUser(RegisterUserDto userDto)
        {
            return await _userRepository.AddUser(userDto);
        }
        /// <summary>
        /// Users the login.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public async Task<UserDto> UserLogin(string userName, string password)
        {
            return await _userRepository.UserLogin(userName, password);
        }

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public async Task<UserDto> GetById(int userId)
        {
            return await _userRepository.GetById(userId);
        }
       
    }
}
