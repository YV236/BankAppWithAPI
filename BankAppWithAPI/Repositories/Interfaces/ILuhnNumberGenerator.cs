using BankAppWithAPI.Models;

namespace BankAppWithAPI.Repositories.Interfaces
{
    public interface ILuhnNumberGenerator
    {
        Task<string> GenerateUniqueCardNumber(PaymentSystem? paymentSystem);
        Task<string> GenerateUniqueIBAN();
    }
}
