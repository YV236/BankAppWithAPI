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


            //var userId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            //var user = await _context.Users.FindAsync(userId);

            if (!response.IsSuccessful)
            {
                return NotFound(response);
            }

            //var userDto = _mapper.Map<GetUserDto>(user);
            return Ok(response);
        }

        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegisterDto userRegisterDto)
        {
            var response = await userService.Register(userRegisterDto);

            if (!response.IsSuccessful)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}
