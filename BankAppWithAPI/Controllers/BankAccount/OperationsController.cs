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
    public class OperationsController(IOperationService _operationService) : ControllerBase
    {
        //TODO add use of ClaimsPrincipal
        [HttpPost("Deposit")]
        [Authorize(AuthenticationSchemes = "MyTokenScheme")]
        public async Task<ActionResult<ServiceResponse<OperationResultDto>>> Deposit(OperationRequestDto request, string CardNumber)
        {
            var response = await _operationService.Deposit(request, CardNumber, User);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost("Withdraw")]
        [Authorize(AuthenticationSchemes = "MyTokenScheme")]
        public async Task<ActionResult<ServiceResponse<OperationResultDto>>> Withdraw(OperationRequestDto request, string CardNumber)
        {
            var response = await _operationService.Withdraw(request, CardNumber);

            return StatusCode((int)response.StatusCode, response);
        }

    }
}
