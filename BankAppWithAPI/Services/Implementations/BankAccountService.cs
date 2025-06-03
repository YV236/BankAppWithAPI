using AutoMapper;
using BankAppWithAPI.Data;
using BankAppWithAPI.Dtos.BankAccount;
using BankAppWithAPI.Extensions;
using BankAppWithAPI.Models;
using BankAppWithAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Numerics;
using System.Text;

namespace BankAppWithAPI.Services.Implementations
{
    public class BankAccountService(DataContext _context, IMapper _mapper, ILuhnNumberService _luhnRepository) : IBankAccountService
    {
        public async Task<ServiceResponse<GetBankAccountDto>> GetConcreteBankAccount(ClaimsPrincipal user)
        {
            var serviceResponse = new ServiceResponse<GetBankAccountDto>();

            try
            {
                var getUser = await user.FindUser(_context);

                if (getUser == null)
                    return serviceResponse.CreateErrorResponse(new GetBankAccountDto(), "Unable to find the user.", HttpStatusCode.NotFound);

                if (getUser.AccountCards!.Count == 0)
                    return serviceResponse.CreateErrorResponse(new GetBankAccountDto(), "You don't have any bank accounts at the moment.", HttpStatusCode.NotFound);

                var bankAccountDto = _mapper.Map<GetBankAccountDto>(getUser.AccountCards
                    .FirstOrDefault(ac => ac.Account!.IsActive == true)!.Account);

                serviceResponse.Data = bankAccountDto;
                serviceResponse.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                serviceResponse.CreateErrorResponse(new GetBankAccountDto(), ex.Message, HttpStatusCode.InternalServerError);
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetBankAccountDto>> CreateBankAccount(CreateBankAccountDto bankAccountDto, ClaimsPrincipal user)
        {
            var serviceResponse = new ServiceResponse<GetBankAccountDto>();

            try
            {
                var iban = await _luhnRepository.GenerateUniqueIBAN();

                var newBankAccount = new BankAccount
                {
                    IBAN = iban,
                    AccountName = bankAccountDto.AccountName,
                    DateOfCreation = DateTime.UtcNow,
                };

                var bankAccountCard = new BankAccountCard();
                bankAccountCard.Account = newBankAccount;
                bankAccountCard.User = await user.FindUser(_context);

                if (bankAccountCard.User.AccountCards!.Count == 0)
                    bankAccountCard.Account.IsActive = true;

                if (bankAccountCard.User == null)
                    return serviceResponse.CreateErrorResponse(new GetBankAccountDto(), "Unable to find the user.", HttpStatusCode.NotFound);

                if (bankAccountCard.User.Card == null)
                    return serviceResponse.CreateErrorResponse(new GetBankAccountDto(), "A card is required before a user can create a bank account.", HttpStatusCode.InternalServerError);

                bankAccountCard.Card = bankAccountCard.User.Card;

                _context.BankAccountCards.Add(bankAccountCard);
                _context.BankAccounts.Add(newBankAccount);
                await _context.SaveChangesAsync();

                var getBankAccount = _mapper.Map<GetBankAccountDto>(newBankAccount);

                serviceResponse.Data = getBankAccount;
                serviceResponse.IsSuccessful = true;

            }
            catch (Exception ex)
            {
                serviceResponse.CreateErrorResponse(new GetBankAccountDto(), ex.Message, HttpStatusCode.InternalServerError);
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetBankAccountDto>>> GetUserBankAccounts(ClaimsPrincipal user)
        {
            var serviceResponse = new ServiceResponse<List<GetBankAccountDto>>();

            try
            {
                var getUser = await user.FindUser(_context);

                if (getUser == null)
                    return serviceResponse.CreateErrorResponse(new List<GetBankAccountDto>(), "Unable to find the user.", HttpStatusCode.NotFound);

                if (getUser.AccountCards!.Count == 0)
                    return serviceResponse.CreateErrorResponse(new List<GetBankAccountDto>(), "You don't have any bank accounts at the moment.", HttpStatusCode.NotFound);

                var bankAccountsDto = new List<GetBankAccountDto>();

                foreach (var bankAccount in getUser.AccountCards!)
                {
                    bankAccountsDto.Add(_mapper.Map<GetBankAccountDto>(bankAccount.Account));
                }

                serviceResponse.Data = bankAccountsDto;
                serviceResponse.IsSuccessful = true;

            }
            catch (Exception ex)
            {
                serviceResponse.CreateErrorResponse(new List<GetBankAccountDto>(), ex.Message, HttpStatusCode.InternalServerError);
            }

            return serviceResponse;
        }
    }
}
