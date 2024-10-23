using BankAppWithAPI.Dtos.BankAccount;
using BankAppWithAPI.Models;

namespace BankAppWithAPI.Services.BankAccountService
{
    public interface IBankAccountService
    {
        Task<ServiceResponse<GetBankAccountDto>> CreateBankAccount(CreateBankAccountDto bankAccount, ClaimsPrincipal user);
        Task<ServiceResponse<List<GetBankAccountDto>>> GetUserBankAccounts(ClaimsPrincipal user);
        Task<ServiceResponse<GetBankAccountDto>> GetConcreteBankAccount(ClaimsPrincipal user);
    }
}
