using Azure;
using Microsoft.AspNetCore.Authorization;

namespace BankAppWithAPI.Controllers.User
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class UserController(IUserService userService, DataContext context, IMapper mapper)
        : ControllerBase
    {
        [HttpPost]
        [Route("me")]
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

        [HttpPut]
        [Route("AddMoreInfo")]
        public async Task<ActionResult<ServiceResponse<int>>> AddMoreInfo(UserRegisterDto userRegisterDto)
        {
            var response = await userService.AddMoreInfo(User, userRegisterDto);

            if (!response.IsSuccessful)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}
