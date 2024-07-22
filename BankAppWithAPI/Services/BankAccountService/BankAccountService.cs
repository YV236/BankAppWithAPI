using BankAppWithAPI.Data;
using BankAppWithAPI.Dtos.BankAccount;
using BankAppWithAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BankAppWithAPI.Services.BankAccountService
{
    public class BankAccountService(DataContext _context) : IBankAccountService
    {
        public async Task<ServiceResponse<BankAccount>> CreateBankAccount(CreateBankAccountDto bankAccountDto)
        {
            var serviceResponse = new ServiceResponse<BankAccount>();

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

            // Додавання нового рахунку в базу даних
            _context.BankAccounts.Add(newBankAccount);
            _context.BankAccountCards.Add(new BankAccountCard { 
                AccountId=newBankAccount.Id,
                Account=newBankAccount});
            await _context.SaveChangesAsync();

            serviceResponse.Data = newBankAccount;
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
    }
}
