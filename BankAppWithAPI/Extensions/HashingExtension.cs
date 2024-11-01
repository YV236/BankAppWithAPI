

using BankAppWithAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace BankAppWithAPI.Extensions
{
    public static class HashingExtension
    {
        public static void CreateHash(string pinCode, out byte[] Hash, out byte[] Salt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                Salt = hmac.Key;
                using (var pbkdf2 = new System.Security.Cryptography.Rfc2898DeriveBytes(pinCode, Salt, 10000))
                {
                    Hash = pbkdf2.GetBytes(32);
                }
            }
        }

        public static bool VerifyPasswordHash(string pinCode, byte[] pinHash, byte[] pinSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(pinSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(pinCode));
                return computedHash.SequenceEqual(pinHash);
            }
        }
    }
}
