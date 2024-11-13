using BankAppWithAPI.Models;
using BankAppWithAPI.Dtos.Operation;

namespace BankAppWithAPI.Services.OperationService
{
    public interface IOperationService
    {
        Task<ServiceResponse<OperationResultDto>> Deposit(OperationRequestDto request, string card, ClaimsPrincipal user);
        Task<ServiceResponse<OperationResultDto>> Withdraw(OperationRequestDto request, string card);
        Task<ServiceResponse<OperationResultDto>> Transfer(OperationRequestDto request);
    }
}
