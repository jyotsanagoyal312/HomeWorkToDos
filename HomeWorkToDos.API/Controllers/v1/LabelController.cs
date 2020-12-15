using AutoMapper;
using HomeWorkToDos.Business.Contract;
using HomeWorkToDos.Util.Dtos;
using HomeWorkToDos.Util.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace HomeWorkToDos.API.Controllers.v1
{
    /// <summary>
    /// Label Controller
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/v{version:apiVersion}/todo/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize]
    public class LabelController : ControllerBase
    {
        /// <summary>
        /// The label service
        /// </summary>
        private readonly ILabel _labelService;
        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="LabelController"/> class.
        /// </summary>
        /// <param name="labelService">The label service.</param>
        /// <param name="mapper">The mapper.</param>
        public LabelController(ILabel labelService, IMapper mapper)
        {
            this._labelService = labelService;
            this._mapper = mapper;
        }

        /// <summary>
        /// Gets all label.
        /// </summary>
        /// <param name="parameters">The pagination parameters.</param>
        /// <returns>Returns Action Result type based on Success or Failure.</returns>
        [ProducesResponseType(typeof(ResponseModel<string>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseModel<PagedList<LabelDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<string>), StatusCodes.Status404NotFound)]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParameters parameters)
        {
            int userId = int.Parse(HttpContext.Items["UserId"].ToString());
            PagedList<LabelDto> pagedLabel = await _labelService.GetByUser(parameters, userId);
            if (pagedLabel != null)
            {
                if (pagedLabel.Count > 0)
                {
                    var metadata = new
                    {
                        pagedLabel.TotalCount,
                        pagedLabel.PageSize,
                        pagedLabel.CurrentPage,
                        pagedLabel.TotalPages,
                        pagedLabel.HasNext,
                        pagedLabel.HasPrevious
                    };
                    Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                    return Ok(
                        new ResponseModel<PagedList<LabelDto>>
                        {
                            IsSuccess = true,
                            Result = pagedLabel,
                            Message = "Data retrieval successful."
                        });
                }
                else
                {
                    return Ok(
                        new ResponseModel<string>
                        {
                            IsSuccess = true,
                            Result = "No Label records present.",
                            Message = "Please add labels first."
                        });
                }
            }
            return NotFound(
                new ResponseModel<string>
                {
                    IsSuccess = false,
                    Result = "No Results Found.",
                    Message = "No data found."
                });
        }

        /// <summary>
        /// Gets the label by identifier.
        /// </summary>
        /// <param name="id">The label identifier.</param>
        /// <returns>Returns Action Result type based on Success or Failure.</returns>
        [ProducesResponseType(typeof(ResponseModel<string>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseModel<LabelDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<string>), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([Required] int id)
        {
            int userId = int.Parse(HttpContext.Items["UserId"].ToString());
            LabelDto labelModel = await _labelService.GetById(id, userId);
            if (labelModel != null)
            {
                return Ok(
                    new ResponseModel<LabelDto>
                    {
                        IsSuccess = true,
                        Result = labelModel,
                        Message = "Data retrieved successfully."
                    });
            }
            return NotFound(
                new ResponseModel<string>
                {
                    IsSuccess = false,
                    Result = "Not found.",
                    Message = "No data exist for Id = " + id + "."
                });
        }


        /// <summary>
        /// Adds the specified label.
        /// </summary>
        /// <param name="labelDto">The label.</param>
        /// <param name="version">The version.</param>
        /// <returns>Returns Action Result type based on Success or Failure.</returns>
        [ProducesResponseType(typeof(ResponseModel<string>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseModel<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<LabelDto>), StatusCodes.Status201Created)]
        [HttpPost]
        public async Task<IActionResult> Add(CreateLabelDto labelDto, ApiVersion version)
        {
            int userId = int.Parse(HttpContext.Items["UserId"].ToString());
            if (labelDto == null || string.IsNullOrWhiteSpace(labelDto.Description))
            {
                return BadRequest(new ResponseModel<string>
                {
                    IsSuccess = false,
                    Result = "Not Updated.",
                    Message = "Invalid request, Mandatory fields not provided in request."
                });
            }
            labelDto.CreatedBy = userId;
            labelDto.UserId = userId;
            LabelDto createdLabel = await _labelService.Add(labelDto);
            return CreatedAtAction(nameof(GetById), new { id = createdLabel.LabelId, version = $"{version}" }, createdLabel);
        }

        /// <summary>
        /// Deletes the label.
        /// </summary>
        /// <param name="id">The label identifier.</param>
        /// <returns>Returns Action Result type based on Success or Failure.</returns>
        [ProducesResponseType(typeof(ResponseModel<string>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseModel<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<string>), StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLabel([Required] int id)
        {

            int userId = int.Parse(HttpContext.Items["UserId"].ToString());
            int deletedItem = await _labelService.Delete(id, userId);
            if (deletedItem == 1)
            {
                return Ok(
                    new ResponseModel<object>
                    {
                        IsSuccess = true,
                        Result = "Deleted successful",
                        Message = "Label with ID = " + id + " is deleted by UserId = " + userId + "."
                    });
            }
            return NotFound(
                new ResponseModel<string>
                {
                    IsSuccess = false,
                    Result = "Not found.",
                    Message = "No data exist for Id = " + id
                });
        }
    }
}
