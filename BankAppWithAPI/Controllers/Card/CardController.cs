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
    [Authorize]
    public class CardController(ICardService cardService) : ControllerBase
    {
        [HttpPost("create")]
        public async Task<ActionResult<ServiceResponse<GetCardDto>>> CreateCard(AddCardDto addCardDto)
        {
            var response = await cardService.CreateCard(addCardDto, User);

            return StatusCode((int)response.StatusCode, response);
        }
    }
}
