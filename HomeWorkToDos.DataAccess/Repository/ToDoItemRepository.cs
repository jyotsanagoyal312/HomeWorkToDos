using AutoMapper;
using HomeWorkToDos.DataAccess.Contract;
using HomeWorkToDos.DataAccess.Models;
using HomeWorkToDos.Util.Dtos;
using HomeWorkToDos.Util.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeWorkToDos.DataAccess.Repository
{
    /// <summary>
    /// ToDoItemRepository
    /// </summary>
    /// <seealso cref="HomeWorkToDos.DataAccess.Contract.IToDoItemRepo" />
    public class ToDoItemRepository : IToDoItemRepo
    {
        /// <summary>
        /// To do item repository
        /// </summary>
        private readonly IRepository<ToDoItem> _toDoItemRepository;
        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;
        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoItemRepository"/> class.
        /// </summary>
        /// <param name="toDoItemRepository">To do item repository.</param>
        /// <param name="mapper">The mapper.</param>
        public ToDoItemRepository(IRepository<ToDoItem> toDoItemRepository, IMapper mapper)
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
            PagedList<ToDoItem> toDoItems;
            if (paginationParams != null && !string.IsNullOrWhiteSpace(paginationParams.SearchText))
                toDoItems = _toDoItemRepository.FilterList(paginationParams, x => x.UserId == userId && x.IsActive.Value && x.Description.Contains(paginationParams.SearchText), null, "Label,User,ToDoList");
            else
                toDoItems = _toDoItemRepository.FilterList(paginationParams, x => x.UserId == userId && x.IsActive.Value, null, "Label,User,ToDoList");
            
            var toDoItemDtos = _mapper.Map<List<ToDoItemDto>>(toDoItems);
            return new PagedList<ToDoItemDto>(toDoItemDtos, toDoItems.Count, toDoItems.CurrentPage, toDoItems.PageSize);
        }

        /// <summary>
        /// Gets all by user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public async Task<List<ToDoItemDto>> GetAllByUser(int userId)
        {
            List<ToDoItem> toDoItems = _toDoItemRepository.FilterList(x => x.UserId == userId && x.IsActive.Value, null, "Label,User,ToDoList").ToList();

            return _mapper.Map<List<ToDoItemDto>>(toDoItems);
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
            PagedList<ToDoItem> toDoItems;
            if (paginationParams != null && !string.IsNullOrWhiteSpace(paginationParams.SearchText))
                toDoItems = _toDoItemRepository.FilterList(paginationParams, x => x.ToDoListId == listId && x.UserId == userId && x.IsActive.Value && x.Description.Contains(paginationParams.SearchText), null, "Label, User, ToDoList");
            else
                toDoItems = _toDoItemRepository.FilterList(paginationParams, x => x.ToDoListId == listId && x.UserId == userId && x.IsActive.Value, null, "Label,User,ToDoList");

            var toDoItemDtos = _mapper.Map<List<ToDoItemDto>>(toDoItems);
            return new PagedList<ToDoItemDto>(toDoItemDtos, toDoItems.Count, toDoItems.CurrentPage, toDoItems.PageSize);
        }

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="itemId">The item identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public async Task<ToDoItemDto> GetById(int itemId, int userId)
        {
            ToDoItem toDoItem = _toDoItemRepository.FilterList(x => x.ToDoItemId == itemId && x.UserId == userId && x.IsActive.Value, null, "Label,User,ToDoList").FirstOrDefault();
            return _mapper.Map<ToDoItemDto>(toDoItem);
        }

        /// <summary>
        /// Adds the specified to do item dto.
        /// </summary>
        /// <param name="toDoItemDto">To do item dto.</param>
        /// <returns></returns>
        public async Task<ToDoItemDto> Add(CreateToDoItemDto toDoItemDto)
        {
            ToDoItem toDoItem = _mapper.Map<ToDoItem>(toDoItemDto);
            toDoItem.CreatedOn = DateTime.UtcNow;
            toDoItem.CreatedBy = toDoItem.UserId;
            toDoItem = _toDoItemRepository.Add(toDoItem);
            _toDoItemRepository.Save();
            return _mapper.Map<ToDoItemDto>(toDoItem);
        }

        /// <summary>
        /// Updates the specified to do item dto.
        /// </summary>
        /// <param name="toDoItemDto">To do item dto.</param>
        /// <returns></returns>
        public async Task<ToDoItemDto> Update(UpdateToDoItemDto toDoItemDto)
        {
            ToDoItem toDoItemDb = _toDoItemRepository.FilterList(x => x.ToDoItemId == toDoItemDto.ToDoItemId && x.UserId == toDoItemDto.UserId && x.IsActive.Value).FirstOrDefault();
            if (toDoItemDb == null)
                return null;

            ToDoItem toDoItem = _mapper.Map<ToDoItem>(toDoItemDto);
            toDoItem.CreatedOn = DateTime.UtcNow;
            toDoItem.CreatedBy = toDoItemDb.CreatedBy;
            toDoItem.ModifiedBy = toDoItem.UserId;
            toDoItem.ModifiedOn = DateTime.UtcNow;
            if (!toDoItem.IsActive.HasValue)
            {
                toDoItem.IsActive = toDoItemDb.IsActive;
            }

            _toDoItemRepository.Update(toDoItem);
            _toDoItemRepository.Save();
            return _mapper.Map<ToDoItemDto>(toDoItem);
        }

        /// <summary>
        /// Deletes the specified item identifier.
        /// </summary>
        /// <param name="itemId">The item identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public async Task<int> Delete(int itemId, int userId)
        {
            ToDoItem toDoItem = _toDoItemRepository.FilterList(x => x.ToDoItemId == itemId && x.UserId == userId && x.IsActive.Value).FirstOrDefault();
            if (toDoItem == null)
                return 0;

            toDoItem.IsActive = false;
            toDoItem.ModifiedBy = toDoItem.UserId;
            toDoItem.ModifiedOn = DateTime.UtcNow;
            _toDoItemRepository.Update(toDoItem);
            _toDoItemRepository.Save();
            return _toDoItemRepository.Save();
        }
    }
}
