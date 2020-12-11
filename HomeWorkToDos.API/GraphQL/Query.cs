using HomeWorkToDos.DataAccess.Contract;
using HomeWorkToDos.Util.Dtos;
using HotChocolate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeWorkToDos.API.GraphQL
{
    /// <summary>
    /// Query class for GraphQl.
    /// </summary>
    [Authorize]
    public class Query
    {
        private readonly ILabelRepo _labelRepo;
        private readonly IToDoListRepo _toDoListRepo;
        private readonly IToDoItemRepo _toDoItemRepo;
        private readonly IUserRepo _userRepo;
        private readonly int _userId;

        public Query([Service] ILabelRepo labelRepo, [Service] IToDoItemRepo toDoItemRepo, [Service] IToDoListRepo toDoListRepo,
            [Service] IUserRepo userRepo, IHttpContextAccessor httpContextAccessor)
        {
            _labelRepo = labelRepo;
            _toDoItemRepo = toDoItemRepo;
            _toDoListRepo = toDoListRepo;
            _userRepo = userRepo;
            if (httpContextAccessor.HttpContext.Items["UserId"] != null)
            {
                _userId = int.Parse(httpContextAccessor.HttpContext.Items["UserId"].ToString());
            }
        }

        #region Labels
        /// <summary>
        /// Get labels.
        /// </summary>
        /// <returns>Returns labels.</returns>
        public async Task<List<LabelDto>> GetAllLabels()
        {
            return await _labelRepo.GetByUser(_userId);
        }

        /// <summary>
        /// Get label by id.
        /// </summary>
        /// <param name="labelId">label id.</param>
        public async Task<LabelDto> GetLabelById(int labelId)
        {
            return await _labelRepo.GetById(labelId, _userId);
        }

        #endregion

        #region Todolists

        /// <summary>
        /// Get ToDoItems.
        /// </summary>
        /// <returns>Returns ToDoItems.</returns>
        public async Task<List<ToDoItemDto>> GetAllToDoItems()
        {
            return await _toDoItemRepo.GetAllByUser(_userId);
        }

        /// <summary>
        /// Get ToDoItem by id.
        /// </summary>
        /// <param name="toDoItemId">ToDoItem id.</param>        
        public async Task<ToDoItemDto> GetToDoItemById(int toDoItemId)
        {
            return await _toDoItemRepo.GetById(toDoItemId, _userId);
        }

        #endregion

        #region ToDoLists

        /// <summary>
        /// Get ToDoLists.
        /// </summary>
        /// <returns>Returns ToDoLists.</returns>
        public async Task<List<ToDoListDto>> GetAllToDoLists()
        {
            return await _toDoListRepo.GetAllByUser(_userId);
        }

        /// <summary>
        /// Get ToDoList by id.
        /// </summary>
        /// <param name="toDoListId">ToDoList id.</param>
        public async Task<ToDoListDto> GetToDoListById(int toDoListId)
        {
            return await _toDoListRepo.GetById(toDoListId, _userId);
        }

        #endregion

        #region Users

        /// <summary>
        /// Get user by user id.
        /// </summary>
        /// <returns>Returns user details.</returns>
        public async Task<UserDto> GetById()
        {
            return await _userRepo.GetById(_userId);
        }

        #endregion
    }
}