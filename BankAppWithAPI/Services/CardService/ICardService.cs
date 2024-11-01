using BankAppWithAPI.Dtos.Card;
using BankAppWithAPI.Models;

namespace BankAppWithAPI.Services.CardService
{
    public interface ICardService
    {
        Task<ServiceResponse<GetCardDto>> CreateCard(AddCardDto addCardDto, ClaimsPrincipal userToFind);
        Task<ServiceResponse<GetCardDto>> Login(LoginCardDto loginCardDto);
    }
}