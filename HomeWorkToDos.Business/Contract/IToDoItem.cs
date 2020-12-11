using HomeWorkToDos.Util.Dtos;
using HomeWorkToDos.Util.Models;
using System.Threading.Tasks;

namespace HomeWorkToDos.Business.Contract
{
    /// <summary>
    /// IToDoItem
    /// </summary>
    public interface IToDoItem
    {
        /// <summary>
        /// Gets all by user.
        /// </summary>
        /// <param name="paginationParams">The pagination parameters.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<PagedList<ToDoItemDto>> GetAllByUser(PaginationParameters paginationParams, int userId);
        /// <summary>
        /// Gets the by list and user.
        /// </summary>
        /// <param name="paginationParams">The pagination parameters.</param>
        /// <param name="listId">The list identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<PagedList<ToDoItemDto>> GetByListAndUser(PaginationParameters paginationParams, int listId, int userId);
        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="itemId">The item identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<ToDoItemDto> GetById(int itemId, int userId);
        /// <summary>
        /// Adds the specified to do item dto.
        /// </summary>
        /// <param name="toDoItemDto">To do item dto.</param>
        /// <returns></returns>
        Task<ToDoItemDto> Add(CreateToDoItemDto toDoItemDto);
        /// <summary>
        /// Updates the specified to do item dto.
        /// </summary>
        /// <param name="toDoItemDto">To do item dto.</param>
        /// <returns></returns>
        Task<ToDoItemDto> Update(UpdateToDoItemDto toDoItemDto);
        /// <summary>
        /// Deletes the specified item identifier.
        /// </summary>
        /// <param name="itemId">The item identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<int> Delete(int itemId, int userId);
    }
}
