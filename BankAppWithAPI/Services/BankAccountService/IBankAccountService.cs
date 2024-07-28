using BankAppWithAPI.Dtos.BankAccount;
using BankAppWithAPI.Models;

namespace BankAppWithAPI.Services.BankAccountService
{
    public interface IBankAccountService
    {
        Task<ServiceResponse<GetBankAccountDto>> CreateBankAccount(CreateBankAccountDto bankAccount);
    }
}
