using BankAppWithAPI.Models;
using BankAppWithAPI.Dtos.Operation;

namespace BankAppWithAPI.Services.OperationService
{
    public interface IOperationService
    {
        Task<ServiceResponse<OperationResultDto>> Deposit(OperationRequestDto request, ClaimsPrincipal card);
        Task<ServiceResponse<OperationResultDto>> Withdraw(OperationRequestDto request, ClaimsPrincipal card);
        Task<ServiceResponse<OperationResultDto>> Transfer(OperationRequestDto request);
    }
}
