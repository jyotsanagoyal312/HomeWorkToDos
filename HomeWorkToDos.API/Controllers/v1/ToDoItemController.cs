using AutoMapper;
using HomeWorkToDos.Business.Contract;
using HomeWorkToDos.Util.Dtos;
using HomeWorkToDos.Util.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace HomeWorkToDos.API.Controllers.v1
{
    /// <summary>
    /// ToDoItem Controller
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/v{version:apiVersion}/todo/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize]
    public class ToDoItemController : ControllerBase
    {
        /// <summary>
        /// The item service
        /// </summary>
        private readonly IToDoItem _itemService;
        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoItemController"/> class.
        /// </summary>
        /// <param name="itemService">The item service.</param>
        /// <param name="mapper">The mapper.</param>
        public ToDoItemController(IToDoItem itemService, IMapper mapper)
        {
            this._itemService = itemService;
            this._mapper = mapper;
        }

        /// <summary>
        /// Get all todoItem for specific user.
        /// </summary>
        /// <param name="parameters">The pagination parameters.</param>
        /// <returns>Returns Action Result type based on Success or Failure.</returns>
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParameters parameters)
        {
            int userId = int.Parse(HttpContext.Items["UserId"].ToString());
            PagedList<ToDoItemDto> pagedToDoItemDto = await _itemService.GetAllByUser(parameters, userId);
            if (pagedToDoItemDto != null)
            {
                if (pagedToDoItemDto.Count > 0)
                {
                    var metadata = new
                    {
                        pagedToDoItemDto.TotalCount,
                        pagedToDoItemDto.PageSize,
                        pagedToDoItemDto.CurrentPage,
                        pagedToDoItemDto.TotalPages,
                        pagedToDoItemDto.HasNext,
                        pagedToDoItemDto.HasPrevious
                    };
                    Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                    return Ok(
                        new ResponseModel<PagedList<ToDoItemDto>>
                        {
                            IsSuccess = true,
                            Result = pagedToDoItemDto,
                            Message = "Data retrieval successful."
                        });
                }
                else
                {
                    return Ok(
                        new ResponseModel<string>
                        {
                            IsSuccess = false,
                            Result = "No ToDoItem records present.",
                            Message = " Please add ToDoItems first."
                        });
                }
            }
            return NotFound(
                new ResponseModel<string>
                {
                    IsSuccess = false,
                    Result = "No Results Found.",
                    Message = "No data exist. Please add todo items first."
                });
        }

        /// <summary>
        /// Gets the todoItem by identifier.
        /// </summary>
        /// <param name="toDoItemId">To do item identifier.</param>
        /// <returns>Returns Action Result type based on Success or Failure.</returns>
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("GetById/{toDoItemId}")]
        public async Task<IActionResult> GetById([Required] int toDoItemId)
        {
            int userId = int.Parse(HttpContext.Items["UserId"].ToString());

            ToDoItemDto ToDoItemDto = await _itemService.GetById(toDoItemId, userId);
            if (ToDoItemDto != null)
            {
                return Ok(
                    new ResponseModel<ToDoItemDto>
                    {
                        IsSuccess = true,
                        Result = ToDoItemDto,
                        Message = "Data retrieval successful."
                    });
            }
            return NotFound(
                new ResponseModel<string>
                {
                    IsSuccess = false,
                    Result = "Not found.",
                    Message = "No data exist for Id = " + toDoItemId + "."
                });
        }

        /// <summary>
        /// Adds the to do item specific to user.
        /// </summary>
        /// <param name="toDoItem">To do item.</param>
        /// <param name="version">The version.</param>
        /// <returns>
        /// Returns Action Result type based on Success or Failure.
        /// </returns>
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [HttpPost]
        public async Task<IActionResult> Add(CreateToDoItemDto toDoItem, ApiVersion version)
        {
            int userId = int.Parse(HttpContext.Items["UserId"].ToString());
            if (toDoItem == null || string.IsNullOrWhiteSpace(toDoItem.Description)
                || toDoItem.ToDoListId == 0)
            {
                return BadRequest(new ResponseModel<string>
                {
                    IsSuccess = false,
                    Result = "Not Updated.",
                    Message = "Invalid request, mandatory fields not provided in request."
                });
            }
            toDoItem.CreatedBy = userId;
            toDoItem.UserId = userId;
            ToDoItemDto createdToDoItem = await _itemService.Add(toDoItem);
            return CreatedAtAction(nameof(GetById), new { createdToDoItem.ToDoItemId, version = $"{version}" }, createdToDoItem);
        }

        /// <summary>
        /// Updates the to do item by specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="toDoItem">To do item.</param>
        /// <returns>Returns Action Result type based on Success or Failure.</returns>
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([Required] int id, UpdateToDoItemDto toDoItem)
        {
            int userId = int.Parse(HttpContext.Items["UserId"].ToString());
            if (null == toDoItem || string.IsNullOrWhiteSpace(toDoItem.Description))
            {
                return BadRequest(new ResponseModel<string>
                {
                    IsSuccess = false,
                    Result = "Not Updated.",
                    Message = "Invalid request, Mandatory fields not provided in request."
                });
            }
            toDoItem.ToDoItemId = id;
            toDoItem.UserId = userId;
            toDoItem.ModifiedBy = userId;
            ToDoItemDto updatedToDoItem = await _itemService.Update(toDoItem);
            if (updatedToDoItem != null)
            {
                return Ok(
                    new ResponseModel<ToDoItemDto>
                    {
                        IsSuccess = true,
                        Result = updatedToDoItem,
                        Message = "ToDoItem with Id = " + id + " is updated on " + updatedToDoItem.ModifiedOn + " by UserId = " + userId + "."
                    });
            }
            return NotFound(
                new ResponseModel<object>
                {
                    IsSuccess = false,
                    Result = "Failed to update.",
                    Message = "No data exist for Id = " + toDoItem.ToDoItemId
                });
        }

        /// <summary>
        /// Deletes to do item.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Returns Action Result type based on Success or Failure.</returns>
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToDoItem([Required] int id)
        {
            int userId = int.Parse(HttpContext.Items["UserId"].ToString());
            int deletedToDoItem = await _itemService.Delete(id, userId);
            if (deletedToDoItem == 1)
            {
                return Ok(
                    new ResponseModel<object>
                    {
                        IsSuccess = true,
                        Result = "Deleted",
                        Message = "ToDoItem with ID = " + id + "is deleted by UserId = " + userId + "."
                    });
            }
            return NotFound(
                new ResponseModel<string>
                {
                    IsSuccess = true,
                    Result = "Not found.",
                    Message = "No data exist for Id = " + id + "."
                });
        }

        /// <summary>
        /// Patches the specified to do item identifier.
        /// </summary>
        /// <param name="toDoItemId">To do item identifier.</param>
        /// <param name="toDoItemPatch">To do item patch.</param>
        /// <returns>Returns Action Result type based on Success or Failure.</returns>
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPatch("{toDoItemId}")]
        public async Task<IActionResult> Patch([Required] int toDoItemId, [FromBody] JsonPatchDocument<UpdateToDoItemDto> toDoItemPatch)
        {
            int userId = int.Parse(HttpContext.Items["UserId"].ToString());
            if (toDoItemPatch == null)
            {
                return BadRequest(
                    new ResponseModel<string>
                    {
                        IsSuccess = false,
                        Result = "Bad Request.",
                        Message = "Invalid request, Mandatory fields not provided in request."
                    });
            }
            ToDoItemDto existingToDoItemDto = await _itemService.GetById(toDoItemId, userId);
            if (existingToDoItemDto == null)
            {
                return NotFound(
                    new ResponseModel<string>
                    {
                        IsSuccess = false,
                        Result = "No existing record found for provided input.",
                        Message = "No data exist for Id = " + toDoItemId
                    });
            }
            var existingUpdateToDoItemDto = _mapper.Map<UpdateToDoItemDto>(existingToDoItemDto);
            toDoItemPatch.ApplyTo(existingUpdateToDoItemDto);
            bool isValid = TryValidateModel(existingToDoItemDto);
            if (!isValid)
            {
                return BadRequest(ModelState);
            }
            existingUpdateToDoItemDto.UserId = userId;
            existingUpdateToDoItemDto.ModifiedBy = userId;
            ToDoItemDto updatedToDoItemDto = await _itemService.Update(existingUpdateToDoItemDto);
            if (updatedToDoItemDto == null)
            {
                return NotFound(
                    new ResponseModel<string>
                    {
                        IsSuccess = false,
                        Result = "No existing record found for provided input.",
                        Message = "No data exist for Id = " + toDoItemId
                    });
            }
            else
            {
                return Ok(
                     new ResponseModel<ToDoItemDto>
                     {
                         IsSuccess = true,
                         Result = updatedToDoItemDto,
                         Message = "ToDoItem with Id = " + updatedToDoItemDto.ToDoItemId + " is updated on " + updatedToDoItemDto.ModifiedOn + " by UserId = " + userId + "."
                     });
            }
        }
    }
}
