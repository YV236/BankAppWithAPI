using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BankAppWithAPI.Data;

namespace BankAppWithAPI.Controllers.User
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<Models.User> _userManager;
        private readonly SignInManager<Models.User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthController(UserManager<Models.User> userManager, SignInManager<Models.User> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(Dtos.User.UserRegisterDto registerDto)
        {
            var user = new Models.User
            {
                UserName = registerDto.Email,
                Email = registerDto.Email,
                UserFirstName = registerDto.UserFirstName,
                UserLastName = registerDto.UserLastName,
                Address = $"{registerDto.Street} {registerDto.HomeNumber} {registerDto.City} {registerDto.Country}",
                PhoneNumber = registerDto.PhoneNumber,
                DateOfCreation = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            // Assign a default role
            //await _userManager.AddToRoleAsync(user, "User");

            return Ok(new { Message = "User registered successfully" });
        }
    }

}
