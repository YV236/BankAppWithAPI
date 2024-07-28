using AutoMapper;
using BankAppWithAPI.Dtos.BankAccount;
using BankAppWithAPI.Dtos.Card;
using BankAppWithAPI.Dtos.User;
using BankAppWithAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace BankAppWithAPI
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, GetUserDto>();
            CreateMap<UserRegisterDto, User>();
            CreateMap<IdentityResult, ServiceResponse<object>>();
            CreateMap<UpdateUserDto, User>();
            CreateMap<Card, GetCardDto>();
            CreateMap<BankAccount, GetBankAccountDto>();
        }
    }
}