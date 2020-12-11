using HomeWorkToDos.Util.Dtos;
using HomeWorkToDos.Util.Models;
using System.Threading.Tasks;

namespace HomeWorkToDos.Business.Contract
{
    /// <summary>
    /// IToDoList
    /// </summary>
    public interface IToDoList
    {
        /// <summary>
        /// Gets all by user.
        /// </summary>
        /// <param name="paginationParams">The pagination parameters.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<PagedList<ToDoListDto>> GetAllByUser(PaginationParameters paginationParams, int userId);
        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="listId">The list identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<ToDoListDto> GetById(int listId, int userId);
        /// <summary>
        /// Adds the specified to do list dto.
        /// </summary>
        /// <param name="toDoListDto">To do list dto.</param>
        /// <returns></returns>
        Task<ToDoListDto> Add(CreateToDoListDto toDoListDto);
        /// <summary>
        /// Updates the specified to do list dto.
        /// </summary>
        /// <param name="toDoListDto">To do list dto.</param>
        /// <returns></returns>
        Task<ToDoListDto> Update(UpdateToDoListDto toDoListDto);
        /// <summary>
        /// Deletes the specified list identifier.
        /// </summary>
        /// <param name="listId">The list identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<int> Delete(long listId, long userId);
    }
}
