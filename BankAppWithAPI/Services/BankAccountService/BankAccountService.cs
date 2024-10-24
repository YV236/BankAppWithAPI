using AutoMapper;
using BankAppWithAPI.Data;
using BankAppWithAPI.Dtos.BankAccount;
using BankAppWithAPI.Extensions;
using BankAppWithAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Numerics;
using System.Text;

namespace BankAppWithAPI.Services.BankAccountService
{
    public class BankAccountService(DataContext _context, IMapper _mapper) : IBankAccountService
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
                var iban = await GenerateUniqueIBAN();

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

                if(bankAccountCard.User.Card == null)
                    return serviceResponse.CreateErrorResponse(new GetBankAccountDto(), "A card is required before a user can create a bank account.", HttpStatusCode.InternalServerError);

                bankAccountCard.Card = bankAccountCard.User.Card;

                _context.BankAccountCards.Add(bankAccountCard);
                _context.BankAccounts.Add(newBankAccount);
                await _context.SaveChangesAsync();

                var getBankAccount = _mapper.Map<GetBankAccountDto>(newBankAccount);

                serviceResponse.Data = getBankAccount;
                serviceResponse.IsSuccessful = true;

            }catch(Exception ex)
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

                if(getUser.AccountCards!.Count == 0)
                    return serviceResponse.CreateErrorResponse(new List<GetBankAccountDto>(), "You don't have any bank accounts at the moment.", HttpStatusCode.NotFound);

                var bankAccountsDto = new List<GetBankAccountDto>();

                foreach (var bankAccount in getUser.AccountCards!)
                {
                    bankAccountsDto.Add(_mapper.Map<GetBankAccountDto>(bankAccount.Account));
                }

                serviceResponse.Data = bankAccountsDto;
                serviceResponse.IsSuccessful = true;

            }
            catch(Exception ex)
            {
                serviceResponse.CreateErrorResponse(new List<GetBankAccountDto>(), ex.Message, HttpStatusCode.InternalServerError);
            }

            return serviceResponse;
        }

        private async Task<string> GenerateUniqueIBAN()
        {
            string countryCode = "PL";
            string bankCode = "30102521";
            string iban = "";
            bool isUnique;

            do
            {
                iban = GenerateIBAN(countryCode, bankCode);
                isUnique = !await _context.BankAccounts.AnyAsync(a => a.IBAN == iban);
            }
            while (!isUnique);

            return iban;
        }

        //Method for IBAN generation
        private string GenerateIBAN(string countryCode, string bankCode)
        {
            bool check = false;
            string result = "";
            while (!check)
            {
                string accountNumber = GenerateRandomAccountNumber();
                // Initial IBAN with check digits 00
                string iban = countryCode + "00" + bankCode + accountNumber;

                // Calculation of check digits
                int checkDigits = CalculateCheckDigits(iban);

                // Formatting check digits
                string formattedCheckDigits = checkDigits.ToString("D2");
                result = countryCode + formattedCheckDigits + bankCode + accountNumber;

                if (ValidateLuhnCheck(result))
                    check = true;
            }

            // Return of final IBAN
            return result;
        }

        private string GenerateRandomAccountNumber()
        {
            var random1 = new Random();
            return random1.Next(10000000, 99999999).ToString("D8") + random1.Next(10000000, 99999999).ToString("D8");
        }

        // Method for calculating check digits
        private int CalculateCheckDigits(string iban)
        {
            // Move the country code and check digits to the end
            string rearrangedIBAN = iban.Substring(4) + iban.Substring(0, 4);

            // Replacing letters with numerical values
            string numericIBAN = ReplaceLettersWithNumbers(rearrangedIBAN);

            // Conversion to BigInteger to execute module 97
            BigInteger ibanNumber = BigInteger.Parse(numericIBAN);
            int remainder = (int)(ibanNumber % 97);

            // Calculation of check digits
            int checkDigits = 98 - remainder;
            return checkDigits;
        }

        private string ReplaceLettersWithNumbers(string input)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in input)
            {
                if (char.IsLetter(c))
                {
                    int numericValue = c - 'A' + 10;
                    sb.Append(numericValue);
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        private bool ValidateLuhnCheck(string number)
        {
            int sum = 0;
            bool alternate = false;

            string numericIBAN = ReplaceLettersWithNumbers(number);

            for (int i = numericIBAN.Length - 1; i >= 0; i--)
            {
                int n = int.Parse(numericIBAN[i].ToString());

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
    }
}
