using BankAppWithAPI.Models;

namespace BankAppWithAPI.Repositories.Interfaces
{
    public interface IHashingRepository
    {
        void CreateHash(string pinCode, out byte[] Hash, out byte[] Salt);
        bool VerifyPasswordHash(string pinCode, byte[] pinHash, byte[] pinSalt);
        string? CreateToken(Card card);
    }
}
