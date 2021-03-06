﻿using AutoMapper;
using HomeWorkToDos.DataAccess.Contract;
using HomeWorkToDos.DataAccess.Models;
using HomeWorkToDos.Util.Dtos;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HomeWorkToDos.Util.Helper;

namespace HomeWorkToDos.DataAccess.Repository
{
    /// <summary>
    /// UserRepository
    /// </summary>
    /// <seealso cref="HomeWorkToDos.DataAccess.Contract.IUserRepo" />
    public class UserRepository : IUserRepo
    {
        /// <summary>
        /// The user repository
        /// </summary>
        private readonly IRepository<User> _userRepository;
        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="mapper">The mapper.</param>
        public UserRepository(IRepository<User> userRepository, IMapper mapper)
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
            if (userDto.Password != null)
            {
                userDto.Password = CommonHelper.EncodePasswordToBase64(userDto.Password);
            }

            var userExists = await _userRepository.FilterList(x => x.UserName.ToLower() == userDto.UserName.ToLower()).AnyAsync();
            if (userExists)
            {
                // user already exists in db with username
                return 0;
            }

            var user = _mapper.Map<User>(userDto);
            user = _userRepository.Add(user);
            await _userRepository.Save();
            return user.UserId;
        }
        /// <summary>
        /// Users the login.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public async Task<UserDto> UserLogin(string userName, string password)
        {
            var encodedPassword = CommonHelper.EncodePasswordToBase64(password);
            var user = await _userRepository.FilterList(x => x.UserName.ToLower() == userName.ToLower() && x.Password == encodedPassword).FirstOrDefaultAsync();
            if (user == null)
            {
                // user not found in db with credentials
                return null;
            }

            var userDto = _mapper.Map<UserDto>(user);
            return userDto;
        }

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public async Task<UserDto> GetById(int userId)
        {
            var user = await _userRepository.FindById(userId);
            if (user == null)
            {
                // user not found in db with credentials
                return null;
            }

            var userDto = _mapper.Map<UserDto>(user);
            return userDto;
        }
       
    }
}
