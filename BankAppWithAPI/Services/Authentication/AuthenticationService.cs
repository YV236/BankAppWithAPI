
using BankAppWithAPI.Data;
using BankAppWithAPI.Models;

namespace BankAppWithAPI.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly DataContext _context;

        public AuthenticationService(DataContext context)
        {
            _context = context;
        }


        public Task<ServiceResponse<string>> Login(string username, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<int>> Register(User user, string password)
        {
            var response = new ServiceResponse<int>();

            //if (await UserExists(user.UserName))
            //{
            //    response.IsSuccessful = false;
            //    response.Message = "User with specify name already exist";
            //    return response;
            //}

            //CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            //user.PasswordHash = passwordHash;
            //user.PasswordSalt = passwordSalt;

            //_context.Users.Add(user);
            //await _context.SaveChangesAsync();
            //response.Data = user.Id;
            return response;
        }

        public Task<bool> UserExists(string username)
        {
            throw new NotImplementedException();
        }

    }
}
