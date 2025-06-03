using BankAppWithAPI.Models;

namespace BankAppWithAPI.Services.Interfaces
{
    public interface ILuhnNumberService
    {
        Task<string> GenerateUniqueCardNumber(PaymentSystem? paymentSystem);
        Task<string> GenerateUniqueIBAN();
    }
}
