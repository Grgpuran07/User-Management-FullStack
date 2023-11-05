using Backend.Core.Constants;
using Backend.Core.Dtos.Auth;
using Backend.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        //Route->Seed Roles to DB
        [HttpPost]
        [Route("seed-roles")]
        public async Task<IActionResult> SeedRoles()
        { 
            var seedResult = await _authService.SeedRolesAsync();
            return StatusCode(seedResult.StatusCode,seedResult.Message);
        }

        //Route -> Register
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        { 
           var registerResult = await _authService.RegisterAsync(registerDto);
           return StatusCode(registerResult.StatusCode, registerResult.Message);
        }

        //Route -> Login
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<LoginServiceResponseDto>> Login([FromBody] LoginDto loginDto)
        {
            var loginResult = await _authService.LoginAsync(loginDto);
            if (loginResult is null)
                return Unauthorized("Your credentials are invalid. Please contact admin.");
            return Ok(loginResult);
        }

        //Route - update user role
        //An owner can change everything
        //An admin can only change manager to user and vice-versa
        //Manager and user don't have access to this 
        [HttpPost]
        [Route("update-role")]
        [Authorize(Roles = StaticUserRoles.OwnerAdmin)]
        public async Task<IActionResult> UpdateRole([FromBody] UpdateRoleDto updateRoleDto)
        {
            var updateResult = await _authService.UpdateRoleAsync(User,updateRoleDto);
            if (updateResult.IsSucceed)
            {
                return Ok(updateResult.Message);
            }
            else
            {
                return StatusCode(updateResult.StatusCode,updateResult.Message);
            }
        }

        //Route -> Getting data of a user from its token
        [HttpPost]
        [Route("me")]
        public async Task<ActionResult<LoginServiceResponseDto>> Me([FromBody] MeDto token)
        {
            try
            {
                var me = await _authService.MeAsync(token);
                if (me is not null)
                {
                    return Ok(me);
                }
                else 
                {
                    return Unauthorized("Invalid Token.");
                }
            }
            catch (Exception ex) 
            {
                return Unauthorized("Invalid Token.");
            }
        }

        //Route -> List of all users with details
        [HttpGet]
        [Route("users")]

        public async Task<ActionResult<IEnumerable<UserInfoResult>>> GetUsersList() 
        { 
            var usersList = await _authService.GetUserListAsync();
            return Ok(usersList);
        }

        //Route -> Get a user by username
        [HttpGet]
        [Route("users/{userName}")]
        public async Task<ActionResult<UserInfoResult>> GetUserDetailsByUserName([FromRoute] string userName)
        {
            var user = await _authService.GetUserDetailsByUserNameAsync(userName);

            if (user is not null)
            {
                return Ok(user);
            }
            else
            {
               return NotFound("UserName not found.");
            }
        }

        //Route -> get list of all usernames
        [HttpGet]
        [Route("usernames")]

        public async Task<ActionResult<IEnumerable<string>>> GetUsersNameList()
        { 
          var userNames = await _authService.GetUsersNameListAsync();
          return Ok(userNames);
        }
    }
}
