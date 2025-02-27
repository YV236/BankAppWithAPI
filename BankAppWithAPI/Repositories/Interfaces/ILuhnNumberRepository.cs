using BankAppWithAPI.Models;

namespace BankAppWithAPI.Repositories.Interfaces
{
    public interface ILuhnNumberRepository
    {
        Task<string> GenerateUniqueCardNumber(PaymentSystem? paymentSystem);
        Task<string> GenerateUniqueIBAN();
    }
}
