using BankAppWithAPI.Dtos.Card;
using BankAppWithAPI.Models;

namespace BankAppWithAPI.Services.Card
{
    public interface ICardService
    {
        Task<ServiceResponse<GetCardDto>> CreateCard(string pinCode);
    }
}
