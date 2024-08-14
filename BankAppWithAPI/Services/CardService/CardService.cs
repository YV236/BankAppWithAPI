using AutoMapper;
using BankAppWithAPI.Data;
using BankAppWithAPI.Dtos.Card;
using BankAppWithAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Numerics;
using System.Text;

namespace BankAppWithAPI.Services.CardService
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

        public async Task<ServiceResponse<GetCardDto>> CreateCard(AddCardDto addCardDto)
        {
            var serviceResponse = new ServiceResponse<GetCardDto>();

            if (!addCardDto.PinCode.All(char.IsDigit) || addCardDto.PinCode.Length != 4)
            {
                serviceResponse.Data = null;
                serviceResponse.IsSuccessful = false;
                serviceResponse.Message = $"PinCode {addCardDto.PinCode} in not valid. It must contain digits and contain 4 numbers";
                serviceResponse.StatusCode = HttpStatusCode.UnprocessableEntity;
                return serviceResponse;
            }

            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                serviceResponse.IsSuccessful = false;
                serviceResponse.Message = "User not found";
                serviceResponse.StatusCode = HttpStatusCode.NotFound;
                return serviceResponse;
            }

            var user = await _context.Users.Include(u => u.Card).FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                serviceResponse.IsSuccessful = false;
                serviceResponse.Message = "User not found";
                serviceResponse.StatusCode = HttpStatusCode.NotFound;
                return serviceResponse;
            }
            else if(addCardDto.PaymentSystem == null)
            {
                serviceResponse.IsSuccessful = false;
                serviceResponse.Message = "Please, choose the payment System";
                serviceResponse.StatusCode = HttpStatusCode.BadRequest;
                return serviceResponse;
            }
            else if (user.Card != null)
            {
                serviceResponse.IsSuccessful = false;
                serviceResponse.Message = "The user has a card already.";
                serviceResponse.StatusCode = HttpStatusCode.BadRequest;
                return serviceResponse;
            }

            try
            {
                var cardNumber = await GenerateUniqueCardNumber(addCardDto.PaymentSystem);

                CreatePinHash(addCardDto.PinCode, out byte[] pinHash, out byte[] pinSalt);
                CreateCVVHash(addCardDto.PinCode, out byte[] CVVHash, out byte[] CVVSalt);

                var card = new Card
                {
                    CardNumber = cardNumber,
                    PaymentSystem = addCardDto.PaymentSystem,
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
            }
            catch (Exception ex)
            {
                serviceResponse.IsSuccessful = false;
                serviceResponse.Message = ex.Message;
                serviceResponse.StatusCode = HttpStatusCode.InternalServerError;
            }

            return serviceResponse;
        }

        private async Task<string> GenerateUniqueCardNumber(PaymentSystem? paymentSystem)
        {
            string bin = ((int)paymentSystem!).ToString() + "301025";
            string cardNumber = "";
            bool isUnique;

            do
            {
                cardNumber = GenerateRandomCardNumber(bin);
                isUnique = !await _context.Cards.AnyAsync(a => a.CardNumber == cardNumber);
            }
            while (!isUnique);

            return cardNumber;
        }

        private string GenerateRandomCardNumber(string bin)
        {
            StringBuilder sb = new StringBuilder(bin);
            int length = 16;
            bool check = false;

            while (!check)
            {
                // Generating random numbers for the card until we reach the desired length minus 1 (check digit)
                Random random = new Random();
                while (sb.Length < length - 1)
                {
                    sb.Append(random.Next(0, 10));
                }

                // Adding a check digit
                sb.Append(CalculateLuhnCheckDigit(sb.ToString()));

                // Тут іде перевірка чи число відповідає алгоритму Луна
                if(ValidateLuhnCheck(sb.ToString()))
                {
                    check = true;
                }
                else
                {
                    sb = new StringBuilder(bin);
                }
            }

            return sb.ToString();
        }

        private int CalculateLuhnCheckDigit(string number)
        {
            int sum = 0;
            bool alternate = false;
            int n = 0;

            for (int i = number.Length - 1; i >= 0; i--)
            {
                n = int.Parse(number[i].ToString());

                if (alternate)
                {
                    n *= 2;
                    if (n > 9)
                        n -= 9;
                }

                sum += n;
                alternate = !alternate;
            }

            int checkDigit = (10 - (sum % 10)) % 10;
            return checkDigit;
        }

        private static bool ValidateLuhnCheck(string number)
        {
            int sum = 0;
            bool alternate = false;

            // Обчислення суми цифр
            for (int i = number.Length - 1; i >= 0; i--)
            {
                int n = int.Parse(number[i].ToString());

                if (alternate)
                {
                    n *= 2;
                    if (n > 9)
                        n -= 9;
                }

                sum += n;
                alternate = !alternate;
            }

            return sum % 10 == 0;
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
