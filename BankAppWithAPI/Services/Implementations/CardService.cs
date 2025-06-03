using AutoMapper;
using BankAppWithAPI.Data;
using BankAppWithAPI.Dtos.Card;
using BankAppWithAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Numerics;
using System.Text;
using BankAppWithAPI.Extensions;
using BankAppWithAPI.Services.Interfaces;

namespace BankAppWithAPI.Services.Implementations
{
    public class CardService(DataContext _context, IMapper _mapper, ILuhnNumberService _luhnRepository) : ICardService
    {
        public async Task<ServiceResponse<string>> Login(LoginCardDto loginCardDto)
        {
            var serviceResponse = new ServiceResponse<string>();

            try
            {
                var card = await _context.Cards.FirstOrDefaultAsync(u => u.CardNumber == loginCardDto.CardNumber);

                if (card == null)
                    return serviceResponse.CreateErrorResponse(null!, $"Sorry, but the card with this" +
                        $" '{loginCardDto.CardNumber}' number no longer exists.", HttpStatusCode.NotFound);

                if (card.ExpiryDate < DateTime.UtcNow)
                    return serviceResponse.CreateErrorResponse(null!, $"Sorry, but your card '{loginCardDto.CardNumber}' is expired." +
                        $"Your card expired in '{card.ExpiryDate}' but now is '{DateTime.UtcNow}'", HttpStatusCode.Gone);

                if (HashingExtension.VerifyPasswordHash(loginCardDto.PinCode, card.PinHash, card.PinSalt) == false)
                    return serviceResponse.CreateErrorResponse(null!, $"Invalid pin code", HttpStatusCode.BadRequest);

                serviceResponse.Data = HashingExtension.CreateToken(card);
                serviceResponse.IsSuccessful = true;
                serviceResponse.Message = "Card logged in successfully";
            }
            catch (Exception ex)
            {
                serviceResponse.CreateErrorResponse(null!, ex.Message, HttpStatusCode.InternalServerError);
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCardDto>> CreateCard(AddCardDto addCardDto, ClaimsPrincipal userToFind)
        {
            var serviceResponse = new ServiceResponse<GetCardDto>();

            if (!addCardDto.PinCode.All(char.IsDigit) || addCardDto.PinCode.Length != 4)
                return serviceResponse.CreateErrorResponse(null!, $"PinCode '{addCardDto.PinCode}' in not valid. It must contain digits and contain 4 numbers",
                    HttpStatusCode.UnprocessableEntity);


            var user = await userToFind.FindUser(_context);

            if (user == null)
                return serviceResponse.CreateErrorResponse(new GetCardDto(), "Unable to find the user.", HttpStatusCode.NotFound);

            if (addCardDto.PaymentSystem == null)
                return serviceResponse.CreateErrorResponse(new GetCardDto(), "Please, choose the payment System", HttpStatusCode.BadRequest);

            if (user.Card != null)
                return serviceResponse.CreateErrorResponse(new GetCardDto(), "The user has a card already.", HttpStatusCode.BadRequest);

            try
            {
                var cardNumber = await _luhnRepository.GenerateUniqueCardNumber(addCardDto.PaymentSystem);

                HashingExtension.CreateHash(addCardDto.PinCode, out byte[] pinHash, out byte[] pinSalt);
                HashingExtension.CreateHash(addCardDto.PinCode, out byte[] CVVHash, out byte[] CVVSalt);

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
                serviceResponse.CreateErrorResponse(null!, ex.Message, HttpStatusCode.InternalServerError);
            }

            return serviceResponse;
        }
    }
}
