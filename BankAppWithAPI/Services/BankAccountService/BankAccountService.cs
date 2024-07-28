using AutoMapper;
using BankAppWithAPI.Data;
using BankAppWithAPI.Dtos.BankAccount;
using BankAppWithAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BankAppWithAPI.Services.BankAccountService
{
    public class BankAccountService(DataContext _context, IHttpContextAccessor _httpContextAccessor, IMapper _mapper) : IBankAccountService
    {
        public async Task<ServiceResponse<GetBankAccountDto>> CreateBankAccount(CreateBankAccountDto bankAccountDto)
        {
            var serviceResponse = new ServiceResponse<GetBankAccountDto>();

            // Генерація унікального номера рахунку
            var accountNumber = await GenerateUniqueAccountNumber();

            // Створення нового об'єкта BankAccount
            var newBankAccount = new BankAccount
            {
                AccountNumber = accountNumber,
                AccountPriority = bankAccountDto.AccountPriority,
                AccountName = bankAccountDto.AccountName,
                DateOfCreation = DateTime.UtcNow,
                // Додаткові поля можна заповнити тут
            };
            var bankAccountCard = new BankAccountCard();
            bankAccountCard.Account = newBankAccount;
            bankAccountCard.User = await _context.Users.Include(u => u.Card)
                .FirstOrDefaultAsync(u => u.Id == FindUserId());
            bankAccountCard.Card = bankAccountCard.User.Card;

            // Додавання нового рахунку в базу даних
            _context.BankAccountCards.Add(bankAccountCard);
            _context.BankAccounts.Add(newBankAccount);
            await _context.SaveChangesAsync();

            var getBankAccount= _mapper.Map<GetBankAccountDto>(newBankAccount);

            serviceResponse.Data = getBankAccount;
            serviceResponse.IsSuccessful = true;
            return serviceResponse;
        }


        private async Task<string> GenerateUniqueAccountNumber()
        {
            string accountNumber;
            bool isUnique;

            do
            {
                // Генерація випадкового номера рахунку
                accountNumber = GenerateRandomAccountNumber();
                // Перевірка наявності номера рахунку в базі даних
                isUnique = !await _context.BankAccounts.AnyAsync(a => a.AccountNumber == accountNumber);
            }
            while (!isUnique);

            return accountNumber;
        }

        private string GenerateRandomAccountNumber()
        {
            // Генерація випадкового числа певної довжини, наприклад, 10 цифр
            var random = new Random();
            return random.Next(100000000, 999999999).ToString("D10");
        }

        private string FindUserId() => _httpContextAccessor.HttpContext!.User
            .FindFirstValue(ClaimTypes.NameIdentifier)!;
    }
}
