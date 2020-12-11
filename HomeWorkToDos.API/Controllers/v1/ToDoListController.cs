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
    /// ToDoList Controller
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/v{version:apiVersion}/todo/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize]
    public class ToDoListController : ControllerBase
    {
        /// <summary>
        /// The list service
        /// </summary>
        private readonly IToDoList _listService;
        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoListController"/> class.
        /// </summary>
        /// <param name="listService">The list service.</param>
        /// <param name="mapper">The mapper.</param>
        public ToDoListController(IToDoList listService, IMapper mapper)
        {
            this._listService = listService;
            this._mapper = mapper;
        }

        /// <summary>
        /// Gets all to do list by specific user.
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
            PagedList<ToDoListDto> pagedToDoListDto = await _listService.GetAllByUser(parameters, userId);
            if (pagedToDoListDto != null)
            {
                if (pagedToDoListDto.Count > 0)
                {
                    var metadata = new
                    {
                        pagedToDoListDto.TotalCount,
                        pagedToDoListDto.PageSize,
                        pagedToDoListDto.CurrentPage,
                        pagedToDoListDto.TotalPages,
                        pagedToDoListDto.HasNext,
                        pagedToDoListDto.HasPrevious
                    };
                    Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                    return Ok(
                        new ResponseModel<PagedList<ToDoListDto>>
                        {
                            IsSuccess = true,
                            Result = pagedToDoListDto,
                            Message = "Data retrieval successful."
                        });
                }
                else
                {
                    return Ok(
                        new ResponseModel<string>
                        {
                            IsSuccess = false,
                            Result = "No ToDoList records present.",
                            Message = " Please add ToDoLists first."
                        });
                }
            }
            return NotFound(
                new ResponseModel<string>
                {
                    IsSuccess = false,
                    Result = "No Results Found.",
                    Message = "Please add items to list first."
                });
        }

        /// <summary>
        /// Gets the to do list by identifier.
        /// </summary>
        /// <param name="toDoListId">To do list identifier.</param>
        /// <returns>Returns Action Result type based on Success or Failure.</returns>
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("GetById/{toDoListId}")]
        public async Task<IActionResult> GetById([Required] int toDoListId)
        {
            int userId = int.Parse(HttpContext.Items["UserId"].ToString());
            ToDoListDto toDoListDto = await _listService.GetById(toDoListId, userId);
            if (toDoListDto != null)
            {
                return Ok(
                    new ResponseModel<ToDoListDto>
                    {
                        IsSuccess = true,
                        Result = toDoListDto,
                        Message = "Data retrieval successful."
                    });
            }
            return NotFound(
                new ResponseModel<string>
                {
                    IsSuccess = false,
                    Result = "Not found.",
                    Message = "No data exist for Id = " + toDoListId + "."
                });
        }

        /// <summary>
        /// Adds the specified to do list.
        /// </summary>
        /// <param name="createToDoList">The create to do list.</param>
        /// <param name="version">The version.</param>
        /// <returns>
        /// Returns Action Result type based on Success or Failure.
        /// </returns>
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [HttpPost]
        public async Task<IActionResult> Add(CreateToDoListDto createToDoList, ApiVersion version)
        {
            int userId = int.Parse(HttpContext.Items["UserId"].ToString());
            if (createToDoList == null || string.IsNullOrWhiteSpace(createToDoList.Description))
            {
                return BadRequest(new ResponseModel<string>
                {
                    IsSuccess = false,
                    Result = "Not Updated.",
                    Message = "Invalid request, mandatory fields not provided in request."
                });
            }
            createToDoList.CreatedBy = userId;
            createToDoList.UserId = userId;
            ToDoListDto createdToDoList = await _listService.Add(createToDoList);
            return CreatedAtAction(nameof(GetById), new { createdToDoList.ToDoListId, version = $"{version}" }, createdToDoList);
        }

        /// <summary>
        /// Updates the to do list by specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="listToUpdate">The list to update.</param>
        /// <returns>Returns Action Result type based on Success or Failure.</returns>
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([Required] int id, UpdateToDoListDto listToUpdate)
        {
            int userId = int.Parse(HttpContext.Items["UserId"].ToString());
            if (null == listToUpdate || string.IsNullOrWhiteSpace(listToUpdate.Description))
            {
                return BadRequest(new ResponseModel<string>
                {
                    IsSuccess = false,
                    Result = "Not Updated.",
                    Message = "Invalid request, Mandatory fields not provided in request."
                });
            }
            listToUpdate.UserId = userId;
            listToUpdate.ModifiedBy = userId;
            ToDoListDto updatedToDoListDto = await _listService.Update(listToUpdate);

            if (updatedToDoListDto != null)
            {
                return Ok(
                    new ResponseModel<ToDoListDto>
                    {
                        IsSuccess = true,
                        Result = updatedToDoListDto,
                        Message = "ToDoList with Id = " + updatedToDoListDto.ToDoListId + " is updated on " + updatedToDoListDto.ModifiedOn + " by UserId = " + userId + "."
                    });
            }
            return NotFound(
                new ResponseModel<object>
                {
                    IsSuccess = false,
                    Result = "Item to be updated not found.",
                    Message = "No data exist for ToDoListId = " + listToUpdate.ToDoListId
                });
        }

        /// <summary>
        /// Deletes the to do list by specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Returns Action Result type based on Success or Failure.</returns>
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([Required] int id)
        {
            int userId = int.Parse(HttpContext.Items["UserId"].ToString());
            int deletedItem = await _listService.Delete(id, userId);
            if (deletedItem == 1)
            {
                return Ok(
                    new ResponseModel<object>
                    {
                        IsSuccess = true,
                        Result = "Deleted",
                        Message = "ToDoList with ID = " + id + "is deleted by UserId = " + userId + "."
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
        /// Patches to do list.
        /// </summary>
        /// <param name="toDoListId">To do list identifier.</param>
        /// <param name="listToUpdatePatchDoc">The list to update patch document.</param>
        /// <returns>Returns Action Result type based on Success or Failure.</returns>
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPatch("{toDoListId}")]
        public async Task<IActionResult> PatchToDoList([Required] int toDoListId, [FromBody] JsonPatchDocument<UpdateToDoListDto> listToUpdatePatchDoc)
        {
            int userId = int.Parse(HttpContext.Items["UserId"].ToString());
            if (listToUpdatePatchDoc == null)
            {
                return BadRequest(
                    new ResponseModel<string>
                    {
                        IsSuccess = false,
                        Result = "Bad Request.",
                        Message = "Please try again with correct input."
                    });
            }
            ToDoListDto existingToDoListDto = await _listService.GetById(toDoListId, userId);
            if (existingToDoListDto == null)
            {
                return NotFound(
                    new ResponseModel<string>
                    {
                        IsSuccess = false,
                        Result = "No existing record found for provided input.",
                        Message = "No data exist for Id = " + toDoListId
                    });
            }
            var existingUpdateToDoListDto = _mapper.Map<UpdateToDoListDto>(existingToDoListDto);
            listToUpdatePatchDoc.ApplyTo(existingUpdateToDoListDto);
            bool isValid = TryValidateModel(existingUpdateToDoListDto);
            if (!isValid)
            {
                return BadRequest(ModelState);
            }
            existingUpdateToDoListDto.UserId = userId;
            existingUpdateToDoListDto.ModifiedBy = userId;
            ToDoListDto updatedToDoListDto = await _listService.Update(existingUpdateToDoListDto);

            if (updatedToDoListDto == null)
            {
                return NotFound(
                    new ResponseModel<string>
                    {
                        IsSuccess = false,
                        Result = "No existing record found for provided input.",
                        Message = "No data exist for Id = " + toDoListId
                    });
            }
            else
            {
                return Ok(
                    new ResponseModel<ToDoListDto>
                    {
                        IsSuccess = true,
                        Result = updatedToDoListDto,
                        Message = "ToDoList record with id =" + updatedToDoListDto.ToDoListId + " is updated on " + updatedToDoListDto.ModifiedOn + " by UserId = " + userId
                    });
            }
        }
    }
}