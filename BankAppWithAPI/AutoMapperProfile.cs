using AutoMapper;
using BankAppWithAPI.Dtos.User;
using BankAppWithAPI.Models;

namespace BankAppWithAPI
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, GetUserDto>();
            CreateMap<UserRegisterDto, User>();
        }
    }
}