using BankAppWithAPI.Dtos.Card;
using BankAppWithAPI.Models;

namespace BankAppWithAPI.Services.Interfaces
{
    public interface ICardService
    {
        Task<ServiceResponse<GetCardDto>> CreateCard(AddCardDto addCardDto, ClaimsPrincipal userToFind);
        Task<ServiceResponse<string>> Login(LoginCardDto loginCardDto);
    }
}