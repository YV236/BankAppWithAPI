namespace BankAppWithAPI.Services.UserServices
{
    public interface IUserService
    {
        Task<ServiceResponse<GetUserDto>> GetUserByEmail(string email);
    }
}
