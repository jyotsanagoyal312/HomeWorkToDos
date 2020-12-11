
using HomeWorkToDos.Util.Dtos;
using System.Threading.Tasks;

namespace HomeWorkToDos.DataAccess.Contract
{
    /// <summary>
    /// IUserRepo
    /// </summary>
    public interface IUserRepo
    {
        /// <summary>
        /// Adds the user.
        /// </summary>
        /// <param name="userDto">The user dto.</param>
        /// <returns></returns>
        Task<int> AddUser(RegisterUserDto userDto);
        /// <summary>
        /// Users the login.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        Task<UserDto> UserLogin(string userName, string password);
        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<UserDto> GetById(int userId);
    }
}
