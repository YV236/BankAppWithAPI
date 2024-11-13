using BankAppWithAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

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
            using (var pbkdf2 = new System.Security.Cryptography.Rfc2898DeriveBytes(pinCode, pinSalt, 10000))
            {
                var computedHash = pbkdf2.GetBytes(32);
                return computedHash.SequenceEqual(pinHash);
            }
        }

        public static string? CreateToken(Card card)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, card.Id.ToString()),
                new Claim(ClaimTypes.Name, card.CardNumber.ToString())
            };
            var secretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY");

            if(secretKey is null)
            {
                throw new Exception("Key token is null");
            }

            SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8
               .GetBytes(secretKey));

            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
