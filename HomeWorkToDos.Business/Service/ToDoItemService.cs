using AutoMapper;
using HomeWorkToDos.Business.Contract;
using HomeWorkToDos.DataAccess.Contract;
using HomeWorkToDos.Util.Dtos;
using HomeWorkToDos.Util.Models;
using System.Threading.Tasks;

namespace HomeWorkToDos.Business.Service
{
    /// <summary>
    /// ToDoItemService
    /// </summary>
    /// <seealso cref="HomeWorkToDos.Business.Contract.IToDoItem" />
    public class ToDoItemService : IToDoItem
    {
        /// <summary>
        /// To do item repository
        /// </summary>
        private readonly IToDoItemRepo _toDoItemRepository;
        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;
        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoItemService"/> class.
        /// </summary>
        /// <param name="toDoItemRepository">To do item repository.</param>
        /// <param name="mapper">The mapper.</param>
        public ToDoItemService(IToDoItemRepo toDoItemRepository, IMapper mapper)
        {
            this._toDoItemRepository = toDoItemRepository;
            this._mapper = mapper;
        }

        /// <summary>
        /// Gets all by user.
        /// </summary>
        /// <param name="paginationParams">The pagination parameters.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public async Task<PagedList<ToDoItemDto>> GetAllByUser(PaginationParameters paginationParams, int userId)
        {
            return await _toDoItemRepository.GetAllByUser(paginationParams, userId);
        }

        /// <summary>
        /// Gets the by list and user.
        /// </summary>
        /// <param name="paginationParams">The pagination parameters.</param>
        /// <param name="listId">The list identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public async Task<PagedList<ToDoItemDto>> GetByListAndUser(PaginationParameters paginationParams, int listId, int userId)
        {
            return await _toDoItemRepository.GetByListAndUser(paginationParams, listId, userId);
        }

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="itemId">The item identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public async Task<ToDoItemDto> GetById(int itemId, int userId)
        {
            return await _toDoItemRepository.GetById(itemId, userId);
        }

        /// <summary>
        /// Adds the specified to do item dto.
        /// </summary>
        /// <param name="toDoItemDto">To do item dto.</param>
        /// <returns></returns>
        public async Task<ToDoItemDto> Add(CreateToDoItemDto toDoItemDto)
        {
            return await _toDoItemRepository.Add(toDoItemDto);
        }

        /// <summary>
        /// Updates the specified to do item dto.
        /// </summary>
        /// <param name="toDoItemDto">To do item dto.</param>
        /// <returns></returns>
        public async Task<ToDoItemDto> Update(UpdateToDoItemDto toDoItemDto)
        {
            return await _toDoItemRepository.Update(toDoItemDto);
        }

        /// <summary>
        /// Deletes the specified item identifier.
        /// </summary>
        /// <param name="itemId">The item identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public async Task<int> Delete(int itemId, int userId)
        {
            return await _toDoItemRepository.Delete(itemId, userId);
        }
    }
}
