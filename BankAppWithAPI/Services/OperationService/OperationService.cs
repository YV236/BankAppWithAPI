using AutoMapper;
using BankAppWithAPI.Data;
using BankAppWithAPI.Dtos.Operation;
using BankAppWithAPI.Models;

namespace BankAppWithAPI.Services.OperationService
{
    public class OperationService(DataContext _context, IMapper _mapper) : IOperationService
    {
        public Task<ServiceResponse<OperationResultDto>> Deposit(OperationRequestDto request)
        {
            throw new NotImplementedException();
        }
    }
}
