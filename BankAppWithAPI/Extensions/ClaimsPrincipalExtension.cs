using BankAppWithAPI.Data;
using BankAppWithAPI.Models;

namespace BankAppWithAPI.Extensions
{
    public static class ClaimsPrincipalExtension
    {
        public static async Task<User> FindUser(this ClaimsPrincipal userToFind, DataContext _context)
        {            
            var userId = userToFind.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var getUser = await _context.Users.Include(u => u.Card!).Include(u => u.AccountCards!).ThenInclude(ac => ac.Account)
                .FirstOrDefaultAsync(u => u.Id == userId);

            return getUser!;
        }
    }
}
