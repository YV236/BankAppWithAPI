using BankAppWithAPI.Data;
using BankAppWithAPI.Models;

namespace BankAppWithAPI.Extensions
{
    public static class ClaimsPrincipalExtension
    {
        public static async Task<User> FindUser(this ClaimsPrincipal userToFind, DataContext context)
        {
            var userId = userToFind.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var getUser = await context.Users.Include(u => u.Card!).Include(u => u.AccountCards!).ThenInclude(ac => ac.Account)
                .FirstOrDefaultAsync(u => u.Id.ToString() == userId);

            return getUser!;
        }

        public static async Task<BankAccount> FindActiveAccount(this ClaimsPrincipal cardToFind, DataContext context)
        {
            var cardId = cardToFind.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var getCard = await context.Cards.Include(c => c.AccountCards).ThenInclude(ac => ac.Account)
                .FirstOrDefaultAsync(c => c.Id.ToString() == cardId);

            return getCard!.AccountCards.FirstOrDefault(ac => ac.Account!.IsActive == true)!.Account!;
        }
    }
}
