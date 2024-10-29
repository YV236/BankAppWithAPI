using AutoMapper;
using BankAppWithAPI.Data;
using BankAppWithAPI.Dtos.BankAccount;
using BankAppWithAPI.Dtos.Operation;
using BankAppWithAPI.Extensions;
using BankAppWithAPI.Models;
using BankAppWithAPI.Models.Operations;
using System.Net;

namespace BankAppWithAPI.Services.OperationService
{
    public class OperationService(DataContext _context, IMapper _mapper) : IOperationService
    {
        public async Task<ServiceResponse<OperationResultDto>> Deposit(OperationRequestDto request)
        {
            var serviceResponse = new ServiceResponse<OperationResultDto>();

            try
            {
                var account = await _context.BankAccounts.FirstOrDefaultAsync(b => request.IBAN == b.IBAN);

                if (account == null)
                    return serviceResponse.CreateErrorResponse(new OperationResultDto(), "Account not found.", HttpStatusCode.NotFound);

                account.Balance += request.Amount;

                var deposit = new DepositOperation
                {
                    AccountId = account.Id,
                    Amount = request.Amount,
                    BalanceAfter = account.Balance,
                    Account = account,
                    OperationDate = DateTime.UtcNow,
                };

                _context.Operations.Add(deposit);
                await _context.SaveChangesAsync();

                var result = _mapper.Map<OperationResultDto>(deposit);

                serviceResponse.Data = result;
                serviceResponse.IsSuccessful = true;
                serviceResponse.Message = "Money successfully deposited to your account";

            }
            catch (Exception ex)
            {
                serviceResponse.CreateErrorResponse(new OperationResultDto(), ex.Message, HttpStatusCode.InternalServerError);
            }

            return serviceResponse;
        }

        public Task<ServiceResponse<OperationResultDto>> Transfer(OperationRequestDto request)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<OperationResultDto>> Withdraw(OperationRequestDto request)
        {
            throw new NotImplementedException();
        }
    }
}
