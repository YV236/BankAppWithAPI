using BankAppWithAPI.Models;
using BankAppWithAPI.Dtos.Operation;

namespace BankAppWithAPI.Services.OperationService
{
    public interface IOperationService
    {
        Task<ServiceResponse<OperationResultDto>> Deposit(OperationRequestDto request);
    }
}
