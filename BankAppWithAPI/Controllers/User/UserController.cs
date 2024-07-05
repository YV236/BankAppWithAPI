using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankAppWithAPI.Controllers.User
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, DataContext context, IMapper mapper)
        {
            _userService = userService;

            _context = context;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("me")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse<GetUserDto>>> GetUser()
        {
            var response = await _userService.GetUserInfo(User);


            //var userId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            //var user = await _context.Users.FindAsync(userId);

            if (!response.IsSuccessful)
            {
                return NotFound(response);
            }

            //var userDto = _mapper.Map<GetUserDto>(user);
            return Ok(response);
        }
    }
}
