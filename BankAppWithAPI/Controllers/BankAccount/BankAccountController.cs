using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using BankAppWithAPI.Dtos.User;
using Microsoft.AspNetCore.Mvc;
using BankAppWithAPI.Data;
using BankAppWithAPI.Models;
using BankAppWithAPI.Services.BankAccountService;
using BankAppWithAPI.Dtos.BankAccount;

namespace BankAppWithAPI.Controllers.BankAccount
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class BankAccountController(IBankAccountService bankAccountService) : ControllerBase
    {
        [HttpPost("create")]
        public async Task<ActionResult<ServiceResponse<Models.BankAccount>>> CreateBankAccount(CreateBankAccountDto createBankAccountDto)
        {
            var response = await bankAccountService.CreateBankAccount(createBankAccountDto);

            if (!response.IsSuccessful)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}
