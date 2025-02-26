using BankAppWithAPI.Models;
using BankAppWithAPI.Repositories.Interfaces;

namespace BankAppWithAPI.Repositories.Implementations
{
    public class LuhnNumberGenerator : ILuhnNumberGenerator
    {
        public Task<string> GenerateUniqueCardNumber(PaymentSystem? paymentSystem)
        {
            throw new NotImplementedException();
        }

        public Task<string> GenerateUniqueIBAN()
        {
            throw new NotImplementedException();
        }
    }
}
