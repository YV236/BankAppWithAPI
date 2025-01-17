using BankAppWithAPI.Models;
using BankAppWithAPI.Dtos.Operation;

namespace BankAppWithAPI.Services.OperationService
{
    public interface IOperationService
    {
        Task<ServiceResponse<OperationResultDto>> Deposit(int amount, ClaimsPrincipal card);
        Task<ServiceResponse<OperationResultDto>> Withdraw(int amount, ClaimsPrincipal card);
        Task<ServiceResponse<OperationResultDto>> Transfer(TransferRequestDto request, ClaimsPrincipal user);
    }
}
