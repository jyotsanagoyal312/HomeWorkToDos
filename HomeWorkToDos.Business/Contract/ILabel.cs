using HomeWorkToDos.Util.Dtos;
using HomeWorkToDos.Util.Models;
using System.Threading.Tasks;

namespace HomeWorkToDos.Business.Contract
{
    /// <summary>
    /// ILabel
    /// </summary>
    public interface ILabel
    {
        /// <summary>
        /// Gets the by user.
        /// </summary>
        /// <param name="paginationParams">The pagination parameters.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<PagedList<LabelDto>> GetByUser(PaginationParameters paginationParams, int userId);
        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="labelId">The label identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<LabelDto> GetById(int labelId, int userId);
        /// <summary>
        /// Adds the specified label dto.
        /// </summary>
        /// <param name="labelDto">The label dto.</param>
        /// <returns></returns>
        Task<LabelDto> Add(CreateLabelDto labelDto);
        /// <summary>
        /// Deletes the specified label identifier.
        /// </summary>
        /// <param name="labelId">The label identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<int> Delete(int labelId, int userId);
    }
}
