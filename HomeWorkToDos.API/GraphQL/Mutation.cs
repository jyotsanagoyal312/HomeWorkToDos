using HomeWorkToDos.DataAccess.Contract;
using HomeWorkToDos.Util.Auth;
using HomeWorkToDos.Util.ConfigModels;
using HomeWorkToDos.Util.Dtos;
using HomeWorkToDos.Util.Models;
using HotChocolate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace HomeWorkToDos.API.GraphQL
{
    /// <summary>
    /// Mutation class for GraphQl.
    /// </summary>
    [Authorize]
    public class Mutation
    {
        /// <summary>
        /// To do item repo
        /// </summary>
        private readonly IToDoItemRepo _toDoItemRepo;
        /// <summary>
        /// To do list repo
        /// </summary>
        private readonly IToDoListRepo _toDoListRepo;
        /// <summary>
        /// The label repo
        /// </summary>
        private readonly ILabelRepo _labelRepo;
        /// <summary>
        /// The user repo
        /// </summary>
        private readonly IUserRepo _userRepo;
        /// <summary>
        /// The user identifier
        /// </summary>
        private readonly int _userId;
        /// <summary>
        /// The token configuration
        /// </summary>
        private readonly JwtTokenConfig _tokenConfig;

        /// <summary>
        /// Initializes a new instance of the <see cref="Mutation"/> class.
        /// </summary>
        /// <param name="labelRepo">The label repo.</param>
        /// <param name="toDoItemRepo">To do item repo.</param>
        /// <param name="toDoListRepo">To do list repo.</param>
        /// <param name="userRepo">The user repo.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="tokenConfig">The token configuration.</param>
        public Mutation([Service] ILabelRepo labelRepo, [Service] IToDoItemRepo toDoItemRepo, [Service] IToDoListRepo toDoListRepo,
            [Service] IUserRepo userRepo, IHttpContextAccessor httpContextAccessor, IOptions<JwtTokenConfig> tokenConfig)
        {
            _labelRepo = labelRepo;
            _toDoItemRepo = toDoItemRepo;
            _toDoListRepo = toDoListRepo;
            _userRepo = userRepo;
            _tokenConfig = tokenConfig.Value;
            if (httpContextAccessor.HttpContext.Items["UserId"] != null)
            {
                _userId = int.Parse(httpContextAccessor.HttpContext.Items["UserId"].ToString());
            }
        }

        #region Label Mutations

        /// <summary>
        /// Adds Label record
        /// </summary>
        /// <param name="createLabelDto">The create label dto.</param>
        /// <returns>
        /// added ToDoList record.
        /// </returns>
        public async Task<LabelDto> AddLabel(CreateLabelDto createLabelDto)
        {
            if (createLabelDto != null)
            {
                createLabelDto.CreatedBy = _userId;
            }
            LabelDto addedItem = await _labelRepo.Add(createLabelDto);
            return addedItem;
        }

        /// <summary>
        /// Delete Label record.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// 1 on successful deletion else throws argument exception.
        /// </returns>
        public async Task<int> DeleteLabel(int id)
        {
            int deletedItem = await _labelRepo.Delete(id, _userId);
            return deletedItem;
        }

        #endregion

        #region ToDoItem Mutations

        /// <summary>
        /// Adds ToDoItem record
        /// </summary>
        /// <param name="createToDoItemDto">The create to do item dto.</param>
        /// <returns>
        /// added ToDoList record.
        /// </returns>
        public async Task<ToDoItemDto> AddToDoItem(CreateToDoItemDto createToDoItemDto)
        {
            if (createToDoItemDto != null)
            {
                createToDoItemDto.CreatedBy = _userId;
            }
            ToDoItemDto addedItem = await _toDoItemRepo.Add(createToDoItemDto);
            return addedItem;
        }

        /// <summary>
        /// Update ToDoItem record
        /// </summary>
        /// <param name="updateToDoItemDto">The update to do item dto.</param>
        /// <returns>
        /// Updated record.
        /// </returns>
        public async Task<ToDoItemDto> UpdateToDoItem(UpdateToDoItemDto updateToDoItemDto)
        {
            ToDoItemDto updatedItem = await _toDoItemRepo.Update(updateToDoItemDto);
            return updatedItem;
        }

        /// <summary>
        /// Delete ToDoItem record.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// 1 on successful deletion else throws argument exception.
        /// </returns>
        public async Task<int> DeleteToDoItem(int id)
        {
            int deletedItem = await _toDoItemRepo.Delete(id, _userId);
            return deletedItem;
        }

        #endregion

        #region ToDoLItem Mutations

        /// <summary>
        /// Adds ToDoList record
        /// </summary>
        /// <param name="createToDoListDto">The create to do list dto.</param>
        /// <returns>
        /// added ToDoList record.
        /// </returns>
        public async Task<ToDoListDto> AddToDoList(CreateToDoListDto createToDoListDto)
        {
            if (createToDoListDto != null)
            {
                createToDoListDto.CreatedBy = _userId;
            }
            ToDoListDto addedItem = await _toDoListRepo.Add(createToDoListDto);
            return addedItem;
        }

        /// <summary>
        /// Update ToDoList record
        /// </summary>
        /// <param name="updateToDoListDto">The update to do list dto.</param>
        /// <returns>
        /// Updated record.
        /// </returns>
        public async Task<ToDoListDto> UpdateToDoList(UpdateToDoListDto updateToDoListDto)
        {
            ToDoListDto updatedItem = await _toDoListRepo.Update(updateToDoListDto);
            return updatedItem;
        }

        /// <summary>
        /// Delete ToDoList record.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// 1 on successful deletion else throws argument exception.
        /// </returns>
        public async Task<int> DeleteToDoList(int id)
        {
            int deletedItem = await _toDoListRepo.Delete(id, _userId);
            return deletedItem;
        }
        #endregion

        #region User Mutations
        /// <summary>
        /// Authenticate user.
        /// </summary>
        /// <param name="userName">Username.</param>
        /// <param name="password">Password.</param>
        /// <returns>
        /// Returns user id.
        /// </returns>
        [AllowAnonymous]
        public async Task<ResponseModel<string>> AuthenticateUser(string userName, string password)
        {
            UserDto userDto = await _userRepo.UserLogin(userName, password);

            if (userDto != null)
            {
                // On Successful authentication, generate jwt token.
                string token = TokenService.GenerateJwtToken(userDto, _tokenConfig);

                return new ResponseModel<string>
                {
                    IsSuccess = true,
                    Result = token,
                    Message = "Authentication successful."
                };
            }
            return null;
        }
        /// <summary>
        /// register user.
        /// </summary>
        /// <param name="userDto">The user dto.</param>
        /// <returns>
        /// success/failure result.
        /// </returns>
        [AllowAnonymous]
        public async Task<int> RegisterUser(RegisterUserDto userDto)
        {
            return await _userRepo.AddUser(userDto);
        }

        #endregion
    }
}