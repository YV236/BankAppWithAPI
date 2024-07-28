using AutoMapper;
using BankAppWithAPI.Data;
using BankAppWithAPI.Dtos.Card;
using BankAppWithAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BankAppWithAPI.Services.Card
{
    public class CardService : ICardService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public CardService(DataContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<GetCardDto>> CreateCard(string pinCode)
        {
            var serviceResponse = new ServiceResponse<GetCardDto>();

            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                serviceResponse.IsSuccessful = false;
                serviceResponse.Message = "User not found";
                return serviceResponse;
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                serviceResponse.IsSuccessful = false;
                serviceResponse.Message = "User not found";
                return serviceResponse;
            }

            if (user.Card != null)
            {
                serviceResponse.IsSuccessful = false;
                serviceResponse.Message = "User already has a card";
                return serviceResponse;
            }

            var cardNumber = await GenerateUniqueCardNumber();

            CreatePinHash(pinCode, out byte[] pinHash, out byte[] pinSalt);
            CreateCVVHash(pinCode, out byte[] CVVHash, out byte[] CVVSalt);

            var card = new Models.Card
            {
                CardNumber = cardNumber,
                PinHash = pinHash,
                PinSalt = pinSalt,
                CVVHash = CVVHash,
                CVVSalt = CVVSalt,
                ExpiryDate = DateTime.UtcNow.AddYears(10),
                User = user
            };

            _context.Cards.Add(card);
            await _context.SaveChangesAsync();

            serviceResponse.Data = _mapper.Map<GetCardDto>(card);
            serviceResponse.IsSuccessful = true;
            serviceResponse.Message = "Card created successfully";
            return serviceResponse;
        }

        private async Task<string> GenerateUniqueCardNumber()
        {
            string cardNumber;
            bool isUnique;

            do
            {
                cardNumber = GenerateRandomCardNumber();
                isUnique = !await _context.Cards.AnyAsync(a => a.CardNumber == cardNumber);
            }
            while (!isUnique);

            return cardNumber;
        }

        private string GenerateRandomCardNumber()
        {
            var random = new Random();
            return random.Next(10000000, 99999999).ToString("D10");
        }

        private void CreatePinHash(string pinCode, out byte[] pinHash, out byte[] pinSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                pinSalt = hmac.Key;
                using (var pbkdf2 = new System.Security.Cryptography.Rfc2898DeriveBytes(pinCode, pinSalt, 10000))
                {
                    pinHash = pbkdf2.GetBytes(32);
                }
            }
        }

        private void CreateCVVHash(string pinCode, out byte[] CVVHash, out byte[] CVVSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                CVVSalt = hmac.Key;
                using (var pbkdf2 = new System.Security.Cryptography.Rfc2898DeriveBytes(pinCode, CVVSalt, 10000))
                {
                    CVVHash = pbkdf2.GetBytes(32);
                }
            }
        }
    }

}
