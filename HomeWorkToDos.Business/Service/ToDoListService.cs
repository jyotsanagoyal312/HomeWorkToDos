using AutoMapper;
using HomeWorkToDos.Business.Contract;
using HomeWorkToDos.DataAccess.Contract;
using HomeWorkToDos.Util.Dtos;
using HomeWorkToDos.Util.Models;
using System.Threading.Tasks;

namespace HomeWorkToDos.Business.Service
{
    /// <summary>
    /// ToDoListService
    /// </summary>
    /// <seealso cref="HomeWorkToDos.Business.Contract.IToDoList" />
    public class ToDoListService : IToDoList
    {
        /// <summary>
        /// To do list repository
        /// </summary>
        private readonly IToDoListRepo _toDoListRepository;
        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;
        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoListService"/> class.
        /// </summary>
        /// <param name="toDoListRepository">To do list repository.</param>
        /// <param name="mapper">The mapper.</param>
        public ToDoListService(IToDoListRepo toDoListRepository, IMapper mapper)
        {
            this._toDoListRepository = toDoListRepository;
            this._mapper = mapper;
        }

        /// <summary>
        /// Gets all by user.
        /// </summary>
        /// <param name="paginationParams">The pagination parameters.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public async Task<PagedList<ToDoListDto>> GetAllByUser(PaginationParameters paginationParams, int userId)
        {
            return await _toDoListRepository.GetAllByUser(paginationParams, userId);
        }

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="listId">The list identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public async Task<ToDoListDto> GetById(int listId, int userId)
        {
            return await _toDoListRepository.GetById(listId, userId);
        }

        /// <summary>
        /// Adds the specified to do list dto.
        /// </summary>
        /// <param name="toDoListDto">To do list dto.</param>
        /// <returns></returns>
        public async Task<ToDoListDto> Add(CreateToDoListDto toDoListDto)
        {
            return await _toDoListRepository.Add(toDoListDto);
        }

        /// <summary>
        /// Updates the specified to do list dto.
        /// </summary>
        /// <param name="toDoListDto">To do list dto.</param>
        /// <returns></returns>
        public async Task<ToDoListDto> Update(UpdateToDoListDto toDoListDto)
        {
            return await _toDoListRepository.Update(toDoListDto);
        }

        /// <summary>
        /// Deletes the specified list identifier.
        /// </summary>
        /// <param name="listId">The list identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public async Task<int> Delete(long listId, long userId)
        {
            return await _toDoListRepository.Delete(listId, userId);
        }
    }
}
