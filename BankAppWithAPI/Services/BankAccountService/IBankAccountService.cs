using BankAppWithAPI.Dtos.BankAccount;
using BankAppWithAPI.Models;

namespace BankAppWithAPI.Services.BankAccountService
{
    public interface IBankAccountService
    {
        Task<ServiceResponse<BankAccount>> CreateBankAccount(CreateBankAccountDto bankAccount);
    }
}
