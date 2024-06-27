using AutoMapper;

namespace BankAppWithAPI
{
    public class AutoMapperProfile : Profile
    {

        public AutoMapperProfile()
        {
            CreateMap<User, GetUserDto>();
            CreateMap<GetUserDto, User>();
        }
    }
}
