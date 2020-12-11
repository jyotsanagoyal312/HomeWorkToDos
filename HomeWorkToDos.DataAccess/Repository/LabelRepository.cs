using AutoMapper;
using HomeWorkToDos.DataAccess.Contract;
using HomeWorkToDos.DataAccess.Models;
using HomeWorkToDos.Util.Dtos;
using System.Threading.Tasks;
using System.Linq;
using HomeWorkToDos.Util.Models;
using System.Collections.Generic;

namespace HomeWorkToDos.DataAccess.Repository
{
    /// <summary>
    /// LabelRepository
    /// </summary>
    /// <seealso cref="HomeWorkToDos.DataAccess.Contract.ILabelRepo" />
    public class LabelRepository : ILabelRepo
    {
        /// <summary>
        /// The label repository
        /// </summary>
        private readonly IRepository<Label> _labelRepository;
        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;
        /// <summary>
        /// Initializes a new instance of the <see cref="LabelRepository"/> class.
        /// </summary>
        /// <param name="labelRepository">The label repository.</param>
        /// <param name="mapper">The mapper.</param>
        public LabelRepository(IRepository<Label> labelRepository, IMapper mapper)
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
            PagedList<Label> labels;
            if (paginationParams!= null && !string.IsNullOrWhiteSpace(paginationParams.SearchText))
                labels = _labelRepository.FilterList(paginationParams, x => x.UserId == userId && x.IsActive.Value && x.Description.Contains(paginationParams.SearchText), null, "User");
            else
                labels = _labelRepository.FilterList(paginationParams, x => x.UserId == userId && x.IsActive.Value, null, "User");

            var labelDtos = _mapper.Map<List<LabelDto>>(labels);

            return new PagedList<LabelDto>(labelDtos, labels.Count, labels.CurrentPage, labels.PageSize);
        }

        /// <summary>
        /// Gets the by user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public async Task<List<LabelDto>> GetByUser(int userId)
        {
            List<Label> labels = _labelRepository.FilterList(x => x.UserId == userId && x.IsActive.Value, null, "User").ToList();
            return  _mapper.Map<List<LabelDto>>(labels);
        }

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="labelId">The label identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public async Task<LabelDto> GetById(int labelId, int userId)
        {
            var label = _labelRepository.FilterList(x => x.LabelId == labelId && x.UserId == userId && x.IsActive.Value, null, "User").FirstOrDefault();
            return _mapper.Map<LabelDto>(label);
        }

        /// <summary>
        /// Adds the specified label dto.
        /// </summary>
        /// <param name="labelDto">The label dto.</param>
        /// <returns></returns>
        public async Task<LabelDto> Add(CreateLabelDto labelDto)
        {
            Label label = _mapper.Map<Label>(labelDto);
            label = _labelRepository.Add(label);
            _labelRepository.Save();
            return _mapper.Map<LabelDto>(label);
        }

        /// <summary>
        /// Deletes the specified label identifier.
        /// </summary>
        /// <param name="labelId">The label identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public async Task<int> Delete(int labelId, int userId)
        {
            var label = _labelRepository.FilterList(x => x.LabelId == labelId && x.UserId == userId && x.IsActive.Value).FirstOrDefault();

            if (label == null)
                return 0;

            label.IsActive = false;
            _labelRepository.Update(label);
            return _labelRepository.Save();
        }
    }
}
