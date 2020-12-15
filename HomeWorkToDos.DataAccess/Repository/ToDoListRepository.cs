using AutoMapper;
using HomeWorkToDos.DataAccess.Contract;
using HomeWorkToDos.DataAccess.Models;
using HomeWorkToDos.Util.Dtos;
using HomeWorkToDos.Util.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeWorkToDos.DataAccess.Repository
{
    /// <summary>
    /// ToDoListRepository
    /// </summary>
    /// <seealso cref="HomeWorkToDos.DataAccess.Contract.IToDoListRepo" />
    public class ToDoListRepository : IToDoListRepo
    {
        /// <summary>
        /// To do list repository
        /// </summary>
        private readonly IRepository<ToDoList> _toDoListRepository;
        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;
        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoListRepository"/> class.
        /// </summary>
        /// <param name="toDoListRepository">To do list repository.</param>
        /// <param name="mapper">The mapper.</param>
        public ToDoListRepository(IRepository<ToDoList> toDoListRepository, IMapper mapper)
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
            PagedList<ToDoList> toDoLists;
            if (paginationParams != null && !string.IsNullOrWhiteSpace(paginationParams.SearchText))
                toDoLists = await _toDoListRepository.FilterList(paginationParams, x => x.UserId == userId && x.IsActive.Value && x.Description.Contains(paginationParams.SearchText), null, "Label,User,ToDoItem");
            else
                toDoLists = await _toDoListRepository.FilterList(paginationParams, x => x.UserId == userId && x.IsActive.Value, null, "Label,User,ToDoItem");
            
            var toDoListDtos =  _mapper.Map<List<ToDoListDto>>(toDoLists);
            return new PagedList<ToDoListDto>(toDoListDtos, toDoLists.Count, toDoLists.CurrentPage, toDoLists.PageSize);
        }

        /// <summary>
        /// Gets all by user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public async Task<List<ToDoListDto>> GetAllByUser(int userId)
        {
            List<ToDoList> toDoLists = await _toDoListRepository.FilterList(x => x.UserId == userId && x.IsActive.Value, null, "Label,User,ToDoItem").ToListAsync();

            return _mapper.Map<List<ToDoListDto>>(toDoLists);
        }

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="listId">The list identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public async Task<ToDoListDto> GetById(int listId, int userId)
        {
            ToDoList toDoList = await _toDoListRepository.FilterList(x => x.ToDoListId == listId && x.UserId == userId && x.IsActive.Value, null, "Label,User,ToDoItem").FirstOrDefaultAsync();
            return _mapper.Map<ToDoListDto>(toDoList);
        }

        /// <summary>
        /// Adds the specified to do list dto.
        /// </summary>
        /// <param name="toDoListDto">To do list dto.</param>
        /// <returns></returns>
        public async Task<ToDoListDto> Add(CreateToDoListDto toDoListDto)
        {
            ToDoList toDoList = _mapper.Map<ToDoList>(toDoListDto);
            toDoList.CreatedOn = DateTime.UtcNow;
            toDoList.CreatedBy = toDoListDto.UserId;
            toDoList = _toDoListRepository.Add(toDoList);
            await _toDoListRepository.Save();
            return _mapper.Map<ToDoListDto>(toDoList);
        }

        /// <summary>
        /// Updates the specified to do list dto.
        /// </summary>
        /// <param name="toDoListDto">To do list dto.</param>
        /// <returns></returns>
        public async Task<ToDoListDto> Update(UpdateToDoListDto toDoListDto)
        {
            ToDoList toDoListDb = await _toDoListRepository.FilterList(x => x.ToDoListId == toDoListDto.ToDoListId && x.UserId == toDoListDto.UserId && x.IsActive.Value).FirstOrDefaultAsync();
            if (toDoListDb == null)
                return null;

            ToDoList toDoList = _mapper.Map<ToDoList>(toDoListDto);
            toDoList.CreatedBy = toDoListDb.CreatedBy;
            toDoList.CreatedOn = toDoListDb.CreatedOn;
            toDoList.ModifiedBy = toDoListDto.UserId;
            toDoList.ModifiedOn = DateTime.UtcNow;
            if (!toDoList.IsActive.HasValue)
            {
                toDoList.IsActive = toDoListDb.IsActive;
            }

            _toDoListRepository.Update(toDoList);
            await _toDoListRepository.Save();
            return _mapper.Map<ToDoListDto>(toDoList);
        }

        /// <summary>
        /// Deletes the specified list identifier.
        /// </summary>
        /// <param name="listId">The list identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public async Task<int> Delete(long listId, long userId)
        {
            ToDoList toDoList = await _toDoListRepository.FilterList(x => x.ToDoListId == listId && x.UserId == userId && x.IsActive.Value).FirstOrDefaultAsync();
            if (toDoList == null)
                return 0;

            toDoList.IsActive = false;
            toDoList.ModifiedBy = toDoList.UserId;
            toDoList.ModifiedOn = DateTime.UtcNow;
            _toDoListRepository.Update(toDoList);
            return await _toDoListRepository.Save();
        }
    }
}
