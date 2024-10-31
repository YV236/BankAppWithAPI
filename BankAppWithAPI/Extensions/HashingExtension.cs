
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
    }
}
