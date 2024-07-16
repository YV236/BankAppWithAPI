using BankAppWithAPI.Dtos.User;
using BankAppWithAPI.Models;

namespace BankAppWithAPI.Services.UserServices
{
    public interface IUserService
    {
        Task<ServiceResponse<GetUserDto>> GetUserInfo(ClaimsPrincipal user);
        Task<ServiceResponse<int>> Register(UserRegisterDto userRegisterDto);
        Task<ServiceResponse<GetUserDto>> UpdateUserInfo(ClaimsPrincipal user, UpdateUserDto userUpdateDto);
    }
}
