using Azure;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using BankAppWithAPI.Dtos.User;
using Microsoft.AspNetCore.Mvc;
using BankAppWithAPI.Data;
using BankAppWithAPI.Models;

namespace BankAppWithAPI.Controllers.User
{
    [ApiController]
    [Route("[controller]")]
    public class UserController(IUserService userService, DataContext context, IMapper mapper)
        : ControllerBase
    {
        [HttpGet]
        [Route("me")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse<GetUserDto>>> GetUser()
        {
            var response = await userService.GetUserInfo(User);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegisterDto userRegisterDto)
        {
            var response = await userService.Register(userRegisterDto);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPut("UpdateInfo")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse<int>>> UpdateInfo(UpdateUserDto userUpdateDto)
        {
            var response = await userService.UpdateUserInfo(User, userUpdateDto);

            return StatusCode((int)response.StatusCode, response);
        }
    }
}
