using BankAppWithAPI.Dtos.BankAccount;
using BankAppWithAPI.Dtos.Card;
using BankAppWithAPI.Models;
using BankAppWithAPI.Services.BankAccountService;
using BankAppWithAPI.Services.CardService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankAppWithAPI.Controllers.Card
{
    [ApiController]
    [Route("[controller]")]
    public class CardController(ICardService cardService) : ControllerBase
    {
        [HttpPost("create")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse<GetCardDto>>> CreateCard(AddCardDto addCardDto)
        {
            var response = await cardService.CreateCard(addCardDto, User);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<ServiceResponse<int>>> Login(LoginCardDto loginCardDto)
        {
            var response = await cardService.Login(loginCardDto);

            return StatusCode((int)response.StatusCode, response);
        }
    }
}
