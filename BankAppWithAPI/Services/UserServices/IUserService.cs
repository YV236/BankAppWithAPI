namespace BankAppWithAPI.Services.UserServices
{
    public interface IUserService
    {
        Task<ServiceResponse<GetUserDto>> GetUserInfo(ClaimsPrincipal user);
        Task<ServiceResponse<int>> AddMoreInfo(ClaimsPrincipal user, UserRegisterDto userRegisterDto);
    }
}
