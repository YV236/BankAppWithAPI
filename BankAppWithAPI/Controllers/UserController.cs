using Azure;
using BankAppWithAPI.Services.UserServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BankAppWithAPI.Controllers
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
           // var response = _userService.GetUserByEmail("email");


            var userId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var userDto = _mapper.Map<GetUserDto>(user);
            return Ok(userDto);
        }
    }
}
