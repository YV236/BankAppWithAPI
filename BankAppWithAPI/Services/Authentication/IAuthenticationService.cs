using BankAppWithAPI.Models;

namespace BankAppWithAPI.Services.Authentication
{
    public interface IAuthenticationService
    {
        Task<ServiceResponse<int>> Register(User user, string password);
        Task<ServiceResponse<string>> Login(string username, string password);
        Task<bool> UserExists(string username);
    }
}
