using AutoMapper;
using HomeWorkToDos.Business.Contract;
using HomeWorkToDos.Util.Auth;
using HomeWorkToDos.Util.ConfigModels;
using HomeWorkToDos.Util.Dtos;
using HomeWorkToDos.Util.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace HomeWorkToDos.API.Controllers
{
    /// <summary>
    /// User Controller
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/v{version:apiVersion}/todo/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class UserController : ControllerBase
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<UserController> _logger;
        /// <summary>
        /// The JWT token configuration
        /// </summary>
        private readonly JwtTokenConfig _jwtTokenConfig;
        /// <summary>
        /// The user service
        /// </summary>
        private readonly IUser _userService;
        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;
        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="jwtTokenConfig">The JWT token configuration.</param>
        /// <param name="userService">The user service.</param>
        /// <param name="mapper">The mapper.</param>
        public UserController(ILogger<UserController> logger, IOptions<JwtTokenConfig> jwtTokenConfig,  IUser userService, IMapper mapper)
        {
            this._logger = logger;
            this._mapper = mapper;
            this._userService = userService;
            this._jwtTokenConfig = jwtTokenConfig.Value;
        }

        /// <summary>
        /// Logins the user with credential and generates auth token.
        /// </summary>
        /// <param name="loginDto">The login model.</param>
        /// <returns>ApiResponse on User Login</returns>
        [ProducesResponseType(typeof(ResponseModel<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<string>), StatusCodes.Status400BadRequest)]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            _logger.LogInformation("Started : Logging In.");
            UserDto userDto = await _userService.UserLogin(loginDto.UserName, loginDto.Password);

            if (userDto != null)
            {
                // On Successful authentication, generate jwt token.
                string token = TokenService.GenerateJwtToken(userDto, _jwtTokenConfig);

                return Ok(
                    new ResponseModel<string>
                    {
                        IsSuccess = true,
                        Result = token,
                        Message = "Authentication successful."
                    });
            }
            else
            {
                return BadRequest(
                    new ResponseModel<string>
                    {
                        IsSuccess = false,
                        Result = "Authentication failed.",
                        Message = "Username or password is incorrect."
                    });
            }
        }

        /// <summary>
        /// Registers the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>ApiResponse based on success/failure.</returns>
        [ProducesResponseType(typeof(ResponseModel<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<string>), StatusCodes.Status400BadRequest)]
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterUserDto user)
        {
            _logger.LogInformation("Started : Registering User.");
            int userId = await _userService.AddUser(user);
            if (userId > 0)
            {
                return Ok(
                    new ResponseModel<string>
                    {
                        IsSuccess = true,
                        Result = "Success.",
                        Message = "User registered successfully."
                    });
            }
            else
            {
                return BadRequest(
                    new ResponseModel<string>
                    {
                        IsSuccess = false,
                        Result = "Failed.",
                        Message = "Invalid request, User already exists with this username."
                    });
            }
        }
    }
}
