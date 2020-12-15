using AutoMapper;
using HomeWorkToDos.Business.Contract;
using HomeWorkToDos.Util.Dtos;
using System.Threading.Tasks;
using HomeWorkToDos.DataAccess.Contract;
using HomeWorkToDos.Util.Models;

namespace HomeWorkToDos.Business.Service
{
    /// <summary>
    /// LabelService
    /// </summary>
    /// <seealso cref="HomeWorkToDos.Business.Contract.ILabel" />
    public class LabelService : ILabel
    {
        /// <summary>
        /// The label repository
        /// </summary>
        private readonly ILabelRepo _labelRepository;
        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;
        /// <summary>
        /// Initializes a new instance of the <see cref="LabelService"/> class.
        /// </summary>
        /// <param name="labelRepository">The label repository.</param>
        /// <param name="mapper">The mapper.</param>
        public LabelService(ILabelRepo labelRepository, IMapper mapper)
        {
            this._labelRepository = labelRepository;
            this._mapper = mapper;
        }

        /// <summary>
        /// Gets the by user.
        /// </summary>
        /// <param name="paginationParams">The pagination parameters.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public async Task<PagedList<LabelDto>> GetByUser(PaginationParameters paginationParams, int userId)
        {
            return await _labelRepository.GetByUser(paginationParams, userId);
        }

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="labelId">The label identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public async Task<LabelDto> GetById(int labelId, int userId)
        {
            return await _labelRepository.GetById(labelId, userId);
        }

        /// <summary>
        /// Adds the specified label dto.
        /// </summary>
        /// <param name="labelDto">The label dto.</param>
        /// <returns></returns>
        public async Task<LabelDto> Add(CreateLabelDto labelDto)
        {
            return await _labelRepository.Add(labelDto);
        }

        /// <summary>
        /// Deletes the specified label identifier.
        /// </summary>
        /// <param name="labelId">The label identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public async Task<int> Delete(int labelId, int userId)
        {
            return await _labelRepository.Delete(labelId, userId);
        }
    }
}
