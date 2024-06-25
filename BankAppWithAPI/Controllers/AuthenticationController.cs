using Azure;
using Microsoft.AspNetCore.Mvc;

namespace BankAppWithAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        public AuthenticationController()
        {
        }

        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegisterDto request)
        {
            int response = 0;


            return Ok(response);
        }
    }
}
