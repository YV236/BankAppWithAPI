using Azure;
using BankAppWithAPI.Dtos.Operation;
using BankAppWithAPI.Models;
using BankAppWithAPI.Services.OperationService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankAppWithAPI.Controllers.BankAccount
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class OperationsController(IOperationService _operationService) : ControllerBase
    {
        [HttpPost("Deposit")]
        public async Task<ActionResult<ServiceResponse<OperationResultDto>>> Deposit(OperationRequestDto request, string CardNumber)
        {
            var response = await _operationService.Deposit(request, CardNumber);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost("Withdraw")]
        public async Task<ActionResult<ServiceResponse<OperationResultDto>>> Withdraw(OperationRequestDto request, string CardNumber)
        {
            var response = await _operationService.Withdraw(request, CardNumber);

            return StatusCode((int)response.StatusCode, response);
        }

    }
}
