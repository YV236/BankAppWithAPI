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
        public async Task<ServiceResponse<OperationResultDto>> Deposit(int amount, ClaimsPrincipal card)
        {
            var serviceResponse = new ServiceResponse<OperationResultDto>();

            try
            {
                var account = await card.FindCardActiveAccount(_context);

                if (account == null)
                    return serviceResponse.CreateErrorResponse(new OperationResultDto(), "Account not found.", HttpStatusCode.NotFound);

                if (amount < 0)
                    throw new Exception("Sorry, something went wrong");

                account.Balance += amount;

                var deposit = new DepositOperation
                {
                    AccountId = account.Id,
                    Amount = amount,
                    BalanceAfter = account.Balance,
                    Account = account,
                    OperationDate = DateTime.UtcNow,
                };

                _context.Operations.Add(deposit);
                await _context.SaveChangesAsync();

                var result = new OperationResultDto
                {
                    IBAN = deposit.Account.IBAN,
                    AccountName = deposit.Account.AccountName,
                    Amount = amount,
                    BalanceAfter = deposit.BalanceAfter,
                    OperationDate = deposit.OperationDate,
                    OperationType = deposit.OperationType,
                };

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

        public async Task<ServiceResponse<OperationResultDto>> Transfer(TransferRequestDto request, ClaimsPrincipal user)
        {
            var serviceResponse = new ServiceResponse<OperationResultDto>();

            try
            {
                var fromAccount = await user.FindUserActiveAccount(_context);

                if(fromAccount == null)
                    return serviceResponse.CreateErrorResponse(new OperationResultDto(), "Account not found.", HttpStatusCode.NotFound);

                if (request.Amount > fromAccount!.Balance)
                    return serviceResponse.CreateErrorResponse(new OperationResultDto(), "You don't have enough funds", HttpStatusCode.BadRequest);

                var toAccount = await _context.BankAccounts.FirstOrDefaultAsync(ac => ac.IBAN == request.DestinationIBAN);

                if (toAccount == null)
                    return serviceResponse.CreateErrorResponse(new OperationResultDto(), "Account not found.", HttpStatusCode.NotFound);

                fromAccount.Balance -= request.Amount;
                toAccount.Balance += request.Amount;

                var transfer = new TransferOperation
                {
                    AccountId = fromAccount.Id,
                    Amount = request.Amount,
                    BalanceAfter = fromAccount.Balance,
                    Account = fromAccount,
                    DestinationAccount = toAccount,
                    OperationDate = DateTime.UtcNow,
                };

                _context.Operations.Add(transfer);
                await _context.SaveChangesAsync();

                var result = new OperationResultDto
                {
                    IBAN = transfer.Account.IBAN,
                    AccountName = transfer.Account.AccountName,
                    Amount = transfer.Amount,
                    BalanceAfter = transfer.BalanceAfter,
                    OperationDate = transfer.OperationDate,
                    OperationType = transfer.OperationType,
                };

                serviceResponse.Data = result;
                serviceResponse.IsSuccessful = true;
                serviceResponse.Message = $"{transfer.Amount} successfully transferred to '{transfer.DestinationAccount.IBAN}' account";

            }
            catch(Exception ex)
            {
                serviceResponse.CreateErrorResponse(new OperationResultDto(), ex.Message, HttpStatusCode.InternalServerError);
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<OperationResultDto>> Withdraw(int amount, ClaimsPrincipal card)
        {
            var serviceResponse = new ServiceResponse<OperationResultDto>();

            try
            {
                var account = await card.FindCardActiveAccount(_context);

                if (account == null)
                    return serviceResponse.CreateErrorResponse(new OperationResultDto(), "Account not found.", HttpStatusCode.NotFound);

                if (amount > account!.Balance)
                    return serviceResponse.CreateErrorResponse(new OperationResultDto(), "You don't have enough funds", HttpStatusCode.BadRequest);

                account.Balance -= amount;

                var withdraw = new WithdrawOperation
                {
                    AccountId = account.Id,
                    Amount = amount,
                    BalanceAfter = account.Balance,
                    Account = account,
                    OperationDate = DateTime.UtcNow,
                };

                _context.Operations.Add(withdraw);
                await _context.SaveChangesAsync();

                var result = new OperationResultDto
                {
                    IBAN = withdraw.Account.IBAN,
                    AccountName = withdraw.Account.AccountName,
                    Amount = amount,
                    BalanceAfter = withdraw.BalanceAfter,
                    OperationDate = withdraw.OperationDate,
                    OperationType = withdraw.OperationType,
                };

                serviceResponse.Data = result;
                serviceResponse.IsSuccessful = true;
                serviceResponse.Message = "Money successfully withdrawn from your account";

            }
            catch (Exception ex)
            {
                serviceResponse.CreateErrorResponse(new OperationResultDto(), ex.Message, HttpStatusCode.InternalServerError);
            }

            return serviceResponse;
        }
    }

}
